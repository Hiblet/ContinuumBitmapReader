namespace ContinuumBitmapReader
{
    partial class BitmapReaderUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelBitmapReader = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelFilenameFieldName = new System.Windows.Forms.Label();
            this.comboboxFilenameField = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelVersion);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 50);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboboxFilenameField);
            this.panel2.Controls.Add(this.labelFilenameFieldName);
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 50);
            this.panel2.TabIndex = 1;
            // 
            // labelBitmapReader
            // 
            this.labelBitmapReader.AutoSize = true;
            this.labelBitmapReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBitmapReader.Location = new System.Drawing.Point(3, 12);
            this.labelBitmapReader.Name = "labelBitmapReader";
            this.labelBitmapReader.Size = new System.Drawing.Size(158, 26);
            this.labelBitmapReader.TabIndex = 2;
            this.labelBitmapReader.Text = "Bitmap Reader";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(170, 22);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(46, 13);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "(v 1.0.0)";
            // 
            // labelFilenameFieldName
            // 
            this.labelFilenameFieldName.AutoSize = true;
            this.labelFilenameFieldName.Location = new System.Drawing.Point(8, 19);
            this.labelFilenameFieldName.Name = "labelFilenameFieldName";
            this.labelFilenameFieldName.Size = new System.Drawing.Size(77, 13);
            this.labelFilenameFieldName.TabIndex = 1;
            this.labelFilenameFieldName.Text = "Filename Field:";
            // 
            // comboboxFilenameField
            // 
            this.comboboxFilenameField.FormattingEnabled = true;
            this.comboboxFilenameField.Location = new System.Drawing.Point(90, 15);
            this.comboboxFilenameField.Name = "comboboxFilenameField";
            this.comboboxFilenameField.Size = new System.Drawing.Size(195, 21);
            this.comboboxFilenameField.TabIndex = 2;
            // 
            // BitmapReaderUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBitmapReader);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "BitmapReaderUserControl";
            this.Size = new System.Drawing.Size(300, 226);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelBitmapReader;
        private System.Windows.Forms.ComboBox comboboxFilenameField;
        private System.Windows.Forms.Label labelFilenameFieldName;
    }
}
