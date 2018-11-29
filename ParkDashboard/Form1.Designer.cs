namespace ParkDashboard
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
            this.buttonParks = new System.Windows.Forms.Button();
            this.richTextBoxParks = new System.Windows.Forms.RichTextBox();
            this.buttonSpots = new System.Windows.Forms.Button();
            this.richTextBoxSpots = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonParks
            // 
            this.buttonParks.Location = new System.Drawing.Point(12, 40);
            this.buttonParks.Name = "buttonParks";
            this.buttonParks.Size = new System.Drawing.Size(135, 32);
            this.buttonParks.TabIndex = 0;
            this.buttonParks.Text = "Get Parks";
            this.buttonParks.UseVisualStyleBackColor = true;
            this.buttonParks.Click += new System.EventHandler(this.buttonParks_Click);
            // 
            // richTextBoxParks
            // 
            this.richTextBoxParks.Location = new System.Drawing.Point(12, 106);
            this.richTextBoxParks.Name = "richTextBoxParks";
            this.richTextBoxParks.Size = new System.Drawing.Size(375, 163);
            this.richTextBoxParks.TabIndex = 1;
            this.richTextBoxParks.Text = "";
            // 
            // buttonSpots
            // 
            this.buttonSpots.Location = new System.Drawing.Point(413, 40);
            this.buttonSpots.Name = "buttonSpots";
            this.buttonSpots.Size = new System.Drawing.Size(135, 32);
            this.buttonSpots.TabIndex = 2;
            this.buttonSpots.Text = "Get Spots";
            this.buttonSpots.UseVisualStyleBackColor = true;
            this.buttonSpots.Click += new System.EventHandler(this.buttonSpots_Click);
            // 
            // richTextBoxSpots
            // 
            this.richTextBoxSpots.Location = new System.Drawing.Point(413, 106);
            this.richTextBoxSpots.Name = "richTextBoxSpots";
            this.richTextBoxSpots.Size = new System.Drawing.Size(375, 163);
            this.richTextBoxSpots.TabIndex = 3;
            this.richTextBoxSpots.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxSpots);
            this.Controls.Add(this.buttonSpots);
            this.Controls.Add(this.richTextBoxParks);
            this.Controls.Add(this.buttonParks);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonParks;
        private System.Windows.Forms.RichTextBox richTextBoxParks;
        private System.Windows.Forms.Button buttonSpots;
        private System.Windows.Forms.RichTextBox richTextBoxSpots;
    }
}

