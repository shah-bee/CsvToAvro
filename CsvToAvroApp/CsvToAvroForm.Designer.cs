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
            this.label1 = new System.Windows.Forms.Label();
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
            this.ConvertCsvToAvro.Location = new System.Drawing.Point(493, 240);
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
            this.importBtn.Location = new System.Drawing.Point(493, 144);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(100, 25);
            this.importBtn.TabIndex = 1;
            this.importBtn.Text = "Browse...";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(261, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 34);
            this.label1.TabIndex = 2;
            this.label1.Text = "CSV To Avro";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Import location :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Export location :";
            // 
            // importLocation
            // 
            this.importLocation.Location = new System.Drawing.Point(206, 148);
            this.importLocation.Name = "importLocation";
            this.importLocation.Size = new System.Drawing.Size(281, 20);
            this.importLocation.TabIndex = 5;
            // 
            // exportLocation
            // 
            this.exportLocation.Location = new System.Drawing.Point(206, 196);
            this.exportLocation.Name = "exportLocation";
            this.exportLocation.Size = new System.Drawing.Size(281, 20);
            this.exportLocation.TabIndex = 6;
            // 
            // exportBtn
            // 
            this.exportBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.exportBtn.Location = new System.Drawing.Point(493, 193);
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
            this.label4.Location = new System.Drawing.Point(127, 112);
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
            this.fileTypes.Location = new System.Drawing.Point(207, 112);
            this.fileTypes.Name = "fileTypes";
            this.fileTypes.Size = new System.Drawing.Size(280, 21);
            this.fileTypes.TabIndex = 9;
            // 
            // cancelBtn
            // 
            this.cancelBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtn.Location = new System.Drawing.Point(373, 242);
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
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(674, 307);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.fileTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.exportLocation);
            this.Controls.Add(this.importLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.importBtn);
            this.Controls.Add(this.ConvertCsvToAvro);
            this.Name = "CsvToAvroForm";
            this.Text = "CSV To Avro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CsvToAvroForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ConvertCsvToAvro;
        private FolderBrowserDialog ImportlocationDialog;
        private Button importBtn;
        private Label label1;
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

