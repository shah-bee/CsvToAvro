using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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
            if (MessageBox.Show(Resources.Confirm, Resources.Exit, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                Close();
            }
        }

        private void DisableFormControls()
        {
            this.Controls.Cast<Control>().ToList().ForEach(c => c.Enabled = false);
        }
        private void EnableFormControls()
        {
            this.Controls.Cast<Control>().ToList().ForEach(c => c.Enabled = true);
        }

        private async void ConvertCsvToAvro_Click(object sender, EventArgs e)
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

            DisableFormControls();

            // start the waiting animation
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Marquee;

            logger = new LogWrapper(exportLocation.Text);

            switch (fileTypes.SelectedItem.ToString().ToLowerInvariant())
            {
                case "claim":
                    var claimImporter = new CsvToAvro.Utility.Claim.ClaimImporter(importLocation.Text, fileTypes.SelectedItem.ToString(),
                        "EDF " + fileTypes.SelectedItem + "*.csv", logger);
                    await Task.Run(() => claimImporter.Import());
                    result = claimImporter.Claims;
                    break;
                default:
                    MessageBox.Show("Currently supporting for claims only!");
                    EnableFormControls();
                    break;
            }

            progressBar.Visible = false;
            if (result != null)
            {
                if (result.ToList().Any())
                {
                    var dialogResult =
                        MessageBox.Show(
                            LogWrapper.ErrorCount.Equals(0)
                                ? "Data imported successfully!"
                                : $"Data imported with {LogWrapper.ErrorCount} : Erros", "Import result",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.OK)
                    {
                        var export = new ExportToAvro(exportLocation.Text, result.Cast<Claim>(),
                            fileTypes.SelectedItem.ToString(), logger);
                        progressBar.Visible = true;
                        await Task.Run(() => export.Export());
                        progressBar.Visible = false;
                        if (export.IsImported)
                        {
                            MessageBox.Show(Resources.Success + exportLocation.Text, Resources.exported,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"{LogWrapper.ErrorCount} : Error(s) while importing the data!", Resources.error,
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            EnableFormControls();
        }

        private void fileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            result = null;
            importLocation.Text = string.Empty;
            exportLocation.Text = string.Empty;
        }
    }
}
