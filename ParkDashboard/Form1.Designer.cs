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
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelParkID = new System.Windows.Forms.Label();
            this.textBoxParkID = new System.Windows.Forms.TextBox();
            this.textBoxSpotID = new System.Windows.Forms.TextBox();
            this.labelSpotID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.Location = new System.Drawing.Point(17, 204);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(853, 252);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Options:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(768, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(104, 63);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(647, 28);
            this.comboBox1.TabIndex = 7;
            // 
            // labelParkID
            // 
            this.labelParkID.AutoSize = true;
            this.labelParkID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelParkID.Location = new System.Drawing.Point(13, 124);
            this.labelParkID.Name = "labelParkID";
            this.labelParkID.Size = new System.Drawing.Size(65, 20);
            this.labelParkID.TabIndex = 8;
            this.labelParkID.Text = "Park ID";
            // 
            // textBoxParkID
            // 
            this.textBoxParkID.Location = new System.Drawing.Point(17, 147);
            this.textBoxParkID.Name = "textBoxParkID";
            this.textBoxParkID.Size = new System.Drawing.Size(81, 22);
            this.textBoxParkID.TabIndex = 9;
            // 
            // textBoxSpotID
            // 
            this.textBoxSpotID.Location = new System.Drawing.Point(150, 147);
            this.textBoxSpotID.Name = "textBoxSpotID";
            this.textBoxSpotID.Size = new System.Drawing.Size(81, 22);
            this.textBoxSpotID.TabIndex = 10;
            // 
            // labelSpotID
            // 
            this.labelSpotID.AutoSize = true;
            this.labelSpotID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpotID.Location = new System.Drawing.Point(146, 124);
            this.labelSpotID.Name = "labelSpotID";
            this.labelSpotID.Size = new System.Drawing.Size(65, 20);
            this.labelSpotID.TabIndex = 11;
            this.labelSpotID.Text = "Spot ID";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 468);
            this.Controls.Add(this.labelSpotID);
            this.Controls.Add(this.textBoxSpotID);
            this.Controls.Add(this.textBoxParkID);
            this.Controls.Add(this.labelParkID);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox);
            this.Name = "Form1";
            this.Text = "ADMIN DASHBORD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelParkID;
        private System.Windows.Forms.TextBox textBoxParkID;
        private System.Windows.Forms.TextBox textBoxSpotID;
        private System.Windows.Forms.Label labelSpotID;
    }
}

