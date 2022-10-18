namespace PMSServiceStatus.Forms
{
    partial class CloseFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloseFrm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.btnMinimumBox = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PMSServiceStatus.Properties.Resources.box_important_64px;
            this.pictureBox1.Location = new System.Drawing.Point(12, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 57);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblPrompt.Location = new System.Drawing.Point(68, 28);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(129, 21);
            this.lblPrompt.TabIndex = 1;
            this.lblPrompt.Text = "确认要退出吗？";
            // 
            // btnMinimumBox
            // 
            this.btnMinimumBox.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMinimumBox.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnMinimumBox.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimumBox.Image")));
            this.btnMinimumBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMinimumBox.Location = new System.Drawing.Point(14, 75);
            this.btnMinimumBox.Name = "btnMinimumBox";
            this.btnMinimumBox.Size = new System.Drawing.Size(266, 35);
            this.btnMinimumBox.TabIndex = 2;
            this.btnMinimumBox.Text = "最小化到托盘";
            this.btnMinimumBox.UseVisualStyleBackColor = true;
            this.btnMinimumBox.Click += new System.EventHandler(this.btnMinimumBox_Click);
            // 
            // btnExit
            // 
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(14, 115);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(266, 35);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "直接退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // CloseFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 168);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnMinimumBox);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CloseFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提示";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button btnMinimumBox;
        private System.Windows.Forms.Button btnExit;
    }
}