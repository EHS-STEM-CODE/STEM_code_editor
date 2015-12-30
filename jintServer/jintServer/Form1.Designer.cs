namespace jintServer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.codeBox = new System.Windows.Forms.RichTextBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.ListenButton = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.codeBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.StopButton);
            this.splitContainer1.Panel2.Controls.Add(this.ListenButton);
            this.splitContainer1.Panel2.Controls.Add(this.outputBox);
            this.splitContainer1.Size = new System.Drawing.Size(874, 680);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 0;
            // 
            // codeBox
            // 
            this.codeBox.Location = new System.Drawing.Point(0, 0);
            this.codeBox.Name = "codeBox";
            this.codeBox.ReadOnly = true;
            this.codeBox.Size = new System.Drawing.Size(661, 330);
            this.codeBox.TabIndex = 0;
            this.codeBox.Text = "";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(437, 271);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(437, 71);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop Listening";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ListenButton
            // 
            this.ListenButton.Location = new System.Drawing.Point(0, 271);
            this.ListenButton.Name = "ListenButton";
            this.ListenButton.Size = new System.Drawing.Size(437, 71);
            this.ListenButton.TabIndex = 1;
            this.ListenButton.Text = "Listen";
            this.ListenButton.UseVisualStyleBackColor = true;
            this.ListenButton.Click += new System.EventHandler(this.ListenButton_Click);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(0, 0);
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(874, 269);
            this.outputBox.TabIndex = 0;
            this.outputBox.Text = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(654, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 330);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 680);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox codeBox;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.Button ListenButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button button1;
    }
}

