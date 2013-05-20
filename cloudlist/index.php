<?php
/*
 * simple cloud list server script for XBSlink
* Version v1.9
* by Oliver Seuffert 2012
*
* php sqlite3 plugin is needed
*
*  Changelog:
*  v1.9
*   - changed to MySQL database, TIMESTAMP in DB
*  v1.8
*   - added check for minimum client version
*  v1.7.1
*   - bugfix in updateNode
*  v1.7
*   - send nodelist to client on UPDATE
*  v1.6.1
*   - serious bugfix in function joinCloud
*  v1.6
*   - new handling when client joins a cloud
*   - new GET param getallnodes for join/create cloud
*   - rejoining known client bug fixed, known UUID is send back
*  v1.5.1
*   - bug fix in function joinCloud
*  v1.5
*   - only "open port" clients can create clouds
*  v1.4.1
*   - case insensitve sort for cloud names
*  v1.4
*   - added PING to nodes on join
*  v1.3
*   - added UDP code for HELLO message
*   - changed TABLE layout, added "reachable" column to nodes
*   - on join, try to return a reachable node
*  v1.2
*   - added STATS command
*  v1.1
*   - added detection of privat IP subnets in announced IPs
*  v1.0
*   - initial release
*/

// GET parameters
define('PARAM_CMD', 		'cmd');
define('PARAM_CLOUDNAME', 	'cloudname');
define('PARAM_PASSWORD', 	'password');
define('PARAM_MAXNODES', 	'maxnodes');
define('PARAM_NODEIP', 		'node_ip');
define('PARAM_NODEPORT', 	'node_port');
define('PARAM_NICKNAME', 	'nick');
define('PARAM_UUID', 		'uuid');
define('PARAM_GETALLNODES', 'getallnodes');
define('PARAM_CLIENTVERSION', 'clientversion');

// commands send by client
define('CMD_GETLIST',	'GETLIST');
define('CMD_JOIN',		'JOIN');
define('CMD_LEAVE',		'LEAVE');
define('CMD_UPDATE',	'UPDATE');
define('CMD_STATS',		'STATS');
define('CMD_SENDHELLO',	'SENDHELLO');

// DB settings
define('DB_FILENAME',	'XBSlink_clouds.sqlite3');
define('TABLE_CLOUDS', 	'clouds');
define('TABLE_NODES', 	'nodes');
define('DB_SERVER', 	'server1.secudb.de');
define('DB_NAME', 		'oli_xbslink_cloudlist');
define('DB_USERNAME', 	'xbslink');
define('DB_PASSWORD', 	'cJqQXfQ4auNy44AY');

// misc settings
define('MIN_CLOUDNAME_LENGTH', 2);
define('MIN_NICKNAME_LENGTH', 2);
define('MAX_NO_ACTION_TIME', 70);
define('MIN_CLIENTVERSION', '0.9.5.3');

// return codes
define('RETURN_CODE_ERROR',	'ERROR:');
define('RETURN_CODE_OK',	'OK:');

// UDP message commands
define ('UDP_MESSAGE_HELLO', 0xFF);
define ('UDP_MESSAGE_PING', 0x04);

class xbslink_cloudlist_server
{
	/**
	 * PDO database connection
	 * @var PDO
	 */
	private $db = null;

	// open or create DB file and create tables
	private function openDB()
	{
		try {
			$this->db = new PDO(
					'mysql:host='.DB_SERVER.';dbname='.DB_NAME, DB_USERNAME, DB_PASSWORD,
					array( PDO::ATTR_PERSISTENT => true)
			);
		}
		catch (PDOException $pe)
		{
			return false;
		}
		if ($this->db == null || !($this->db instanceof PDO))
			return false;
		return $this->createTables();
	}

