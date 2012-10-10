using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.Android.Grid
{
   
        public class CloudsItem
        {

            public CloudsItem(xbs_cloud tmp_cloud)
                : this(tmp_cloud.name, tmp_cloud.node_count.ToString(), tmp_cloud.max_nodes.ToString(), tmp_cloud.isPrivate)
            {

            }

            public CloudsItem(string name, string node_count,string max_nodes, bool is_private)
            {
                SetData(name, node_count, max_nodes, is_private);
            }

            public void SetData(xbs_cloud cloud)
            {
                SetData(cloud.name, cloud.node_count.ToString(), cloud.max_nodes.ToString(), cloud.isPrivate);
            }

            public void SetData(CloudsItem cloud)
            {
                SetData(cloud.Name, cloud.NodeCount , cloud.MaxNodes, cloud.IsPrivate);
            }


            public void SetData(string name, string node_count,string max_nodes, bool is_private) {
                 Name = name; NodeCount = node_count; MaxNodes = max_nodes; IsPrivate = is_private;
                Image = (is_private) ? Resource.Drawable.icon_key :  Resource.Drawable.Icon;
                SetDescription();
            }

            public void SetDescription() {
                Description = String.Format("({0}/{1}) PASSWORD:{2}", NodeCount, MaxNodes, IsPrivate.ToString());
            }

              public int Image { get; set; }


            public string Name
            {
                get;
                set;
            }

            public string NodeCount
            {
                get;
                set;
            }

            public string MaxNodes
            {
                get;
                set;
            }

            public bool IsPrivate
            {
                get;
                set;
            }

            public string Description { get; set; }

        }
    }

