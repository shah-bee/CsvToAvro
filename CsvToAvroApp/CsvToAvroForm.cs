using System;
using System.Collections.Generic;
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

            var csvFile = new ImportCsv();
            result = csvFile.ImportAllFiles(importLocation.Text, fileTypes.SelectedItem.ToString());
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
            var result1 = new ExportToAvro(exportLocation.Text, result, fileTypes.SelectedItem.ToString());

        }
    }
}
