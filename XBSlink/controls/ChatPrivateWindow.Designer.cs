namespace XBSlink.controls
{
    partial class ChatPrivateWindow
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
            this.textBox_chatMessages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_chatMessages
            // 
            this.textBox_chatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_chatMessages.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox_chatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_chatMessages.Location = new System.Drawing.Point(3, 3);
            this.textBox_chatMessages.Multiline = true;
            this.textBox_chatMessages.Name = "textBox_chatMessages";
            this.textBox_chatMessages.ReadOnly = true;
            this.textBox_chatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_chatMessages.Size = new System.Drawing.Size(533, 445);
            this.textBox_chatMessages.TabIndex = 4;
            // 
            // ChatPrivateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_chatMessages);
            this.Name = "ChatPrivateWindow";
            this.Size = new System.Drawing.Size(539, 451);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_chatMessages;
    }
}
