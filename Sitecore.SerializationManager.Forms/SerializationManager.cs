using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sitecore.SerializationManager.Forms
{
    public partial class SerializationManager : Form
    {
        public SerializationManager()
        {
            InitializeComponent();
        }

        private void SerializationManager_Load(object sender, EventArgs e)
        {
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo info in driveInfos.Where(info => info.DriveType == DriveType.Fixed))
            {
                TreeNode rootnode = new TreeNode(info.Name);
                treeView1.Nodes.Add(rootnode);
                FillChildNodes(rootnode);
            }
        }

        void FillChildNodes(TreeNode node)
        {
            try
            {
                DirectoryInfo dirs = new DirectoryInfo(node.FullPath);
                foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
                    TreeNode newnode = new TreeNode(dir.Name);
                    node.Nodes.Add(newnode);
                    newnode.Nodes.Add("*");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillListView(TreeNode node)
        {
            try
            {
                listView1.Items.Clear();
                DirectoryInfo dirs = new DirectoryInfo(node.FullPath);
                foreach (var file in dirs.GetFiles())
                {
                    listView1.Items.Add(file.Name, file.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == @"*")
            {
                e.Node.Nodes.Clear();
                FillChildNodes(e.Node);
                FillListView(e.Node);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            FillListView(e.Node);
        }
    }
}
