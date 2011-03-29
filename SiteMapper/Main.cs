using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace SiteMapper
{
    public partial class Main : Form
    {
        Form about = null;
        String[] includeFiles;
        String[] excludeFolders;
        String[] excludeFiles;

        public Main()
        {
            InitializeComponent();
        }

        private void SelectInputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbSelectedPath.Text = folderBrowserDialog1.SelectedPath;

            }
        }


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            
            if (tbSelectedPath.Text.Length > 0)
            {
                includeFiles = new String[lbIncludeFiles.Items.Count];
                excludeFolders = new String[lbExcludeFolders.Items.Count];
                excludeFiles = new String[lbExcludeFiles.Items.Count];

                lbIncludeFiles.Items.CopyTo(includeFiles, 0);
                lbExcludeFolders.Items.CopyTo(excludeFolders, 0);
                lbExcludeFiles.Items.CopyTo(excludeFiles, 0);

                DirectoryParser dp = new DirectoryParser();
                XDocument doc = dp.getXmlSiteMap(tbSelectedPath.Text,includeFiles,excludeFolders,excludeFiles);
                DataSet dsDoc = new DataSet();
                XmlReader xr = doc.CreateReader();
                dsDoc.ReadXml(xr);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dsDoc;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.DataMember = "siteMapNode";


                tbSummary.Text += doc.Declaration + "\r\n";
                tbSummary.Text += doc.ToString();
                doc.Save(tbOutputDir.Text + "\\Web.sitemap");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbOutputDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void helpContextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutSiteMapperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (about == null || about.IsDisposed)
            {
                about = new AboutSiteMapper();
                about.Visible = true;
            }
            else
            {
                about.Focus();
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  


    }
}
