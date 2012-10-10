using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBSlink
{
    public class xbs_cloud
    {
        public String name;
        public int node_count;
        public int max_nodes;
        public bool isPrivate = false;

        public xbs_cloud(String name, int node_count, int max_nodes, bool isPrivate)
        {
            this.name = name;
            this.node_count = node_count;
            this.max_nodes = max_nodes;
            this.isPrivate = isPrivate;
        }
    }
}
