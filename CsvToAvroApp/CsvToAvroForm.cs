using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CsvToAvro.Utility;
using CsvToAvro.Utility.Models;
using CsvToAvroApp.Properties;

namespace CsvToAvroApp
{
    using CsvToAvro.Utility.Helper;
    using NLog;
    using NLog.Config;

    public partial class CsvToAvroForm : Form
    {
        private IEnumerable<object> result;
        private LogWrapper logger;
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


            switch (fileTypes.SelectedItem.ToString().ToLowerInvariant())
            {
                case "claim":
                    var claimImporter = new CsvToAvro.Utility.Claim.ClaimImporter(importLocation.Text, fileTypes.SelectedItem.ToString(),
                        "EDF " + fileTypes.SelectedItem.ToString() + "*.csv", new LogWrapper(exportLocation.Text));
                    claimImporter.Import();
                    result = claimImporter.Claims;
                    break;

            }

            if (result.Any())
            {
                var dialogResult = MessageBox.Show("Data imported successfully!", "Import result", MessageBoxButtons.OKCancel);
            }
            else
            {
                MessageBox.Show("Data not found to be imported!");
                return;
            }

            var result1 = new ExportToAvro(exportLocation.Text, result.Cast<Claim>(), fileTypes.SelectedItem.ToString());
            if (ExportToAvro.IsImported)
            {
                MessageBox.Show("Successfully converted to avro file at " + exportLocation.Text);
                return;
            }

        }

    }
}
