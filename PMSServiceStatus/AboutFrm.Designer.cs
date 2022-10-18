namespace PMSServiceStatus
{
    partial class AboutFrm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPMSServiceStatusVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblMachineCode = new System.Windows.Forms.Label();
            this.linkLabelURL = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.txtMachineCodeValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PMSServiceStatus.Properties.Resources._19_aboutServiceStatus;
            this.pictureBox1.Location = new System.Drawing.Point(10, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 106);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblPMSServiceStatusVersion
            // 
            this.lblPMSServiceStatusVersion.AutoSize = true;
            this.lblPMSServiceStatusVersion.Location = new System.Drawing.Point(132, 19);
            this.lblPMSServiceStatusVersion.Name = "lblPMSServiceStatusVersion";
            this.lblPMSServiceStatusVersion.Size = new System.Drawing.Size(177, 17);
            this.lblPMSServiceStatusVersion.TabIndex = 1;
            this.lblPMSServiceStatusVersion.Text = "PMS Service Status, Version";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(132, 58);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(322, 17);
            this.lblCopyright.TabIndex = 1;
            this.lblCopyright.Text = "Copyright(C) 2003 - 2022 by Baosight Corporation";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(132, 97);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(47, 17);
            this.lblURL.TabIndex = 1;
            this.lblURL.Text = "URL：";
            // 
            // lblMachineCode
            // 
            this.lblMachineCode.AutoSize = true;
            this.lblMachineCode.Location = new System.Drawing.Point(132, 136);
            this.lblMachineCode.Name = "lblMachineCode";
            this.lblMachineCode.Size = new System.Drawing.Size(102, 17);
            this.lblMachineCode.TabIndex = 1;
            this.lblMachineCode.Text = "Machine Code :";
            // 
            // linkLabelURL
            // 
            this.linkLabelURL.AutoSize = true;
            this.linkLabelURL.Location = new System.Drawing.Point(237, 97);
            this.linkLabelURL.Name = "linkLabelURL";
            this.linkLabelURL.Size = new System.Drawing.Size(125, 17);
            this.linkLabelURL.TabIndex = 2;
            this.linkLabelURL.TabStop = true;
            this.linkLabelURL.Text = "www.baosight.com";
            this.linkLabelURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelURL_LinkClicked);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(7, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(390, 1);
            this.label5.TabIndex = 1;
            // 
            // lblWarning
            // 
            this.lblWarning.Location = new System.Drawing.Point(12, 188);
            this.lblWarning.Margin = new System.Windows.Forms.Padding(3);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(379, 46);
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "Warning";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtMachineCodeValue
            // 
            this.txtMachineCodeValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMachineCodeValue.Location = new System.Drawing.Point(237, 136);
            this.txtMachineCodeValue.Name = "txtMachineCodeValue";
            this.txtMachineCodeValue.ReadOnly = true;
            this.txtMachineCodeValue.Size = new System.Drawing.Size(129, 17);
            this.txtMachineCodeValue.TabIndex = 3;
            // 
            // AboutFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 257);
            this.Controls.Add(this.txtMachineCodeValue);
            this.Controls.Add(this.linkLabelURL);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblMachineCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblPMSServiceStatusVersion);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About PMS Service Status";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblPMSServiceStatusVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblMachineCode;
        private System.Windows.Forms.LinkLabel linkLabelURL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.TextBox txtMachineCodeValue;
    }
}