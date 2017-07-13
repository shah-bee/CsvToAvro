using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CsvToAvro.Utility;
using CsvToAvroApp.Properties;

namespace CsvToAvroApp
{
    public partial class CsvToAvroForm : Form
    {
        private IEnumerable<object> result;
        public CsvToAvroForm()
        {
            InitializeComponent();
            fileTypes.SelectedIndex = 0;
            fileTypes.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            ImportlocationDialog.ShowDialog();
            importLocation.Text = ImportlocationDialog.SelectedPath;

            var csvFile = new ImportCsv(importLocation.Text, fileTypes.SelectedItem.ToString());
            result = csvFile.ImportAllFiles();
            if (result.Any())
            {
                MessageBox.Show("Data imported successfully!");
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            ExportlocationDialog.ShowDialog();
            exportLocation.Text = ExportlocationDialog.SelectedPath;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            CsvToAvroForm_FormClosing(sender, new FormClosingEventArgs(CloseReason.ApplicationExitCall, true));
        }

        private void CsvToAvroForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show(Resources.Confirm, Resources.Exit, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No;
        }

        private void ConvertCsvToAvro_Click(object sender, EventArgs e)
        {
            if (importLocation.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please select import location! ");
                return;
            }
            if (exportLocation.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please select export location! ");
                return;
            }
            var result1 = new ExportToAvro(exportLocation.Text, result, fileTypes.SelectedItem.ToString());
            if (ExportToAvro.IsImported)
            {
                MessageBox.Show("Successfully converted to avro file at" + exportLocation.Text);
                return;
            }

        }
    }
}
