using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsvToAvroApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void selectFiles_Click(object sender, EventArgs e)
        {
            //openFileDialog1.ShowDialog();

            DataTable dt = new DataTable();
            DataRow dr;
            if (openFileDialog1.ShowDialog() != DialogResult.None)
            {
                dt.Columns.Add("FileName");
                foreach (var fileName in openFileDialog1.FileNames)
                {
                    dr = dt.NewRow();
                    dr["FileName"] = fileName;
                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;
            }
        }
    }
}
