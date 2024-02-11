namespace MainApplication
{
    partial class Form1
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
            this.btShowRtp = new System.Windows.Forms.Button();
            this.lbRtpList = new System.Windows.Forms.ListBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btShowRtp
            // 
            this.btShowRtp.Location = new System.Drawing.Point(329, 376);
            this.btShowRtp.Name = "btShowRtp";
            this.btShowRtp.Size = new System.Drawing.Size(146, 35);
            this.btShowRtp.TabIndex = 0;
            this.btShowRtp.Text = "Show Rtp";
            this.btShowRtp.UseMnemonic = false;
            this.btShowRtp.UseVisualStyleBackColor = true;
            this.btShowRtp.Click += new System.EventHandler(this.btShowRtp_Click);
            // 
            // lbRtpList
            // 
            this.lbRtpList.FormattingEnabled = true;
            this.lbRtpList.Location = new System.Drawing.Point(322, 108);
            this.lbRtpList.Name = "lbRtpList";
            this.lbRtpList.Size = new System.Drawing.Size(152, 56);
            this.lbRtpList.TabIndex = 1;
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(330, 249);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(127, 20);
            this.tbText.TabIndex = 2;
            this.tbText.Text = "tbText";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.lbRtpList);
            this.Controls.Add(this.btShowRtp);
            this.Name = "Form1";
            this.Text = "MainApplicationForm";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btShowRtp;
        private System.Windows.Forms.ListBox lbRtpList;
        private System.Windows.Forms.TextBox tbText;
    }
}