	// create tables if they do not exist
	private function createTables()
	{
		$sql = "CREATE TABLE IF NOT EXISTS `".TABLE_CLOUDS."` (
				`id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY ,
				`name` VARCHAR( 255 ) NOT NULL ,
				`max_nodes` INT UNSIGNED NOT NULL ,
				`lastaction` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
				`password_hash` VARCHAR( 255 ) NOT NULL ,
				INDEX (  `lastaction` ) ,
				UNIQUE (
				`name`
				)
				);";
		if ($this->db->exec($sql)===false) {
			echo "ERROR!<br>\n".$sql."<br>\n";
			print_r($this->db->errorInfo(), true);
			return false;
		}
		$sql = "CREATE TABLE IF NOT EXISTS `".TABLE_NODES."` (
				`id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY ,
				`cloud_id` INT UNSIGNED NOT NULL ,
				`nickname` VARCHAR( 255 ) NOT NULL ,
				`ip` INT UNSIGNED NOT NULL ,
				`port` INT UNSIGNED NOT NULL ,
				`lastaction` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
				`uuid` BIGINT( 10 ) UNSIGNED NOT NULL ,
				`reachable` INT UNSIGNED NOT NULL ,
				INDEX (  `cloud_id` ,  `lastaction` ,  `reachable` ) ,
				UNIQUE (
				`uuid`
				)
				);";
		if (!$this->db->exec($sql)===false) {
			echo "ERROR!<br>\n".$sql."<br>\n";
			print_r($this->db->errorInfo(), true);
			return false;
		}
		return true;
	}

	/**
	 * returns the current cloud list to the client
	 *
	 * Format example with 3 clouds in the list, values ar URL encoded:
	 * OK:cloudname=CLOUD1&maxnodes=10&password=False&count_nodes=4\n
	 * cloudname=CLOUD2&maxnodes=200&password=True&count_nodes=10\n
	 * cloudname=another%20cloud&maxnodes=5&password=True&count_nodes=1\n
	 * @return String
	 */
	private function loadCloudList()
	{
		//$sql = "SELECT clouds.*, COUNT(nodes.id) as countnodes FROM ".TABLE_CLOUDS." AS clouds JOIN ".TABLE_NODES." as nodes WHERE clouds.lastaction>:min_lastaction AND nodes.lastaction>:min_lastaction AND nodes.cloud_id=clouds.id GROUP BY clouds.id, clouds.name, clouds.max_nodes, clouds.password_hash ORDER BY clouds.name COLLATE NOCASE";
		$sql = "SELECT clouds.*, COUNT(nodes.id) as countnodes FROM ".TABLE_CLOUDS." AS clouds JOIN ".TABLE_NODES." as nodes WHERE (DATE_ADD(clouds.`lastaction`, INTERVAL ".MAX_NO_ACTION_TIME." SECOND) > NOW()) AND (DATE_ADD(nodes.`lastaction`, INTERVAL ".MAX_NO_ACTION_TIME." SECOND) > NOW()) AND nodes.cloud_id=clouds.id GROUP BY clouds.id, clouds.name, clouds.max_nodes, clouds.password_hash ORDER BY LOWER(clouds.name)";
		$sth = $this->db->prepare($sql);
		$ret = $sth->execute( array() );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (lCL)\n";
			return $ret;
		}
		$rows = $sth->fetchAll(PDO::FETCH_ASSOC);
		$ret = RETURN_CODE_OK."\n";
		foreach ($rows as $row)
			$ret .= "cloudname=".urlencode($row['name'])."&countnodes=".urlencode($row['countnodes'])."&maxnodes=".urlencode($row['max_nodes'])."&password=".(strlen($row['password_hash'])>0 ? 'True' : 'False')."\n" ;
		return $ret;
	}

	// create a new cloud and join the client
	private function createCloud( $cloudname, $password, $maxnodes, $node_ip, $node_port, $nickname)
	{
		if (!is_numeric($maxnodes) || $maxnodes<=0)
			return RETURN_CODE_ERROR."maxnodes must be a positive number";
		$host_reachable = ($this->sendPingToNode($node_ip, $node_port, 2)) ? 1 : 0;
		if (!$host_reachable)
			return RETURN_CODE_ERROR."client unreachable!";
		$sth = $this->db->prepare("INSERT INTO ".TABLE_CLOUDS." (name, max_nodes, lastaction, password_hash) VALUES (:cloudname, :maxnodes, NOW(), :password)");
		$ret = $sth->execute( array(':cloudname'=>$cloudname, ':maxnodes'=>$maxnodes, ':password'=>$password) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (cC)\n";
			return $ret;
		}
		return $this->joinCloud($this->db->lastInsertId(), $password, $node_ip, $node_port, $nickname, false);
	}

	/**
	 * returns an array of all nodes in a cloud sorted by reachable status (reachable first)
	 * @param int $cloud_id
	 */
	private function getAllNodesFromCloud( $cloud_id )
	{
		$sth = $this->db->prepare("SELECT * FROM ".TABLE_NODES." WHERE cloud_id=:cloudid ORDER BY reachable DESC");
		$ret = $sth->execute( array(':cloudid' => $cloud_id ) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (gANFC)\n";
			return null;
		}
		$rows = $sth->fetchAll(PDO::FETCH_ASSOC);
		if (count($rows)==0)
			return null;
		return $rows;
	}

	/**
	 * fins a node in a cloud and returns UUID if found or FALSE otherwise.
	 * @param int $cloud_id
	 * @param long $node_ip
	 * @param int $node_port
	 */
	private function findNodeInCloud( $cloud_id, $node_ip, $node_port )
	{
		$sth = $this->db->prepare("SELECT * FROM ".TABLE_NODES." WHERE cloud_id=:cloudid AND ip=:node_ip AND port=:node_port");
		$ret = $sth->execute( array(':cloudid' => $cloud_id , ':node_ip'=>$node_ip, ':node_port'=>$node_port) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (gANFC)\n";
			return false;
		}
		$rows = $sth->fetchAll(PDO::FETCH_ASSOC);
		if (count($rows)==0)
			return false;
		return $rows[0]['uuid'];
	}

	/**
	 * join an existing cloud.
	 * returns the uuid for UPDATE and LEAVE command and the nodes in the cloud to send the announce message to.
	 * OK:546765486521
	 * node_ip=123.234.345.456&node_port=31415
	 */
	private function joinCloud( $cloud_id, $password, $node_ip, $node_port, $nickname, $returnNodeData=true, $getallnodes=false)
	{
		$host_reachable = ($this->sendPingToNode($node_ip, $node_port, 2)) ? 1 : 0;
		$nodes = $this->getAllNodesFromCloud($cloud_id);
		$uuid = $this->findNodeInCloud($cloud_id, $node_ip, $node_port);
		if (is_bool($uuid) && $uuid==false)
		{
			$uuid = time() + $node_ip + $node_port;
			$sth = $this->db->prepare("INSERT INTO ".TABLE_NODES." (cloud_id, nickname, ip, port, lastaction, uuid, reachable) VALUES (:cloud_id, :nickname, :node_ip, :node_port, NOW(), :uuid, :reachable)");
			$ret = $sth->execute( array(':cloud_id'=>$cloud_id, ':nickname'=>$nickname, ':node_ip'=>$node_ip, ':node_port'=>$node_port, ':uuid'=>$uuid, ':reachable'=>$host_reachable) );
		}
		else {
			$sth = $this->db->prepare("UPDATE ".TABLE_NODES." SET lastaction=NOW() WHERE uuid=:uuid AND cloud_id=:cloud_id");
			$ret = $sth->execute( array(':cloud_id'=>$cloud_id, ':uuid'=>$uuid) );
		}
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (jC1)\n";
			return $ret;
		}
		$sth = $this->db->prepare("UPDATE ".TABLE_CLOUDS." SET lastaction=NOW() WHERE id=:cloud_id");
		$ret = $sth->execute( array(':cloud_id'=>$cloud_id) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (jC2)\n";
			return null;
		}
		$ret_str = RETURN_CODE_OK.$uuid;
		if ($returnNodeData && $nodes!=null)
		{
			$nodes_to_send = $getallnodes ? count($nodes) : 1;
			for ($n=0; $n<$nodes_to_send; $n++)
			{
				$ip = $nodes[$n]["ip"];
				$port = $nodes[$n]["port"];
				if ($ip!=$node_ip || ( $ip!=$node_ip && $port!=$node_port) )
					$ret_str .= "\n" . PARAM_NODEIP ."=".urlencode(long2ip($ip))."&". PARAM_NODEPORT . "=".urlencode($port);
			}
		}
		return $ret_str;
	}

	// load cloud from DB and return assoc array or FALSE
	private function findCloud($cloudname)
	{
		$sth = $this->db->prepare("SELECT * FROM ".TABLE_CLOUDS." WHERE name = :cloudname LIMIT 1");
		$ret = $sth->execute( array(':cloudname' => $cloudname) );
		return $sth->fetch(PDO::FETCH_ASSOC);
	}

	// returns the number of nodes in the specified cloud
	private function getNodeCountInCloud($cloud_id)
	{
		$sth = $this->db->prepare("SELECT COUNT(*) as node_count FROM ".TABLE_NODES." WHERE cloud_id=:cloudid");
		$ret = $sth->execute( array(':cloudid' => $cloud_id) );
		$row = $sth->fetch(PDO::FETCH_ASSOC);
		return $row['node_count'];
	}

	// join an existing cloud or create a new one
	private function joinOrCreateCloud( $cloudname, $password, $maxnodes, $node_ip, $node_port, $nickname, $getallnodes)
	{
		$this->purgeList();
		$row = $this->findCloud( $cloudname);
		if ($row==false)
			return $this->createCloud( $cloudname, $password, $maxnodes, $node_ip, $node_port, $nickname);
		else
		{
			$cloud_id = $row['id'];
			$cloud_max_nodes = $row['max_nodes'];
			$node_count = $this->getNodeCountInCloud($cloud_id);
			if ($node_count>=$cloud_max_nodes)
				return RETURN_CODE_ERROR."to many nodes";
			if (strlen($row['password_hash'])==0 || $row['password_hash']==$password)
				return $this->joinCloud( $cloud_id, $password, $node_ip, $node_port, $nickname, true, $getallnodes);
			else
				return RETURN_CODE_ERROR."wrong password";
		}
	}

	// purge old nodes and clouds
	private function purgeList()
	{
		$sth = $this->db->prepare("DELETE FROM ".TABLE_NODES." WHERE DATE_ADD(`lastaction`, INTERVAL ".MAX_NO_ACTION_TIME." SECOND) < NOW()");
		$ret = $sth->execute( array() );
		$sth = $this->db->prepare("DELETE FROM ".TABLE_CLOUDS." WHERE DATE_ADD(`lastaction`, INTERVAL ".MAX_NO_ACTION_TIME." SECOND) < NOW()");
		$ret = $sth->execute( array() );
	}

	// delete client from node list for cloud
	private function leaveCloud($cloudname, $uuid)
	{
		$this->purgeList();
		$row = $this->findCloud($cloudname);
		if ($row==false)
			return RETURN_CODE_ERROR."cloud not found";
		$cloud_id = $row['id'];
		$sth = $this->db->prepare("DELETE FROM ".TABLE_NODES." WHERE uuid=:uuid AND cloud_id=:cloud_id");
		$ret = $sth->execute( array(':uuid'=>$uuid, ':cloud_id'=>$cloud_id) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (leC1)\n";
			return $ret;
		}

		$sth = $this->db->prepare("SELECT id FROM ".TABLE_NODES." WHERE cloud_id=:cloud_id LIMIT 1");
		$ret = $sth->execute( array(':cloud_id' => $cloud_id) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (leC2)\n";
			return $ret;
		}
		$rows = $sth->fetchAll(PDO::FETCH_ASSOC);
		if (count($rows)==0)
		{
			$sth = $this->db->prepare("DELETE FROM ".TABLE_CLOUDS." WHERE id=:cloud_id");
			$ret = $sth->execute( array(':cloud_id'=> $cloud_id) );
		}
		return RETURN_CODE_OK;
	}

	private function updateNode($cloudname, $uuid )
	{
		$row = $this->findCloud($cloudname);
		if ($row==false)
			return RETURN_CODE_ERROR."cloud not found";
		$cloud_id = $row['id'];
		$sth = $this->db->prepare("UPDATE ".TABLE_NODES." SET lastaction=NOW() WHERE uuid=:uuid AND cloud_id=:cloud_id");
		$ret = $sth->execute( array(':cloud_id'=>$cloud_id, ':uuid'=>$uuid) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (uN1)\n";
			return $ret;
		}
		$sth = $this->db->prepare("UPDATE ".TABLE_CLOUDS." SET lastaction=NOW() WHERE id=:cloud_id");
		$ret = $sth->execute( array(':cloud_id'=>$cloud_id) );
		if ($ret===false)
		{
			$ret = RETURN_CODE_ERROR."DB error (uN2)\n";
			return $ret;
		}
		$nodes = $this->getAllNodesFromCloud($cloud_id);
		$ret_str = RETURN_CODE_OK.$uuid;
		if ($nodes!=null)
		{
			$nodes_to_send = count($nodes);
			for ($n=0; $n<$nodes_to_send; $n++)
			{
				$ip = $nodes[$n]["ip"];
				$port = $nodes[$n]["port"];
				$node_uuid = $nodes[$n]['uuid'];
				if ( $uuid != $node_uuid )
					$ret_str .= "\n" . PARAM_NODEIP ."=".urlencode(long2ip($ip))."&". PARAM_NODEPORT . "=".urlencode($port);
			}
		}
		return $ret_str;
	}

	private function isPrivateSubnetIP( $ip_long )
	{
		$private_subnets = array();
		$private_subnets[] = array('start' => ip2long('192.168.0.0'), 'end' => ip2long('192.168.255.255'));
		$private_subnets[] = array('start' => ip2long('172.16.0.0'), 'end' => ip2long('172.31.255.255'));
		$private_subnets[] = array('start' => ip2long('10.0.0.0'), 'end' => ip2long('10.255.255.255'));
		foreach( $private_subnets as $ps)
			if ($ip_long>=$ps['start'] && $ip_long<=$ps['end'])
			return true;
		return false;
	}

	private function sendPingToNode( $ip_long, $port, $count, $delay_ms=100)
	{
		$ip = long2ip($ip_long);
		$socket = socket_create(AF_INET, SOCK_DGRAM, SOL_UDP);
		if (is_bool($socket) && $socket==false)
			return false;
		/* set socket receive timeout to 1 second */
		socket_set_option($socket, SOL_SOCKET, SO_RCVTIMEO, array("sec" => 1, "usec" => 0));
		$ping_payload = pack("L", time());
		$buffer_ping = pack("CS", UDP_MESSAGE_PING, strlen($ping_payload)) . $ping_payload;
		$bytes = -1;
		$udp_reply = "";
		$from_addr = "";
		$from_port = "";
		for ($i=0;($i<$count) && ($bytes<=0);$i++)
		{
			socket_sendto($socket, $buffer_ping, strlen($buffer_ping), 0, $ip, intval($port));
			$ep = error_reporting(0);
			$bytes = socket_recvfrom ($socket, &$udp_reply, 100, 0, &$from_addr, &$from_port);
			error_reporting($ep);
		}
		socket_close($socket);
		return ($bytes>0);
	}

	private function sendUDPhelloToHost( $ip, $port, $count=1, $delay_ms=100 )
	{
		$socket = socket_create(AF_INET, SOCK_DGRAM, SOL_UDP);
		if (is_bool($socket) && $socket==false)
			return RETURN_CODE_ERROR;
		/* set socket receive timeout to 1 second */
		$buffer_hello = pack("CCC",UDP_MESSAGE_HELLO, 0x00, 0x00);
		for ($i=0;$i<$count;$i++)
		{
			if ($i>0)
				usleep(10000*$delay_ms);
			socket_sendto($socket, $buffer_hello, strlen($buffer_hello), 0, $ip, intval($port));
		}
		socket_close($socket);
		return RETURN_CODE_OK;
	}

	private function list_stats()
	{
		$sth = $this->db->prepare("SELECT COUNT(*) as node_count FROM ".TABLE_NODES);
		$ret = $sth->execute( );
		$row = $sth->fetch(PDO::FETCH_ASSOC);
		$node_count =  $row['node_count'];
		$sth = $this->db->prepare("SELECT COUNT(*) as cloud_count FROM ".TABLE_CLOUDS);
		$ret = $sth->execute( );
		$row = $sth->fetch(PDO::FETCH_ASSOC);
		$cloud_count =  $row['cloud_count'];
		return "{$cloud_count},{$node_count}";
	}

	private function isMinimumClientversion($clientversion)
	{
		$minimum_version_num = str_replace('.', '', MIN_CLIENTVERSION);
		$actual_version_num = str_replace('.', '', $clientversion);
		return ($minimum_version_num <= $actual_version_num);
	}

	/*
	 * main program function
	* returns RETURN_CODE with message string, format "RETURN_CODE:message_string"
	*/
	public function run( $command, $cloudname, $password, $maxnodes, $node_ip, $node_port, $nickname, $uuid, $ip_from, $getallnodes, $clientversion)
	{
		if (strlen($command)<1)
			return RETURN_CODE_ERROR."no command specified";
		if ( $command!=CMD_GETLIST && $command!=CMD_STATS && $command!=CMD_SENDHELLO)
		{
			if (strlen($cloudname)<MIN_CLOUDNAME_LENGTH)
				return RETURN_CODE_ERROR."no valid cloud name specified";
			if ($command==CMD_LEAVE || $command==CMD_UPDATE)
			{
				if (strlen($uuid)<1 || !is_numeric($uuid))
					return RETURN_CODE_ERROR."no valid uuid";
			} else {
				if (strlen($node_ip)<7)
					return RETURN_CODE_ERROR."no valid ip specified";
				$node_ip_long = ip2long($node_ip);
				if ( !is_long($node_ip_long) )
					return RETURN_CODE_ERROR."no valid ip specified";
				if ( !is_numeric($node_port) || $node_port<=0)
					return RETURN_CODE_ERROR."no valid port specified";
			}
		}
		if ($this->openDB()==false)
			return RETURN_CODE_ERROR."could not open DB";
		$getallnodes = ($getallnodes==1);
		switch ($command){
			case CMD_GETLIST:
				return $this->loadCloudList();
				break;
			case CMD_JOIN:
				if ( strlen($nickname)<MIN_NICKNAME_LENGTH )
					return RETURN_CODE_ERROR."no nickname specified";
				if (!$this->isMinimumClientversion($clientversion))
					return RETURN_CODE_ERROR."minimum client version: ".MIN_CLIENTVERSION." , you have ".$clientversion;
				$ip_long = ($this->isPrivateSubnetIP($node_ip_long) || $node_ip_long==0) ? ip2long($ip_from) : $node_ip_long;
				return $this->joinOrCreateCloud( $cloudname, $password, $maxnodes, $ip_long, $node_port, $nickname, $getallnodes);
				break;
			case CMD_LEAVE:
				return $this->leaveCloud( $cloudname, $uuid);
				break;
			case CMD_UPDATE:
				return $this->updateNode($cloudname, $uuid );
			case CMD_STATS:
				return $this->list_stats();
				break;
			case CMD_SENDHELLO:
				return $this->sendUDPhelloToHost($node_ip, $node_port, 2, 10);
				break;
			default:
				return RETURN_CODE_ERROR."no valid command specified.";
		}
	}
}

function g($name)
{
	return (isset($_GET[$name]) ? $_GET[$name] : '');
}

$cls = new xbslink_cloudlist_server();
$ret = $cls->run( g(PARAM_CMD), g(PARAM_CLOUDNAME), g(PARAM_PASSWORD), g(PARAM_MAXNODES), g(PARAM_NODEIP), g(PARAM_NODEPORT), g(PARAM_NICKNAME), g(PARAM_UUID), $_SERVER['REMOTE_ADDR'], g(PARAM_GETALLNODES), g(PARAM_CLIENTVERSION));
echo $ret;
