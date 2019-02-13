namespace Minesweeper
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
            this.rowLabel = new System.Windows.Forms.Label();
            this.columnLabel = new System.Windows.Forms.Label();
            this.mineLabel = new System.Windows.Forms.Label();
            this.rowTextBox = new System.Windows.Forms.TextBox();
            this.columnTextBox = new System.Windows.Forms.TextBox();
            this.mineTextBox = new System.Windows.Forms.TextBox();
            this.continueButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rowLabel
            // 
            this.rowLabel.AutoSize = true;
            this.rowLabel.Location = new System.Drawing.Point(69, 28);
            this.rowLabel.Name = "rowLabel";
            this.rowLabel.Size = new System.Drawing.Size(92, 13);
            this.rowLabel.TabIndex = 0;
            this.rowLabel.Text = "Number of Rows: ";
            // 
            // columnLabel
            // 
            this.columnLabel.AutoSize = true;
            this.columnLabel.Location = new System.Drawing.Point(69, 74);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(105, 13);
            this.columnLabel.TabIndex = 1;
            this.columnLabel.Text = "Number of Columns: ";
            // 
            // mineLabel
            // 
            this.mineLabel.AutoSize = true;
            this.mineLabel.Location = new System.Drawing.Point(72, 116);
            this.mineLabel.Name = "mineLabel";
            this.mineLabel.Size = new System.Drawing.Size(93, 13);
            this.mineLabel.TabIndex = 2;
            this.mineLabel.Text = "Number of Mines: ";
            // 
            // rowTextBox
            // 
            this.rowTextBox.Location = new System.Drawing.Point(230, 28);
            this.rowTextBox.Name = "rowTextBox";
            this.rowTextBox.Size = new System.Drawing.Size(100, 20);
            this.rowTextBox.TabIndex = 3;
            // 
            // columnTextBox
            // 
            this.columnTextBox.Location = new System.Drawing.Point(230, 66);
            this.columnTextBox.Name = "columnTextBox";
            this.columnTextBox.Size = new System.Drawing.Size(100, 20);
            this.columnTextBox.TabIndex = 4;
            // 
            // mineTextBox
            // 
            this.mineTextBox.Location = new System.Drawing.Point(230, 116);
            this.mineTextBox.Name = "mineTextBox";
            this.mineTextBox.Size = new System.Drawing.Size(100, 20);
            this.mineTextBox.TabIndex = 5;
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(230, 173);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(100, 23);
            this.continueButton.TabIndex = 6;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.mineTextBox);
            this.Controls.Add(this.columnTextBox);
            this.Controls.Add(this.rowTextBox);
            this.Controls.Add(this.mineLabel);
            this.Controls.Add(this.columnLabel);
            this.Controls.Add(this.rowLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Load += new System.EventHandler(this.Form1_Load);

        }

        #endregion

        private System.Windows.Forms.Label rowLabel;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.Label mineLabel;
        private System.Windows.Forms.TextBox rowTextBox;
        private System.Windows.Forms.TextBox columnTextBox;
        private System.Windows.Forms.TextBox mineTextBox;
        private System.Windows.Forms.Button continueButton;
    }
}

