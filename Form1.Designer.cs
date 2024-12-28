namespace FollowIt
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            lblScore = new Label();
            lblhgScore = new Label();
            lblWelcome = new Label();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Arial", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScore.ForeColor = Color.Maroon;
            lblScore.Location = new Point(51, 78);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(84, 93);
            lblScore.TabIndex = 2;
            lblScore.Text = "0";
            // 
            // lblhgScore
            // 
            lblhgScore.AutoSize = true;
            lblhgScore.Font = new Font("Arial", 28.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblhgScore.ForeColor = Color.Maroon;
            lblhgScore.Location = new Point(51, 171);
            lblhgScore.Name = "lblhgScore";
            lblhgScore.Size = new Size(79, 55);
            lblhgScore.TabIndex = 3;
            lblhgScore.Text = "👑";
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Arial", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWelcome.ForeColor = Color.DarkRed;
            lblWelcome.Location = new Point(212, 30);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(505, 51);
            lblWelcome.TabIndex = 4;
            lblWelcome.Text = "Follow the green target";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Beige;
            ClientSize = new Size(950, 709);
            Controls.Add(lblWelcome);
            Controls.Add(lblhgScore);
            Controls.Add(lblScore);
            Name = "Form1";
            Text = "Follow It";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Label lblScore;
        private Label lblhgScore;
        private Label lblWelcome;
    }
}
