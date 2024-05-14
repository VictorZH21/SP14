using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace СП14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoaadDrives();
        }
        private void LoaadDrives()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        TreeNode treeNode = treeView1.Nodes.Add(drive.Name);
                        treeNode.Tag = drive.RootDirectory.FullName;
                        treeNode.ImageIndex = 0;
                        treeNode.Nodes.Add("");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке дисков произошла ошибка:" + ex.Message);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                TreeNode selectedNode = e.Node;
                selectedNode.Nodes.Clear();
                string path = (string)selectedNode.Tag;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (var dir in directoryInfo.GetDirectories())
                {
                    TreeNode node = selectedNode.Nodes.Add(dir.Name);
                    node.Tag = dir.FullName;
                    node.ImageIndex = 1;
                    node.Nodes.Add("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При развертывании узла произошла ошибка: " + ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                string path = (string)e.Node.Tag;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (var file in directoryInfo.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.Length.ToString());
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    item.ImageIndex = 2;
                    listView1.Items.Add(item);
                }
                DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(path));
                label1.Text = string.Format("Drive: {0}", path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При выборе узла произошла ошибка: " + ex.Message);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string path = Path.Combine(treeView1.SelectedNode.Tag.ToString(), listView1.SelectedItems[0].Text);
                    if (File.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                    }
                    else
                    {
                        MessageBox.Show("Выбранный элемент не является файлом.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При открытии файла произошла ошибка: " + ex.Message);
            }
        }
    }
}
