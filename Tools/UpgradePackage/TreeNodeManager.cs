using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpgradePackage
{
    public class iTreeNodeManager
    {
        static List<string> tplDic;
        public static iTreeNode getTreefromPath(String path, List<string> _tplDic)
        {
            tplDic = _tplDic;
            iTreeNode itn = createNode(path, NodeType.DIR);
            itn.Tag = path;
            itn.ChildNodesAddChildNodes();
            return itn;
        }


        public static ArrayList getChildNodes(iTreeNode itn)
        {
            ArrayList nodes = new ArrayList();
            String path = itn.path;
            DirectoryInfo dirinfo = new DirectoryInfo(path);
            DirectoryInfo[] child_dirinfos = dirinfo.GetDirectories();
            foreach (DirectoryInfo di in child_dirinfos)
            {
                String p = di.FullName;
                iTreeNode n = createNode(p, NodeType.DIR);
                n.SelectedImageIndex = 0;
                n.ImageIndex = 0;
                n.Checked = tplDic.Contains(p) ? true : false;
                n.Tag = p;
                nodes.Add(n);
            }
            FileInfo[] child_fileinfos = dirinfo.GetFiles();
            foreach (FileInfo fi in child_fileinfos)
            {
                String p = fi.FullName;
                iTreeNode n = createNode(p, NodeType.FILE);
                n.SelectedImageIndex = 1;
                n.ImageIndex = 1;
                n.Checked = tplDic.Contains(p) ? true : false;
                n.Tag = p;
                nodes.Add(n);
            }
            return nodes;

        }
        static iTreeNode createNode(String path, NodeType ntype)
        {
            iTreeNode itn = new iTreeNode();
            itn.ntype = ntype;
            itn.path = path;
            itn.Text = getShortName(path);
            return itn;
        }
        static String getShortName(String path)
        {
            String[] tmp = path.Split('\\');
            int l = tmp.Length;
            return tmp[l - 1];

        }

    }

    public class iTreeNode : TreeNode
    {
        public NodeType ntype;
        public String path;
        public void AddNodes(ArrayList nodes)
        {
            foreach (iTreeNode n in nodes)
            {
                this.Nodes.Add(n);

            }

        }

        public void AddChildNodes()
        {
            if (this.ntype == NodeType.DIR)
            {
                ArrayList childnodes = iTreeNodeManager.getChildNodes(this);
                this.AddNodes(childnodes);
            }
        }
        public void ChildNodesAddChildNodes()
        {
            this.AddChildNodes();
            foreach (iTreeNode itn in this.Nodes)
            {
                itn.ChildNodesAddChildNodes();
            }
        }


    }



   public enum NodeType { DIR, FILE }
}
