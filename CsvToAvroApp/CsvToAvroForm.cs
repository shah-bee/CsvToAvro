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
            var result = MessageBox.Show(Resources.Confirm, Resources.Exit, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result.Equals(DialogResult.Yes))
            {
                this.Close();
            }
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
            logger = new LogWrapper(exportLocation.Text);

            switch (fileTypes.SelectedItem.ToString().ToLowerInvariant())
            {
                case "claim":
                    var claimImporter = new CsvToAvro.Utility.Claim.ClaimImporter(importLocation.Text, fileTypes.SelectedItem.ToString(),
                        "EDF " + fileTypes.SelectedItem + "*.csv", logger);
                    claimImporter.Import();
                    result = claimImporter.Claims;
                    break;

            }

            if (result.Any())
            {
                var dialogResult = MessageBox.Show(LogWrapper.ErrorCount.Equals(0) ? "Data imported successfully!" : $"Date imported with {LogWrapper.ErrorCount} : Erros", "Import result", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.OK)
                {
                    var result1 = new ExportToAvro(exportLocation.Text, result.Cast<Claim>(), fileTypes.SelectedItem.ToString(), logger);
                    if (ExportToAvro.IsImported)
                    {
                        MessageBox.Show("Successfully converted to avro file at " + exportLocation.Text);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Data not found to be imported!");
                return;
            }
            
        }

    }
}
