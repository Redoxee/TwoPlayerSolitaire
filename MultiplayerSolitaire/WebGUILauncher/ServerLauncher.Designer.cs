
namespace WebGUILauncher
{
    partial class ServerLauncher
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
            this.LaunchServerButton = new System.Windows.Forms.Button();
            this.ServerAdress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LaunchServerButton
            // 
            this.LaunchServerButton.Location = new System.Drawing.Point(12, 385);
            this.LaunchServerButton.Name = "LaunchServerButton";
            this.LaunchServerButton.Size = new System.Drawing.Size(460, 64);
            this.LaunchServerButton.TabIndex = 2;
            this.LaunchServerButton.Text = "Launch Game";
            this.LaunchServerButton.UseVisualStyleBackColor = true;
            this.LaunchServerButton.Click += new System.EventHandler(this.LauncheGameButton_click);
            // 
            // ServerAdress
            // 
            this.ServerAdress.Location = new System.Drawing.Point(124, 215);
            this.ServerAdress.Name = "ServerAdress";
            this.ServerAdress.ReadOnly = true;
            this.ServerAdress.Size = new System.Drawing.Size(230, 23);
            this.ServerAdress.TabIndex = 3;
            this.ServerAdress.Text = "127.0.0.1";
            // 
            // ServerLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.ServerAdress);
            this.Controls.Add(this.LaunchServerButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "ServerLauncher";
            this.Text = "Multiplayer Solitaire Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerLauncher_FormClosing);
            this.Load += new System.EventHandler(this.ServerLauncher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button LaunchServerButton;
        private System.Windows.Forms.TextBox ServerAdress;
    }
}

