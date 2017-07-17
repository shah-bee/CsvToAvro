using System.ComponentModel;
using System.Windows.Forms;

namespace CsvToAvroApp
{
    partial class CsvToAvroForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConvertCsvToAvro = new System.Windows.Forms.Button();
            this.ImportlocationDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.importBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.importLocation = new System.Windows.Forms.TextBox();
            this.exportLocation = new System.Windows.Forms.TextBox();
            this.exportBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.fileTypes = new System.Windows.Forms.ComboBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.ExportlocationDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // ConvertCsvToAvro
            // 
            this.ConvertCsvToAvro.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ConvertCsvToAvro.Location = new System.Drawing.Point(493, 154);
            this.ConvertCsvToAvro.Name = "ConvertCsvToAvro";
            this.ConvertCsvToAvro.Size = new System.Drawing.Size(100, 25);
            this.ConvertCsvToAvro.TabIndex = 0;
            this.ConvertCsvToAvro.Text = "Convert";
            this.ConvertCsvToAvro.UseVisualStyleBackColor = true;
            this.ConvertCsvToAvro.Click += new System.EventHandler(this.ConvertCsvToAvro_Click);
            // 
            // importBtn
            // 
            this.importBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.importBtn.Location = new System.Drawing.Point(493, 58);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(100, 25);
            this.importBtn.TabIndex = 1;
            this.importBtn.Text = "Browse...";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Import location :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Export location :";
            // 
            // importLocation
            // 
            this.importLocation.Location = new System.Drawing.Point(206, 62);
            this.importLocation.Name = "importLocation";
            this.importLocation.Size = new System.Drawing.Size(281, 20);
            this.importLocation.TabIndex = 5;
            // 
            // exportLocation
            // 
            this.exportLocation.Location = new System.Drawing.Point(206, 110);
            this.exportLocation.Name = "exportLocation";
            this.exportLocation.Size = new System.Drawing.Size(281, 20);
            this.exportLocation.TabIndex = 6;
            // 
            // exportBtn
            // 
            this.exportBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.exportBtn.Location = new System.Drawing.Point(493, 107);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(100, 25);
            this.exportBtn.TabIndex = 7;
            this.exportBtn.Text = "Browse...";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(127, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Type of Files :";
            // 
            // fileTypes
            // 
            this.fileTypes.FormattingEnabled = true;
            this.fileTypes.Items.AddRange(new object[] {
            "Claim",
            "Policy"});
            this.fileTypes.Location = new System.Drawing.Point(207, 26);
            this.fileTypes.Name = "fileTypes";
            this.fileTypes.Size = new System.Drawing.Size(279, 21);
            this.fileTypes.TabIndex = 9;
            // 
            // cancelBtn
            // 
            this.cancelBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtn.Location = new System.Drawing.Point(373, 156);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(100, 25);
            this.cancelBtn.TabIndex = 10;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // CsvToAvroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(694, 222);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.fileTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.exportLocation);
            this.Controls.Add(this.importLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.importBtn);
            this.Controls.Add(this.ConvertCsvToAvro);
            this.Name = "CsvToAvroForm";
            this.Text = "CSV To Avro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ConvertCsvToAvro;
        private FolderBrowserDialog ImportlocationDialog;
        private Button importBtn;
        private Label label2;
        private Label label3;
        private TextBox importLocation;
        private TextBox exportLocation;
        private Button exportBtn;
        private Label label4;
        private ComboBox fileTypes;
        private Button cancelBtn;
        private FolderBrowserDialog ExportlocationDialog;
    }
}

