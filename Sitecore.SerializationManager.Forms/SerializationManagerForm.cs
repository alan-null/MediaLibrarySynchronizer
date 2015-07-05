using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sitecore.SerializationManager.Models;

namespace Sitecore.SerializationManager.Forms
{
    public partial class SerializationManagerForm : Form
    {
        public SerializationManager Manager { get; set; }

        public SerializationManagerForm()
        {
            InitializeComponent();
            Manager = new SerializationManager();
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

        private void FillChildNodes(TreeNode node)
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
                    listView1.Items.Add(file.Name, file.Name, 0);
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

        private void attachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttachFileCommand();
        }

        private void detachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetachFileCommand();
        }


        private void downloadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DownloadFileCommand();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void attachToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AttachFileCommand();
        }

        private void detachToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DetachFileCommand();
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadFileCommand();
        }

        private void AttachFileCommand()
        {
            var itemName = listView1.SelectedItems;
            if (itemName.Count > 0)
            {
                var itemPath = String.Format("{0}\\{1}", treeView1.SelectedNode.FullPath, itemName[0].Name);
                if (IsValidFile(itemPath))
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog1.FileName;
                        Manager.AttachFileToSerializationItem(itemPath, filePath);
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Please select '.item' file first");
            }
        }

        private void DetachFileCommand()
        {
            var itemName = listView1.SelectedItems;
            if (itemName.Count > 0)
            {
                var itemPath = String.Format("{0}\\{1}", treeView1.SelectedNode.FullPath, itemName[0].Name);
                if (IsValidFile(itemPath))
                {
                    Manager.DetachFileFromSerializationItem(itemPath);
                }
            }
            else
            {
                MessageBox.Show(@"Please select '.item' file first");
            }
        }

        private void DownloadFileCommand()
        {
            var itemName = listView1.SelectedItems;
            if (itemName.Count > 0)
            {
                var itemPath = String.Format("{0}\\{1}", treeView1.SelectedNode.FullPath, itemName[0].Name);
                if (IsValidFile(itemPath))
                {

                    SerializationFile file = Manager.DownloadFileFromSerializationItem(itemPath);
                    saveFileDialog1.DefaultExt = file.Extensions;
                    saveFileDialog1.FileName = file.Name;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog1.FileName;
                        File.WriteAllBytes(filePath, file.Blob);
                    }

                }
            }
            else
            {
                MessageBox.Show(@"Please select '.item' file first");
            }
        }

        private static bool IsValidFile(string itemPath)
        {
            if (new FileInfo(itemPath).Extension != ".item")
            {
                MessageBox.Show(@"Cannot attach file to non '.item' file");
                return false;
            }
            return true;
        }
    }
}
