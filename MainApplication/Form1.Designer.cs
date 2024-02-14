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
            this.btShowRtp1 = new System.Windows.Forms.Button();
            this.lbRtpList = new System.Windows.Forms.ListBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.btShowRtp2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btShowRtp1
            // 
            this.btShowRtp1.Location = new System.Drawing.Point(219, 318);
            this.btShowRtp1.Name = "btShowRtp1";
            this.btShowRtp1.Size = new System.Drawing.Size(146, 35);
            this.btShowRtp1.TabIndex = 0;
            this.btShowRtp1.Text = "Show Rtp 1";
            this.btShowRtp1.UseMnemonic = false;
            this.btShowRtp1.UseVisualStyleBackColor = true;
            this.btShowRtp1.Click += new System.EventHandler(this.btShowRtp1_Click);
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
            // btShowRtp2
            // 
            this.btShowRtp2.Location = new System.Drawing.Point(385, 318);
            this.btShowRtp2.Name = "btShowRtp2";
            this.btShowRtp2.Size = new System.Drawing.Size(146, 35);
            this.btShowRtp2.TabIndex = 3;
            this.btShowRtp2.Text = "Show Rtp 2";
            this.btShowRtp2.UseMnemonic = false;
            this.btShowRtp2.UseVisualStyleBackColor = true;
            this.btShowRtp2.Click += new System.EventHandler(this.btShowRtp2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btShowRtp2);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.lbRtpList);
            this.Controls.Add(this.btShowRtp1);
            this.Name = "Form1";
            this.Text = "MainApplicationForm";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btShowRtp1;
        private System.Windows.Forms.ListBox lbRtpList;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button btShowRtp2;
    }
}

