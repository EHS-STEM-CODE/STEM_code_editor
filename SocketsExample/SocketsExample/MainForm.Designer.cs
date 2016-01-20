namespace SocketsExample
{
    partial class MainForm
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
            this.radClient = new System.Windows.Forms.RadioButton();
            this.radServer = new System.Windows.Forms.RadioButton();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textOutgoing = new System.Windows.Forms.TextBox();
            this.textIncoming = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radClient
            // 
            this.radClient.AutoSize = true;
            this.radClient.Checked = true;
            this.radClient.Location = new System.Drawing.Point(12, 356);
            this.radClient.Name = "radClient";
            this.radClient.Size = new System.Drawing.Size(51, 17);
            this.radClient.TabIndex = 0;
            this.radClient.TabStop = true;
            this.radClient.Text = "Client";
            this.radClient.UseVisualStyleBackColor = true;
            // 
            // radServer
            // 
            this.radServer.AutoSize = true;
            this.radServer.Location = new System.Drawing.Point(78, 356);
            this.radServer.Name = "radServer";
            this.radServer.Size = new System.Drawing.Size(56, 17);
            this.radServer.TabIndex = 1;
            this.radServer.Text = "Server";
            this.radServer.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(140, 353);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textOutgoing
            // 
            this.textOutgoing.Location = new System.Drawing.Point(12, 12);
            this.textOutgoing.Multiline = true;
            this.textOutgoing.Name = "textOutgoing";
            this.textOutgoing.Size = new System.Drawing.Size(239, 318);
            this.textOutgoing.TabIndex = 3;
            this.textOutgoing.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textOutgoing_KeyPress);
            // 
            // textIncoming
            // 
            this.textIncoming.Location = new System.Drawing.Point(257, 12);
            this.textIncoming.Multiline = true;
            this.textIncoming.Name = "textIncoming";
            this.textIncoming.Size = new System.Drawing.Size(243, 318);
            this.textIncoming.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(326, 358);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(230, 353);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 6;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 385);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.textIncoming);
            this.Controls.Add(this.textOutgoing);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.radServer);
            this.Controls.Add(this.radClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Sockets Example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radClient;
        private System.Windows.Forms.RadioButton radServer;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textOutgoing;
        private System.Windows.Forms.TextBox textIncoming;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button buttonStop;
    }
}

