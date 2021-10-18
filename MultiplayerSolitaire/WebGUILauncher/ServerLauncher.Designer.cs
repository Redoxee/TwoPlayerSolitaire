
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.AutoLaunchBrowser = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LaunchServerButton
            // 
            this.LaunchServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LaunchServerButton.AutoSize = true;
            this.LaunchServerButton.Location = new System.Drawing.Point(12, 385);
            this.LaunchServerButton.Name = "LaunchServerButton";
            this.LaunchServerButton.Size = new System.Drawing.Size(460, 64);
            this.LaunchServerButton.TabIndex = 2;
            this.LaunchServerButton.Text = "Start game server";
            this.LaunchServerButton.UseVisualStyleBackColor = true;
            this.LaunchServerButton.Click += new System.EventHandler(this.LauncheGameButton_click);
            // 
            // ServerAdress
            // 
            this.ServerAdress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ServerAdress.Location = new System.Drawing.Point(114, 45);
            this.ServerAdress.Name = "ServerAdress";
            this.ServerAdress.ReadOnly = true;
            this.ServerAdress.Size = new System.Drawing.Size(343, 23);
            this.ServerAdress.TabIndex = 3;
            this.ServerAdress.Text = "127.0.0.1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.23529F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.76471F));
            this.tableLayoutPanel1.Controls.Add(this.ServerAdress, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AutoLaunchBrowser, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 294);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 85);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 33);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server adress";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AutoLaunchBrowser
            // 
            this.AutoLaunchBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoLaunchBrowser.AutoSize = true;
            this.AutoLaunchBrowser.Checked = true;
            this.AutoLaunchBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoLaunchBrowser.Location = new System.Drawing.Point(114, 3);
            this.AutoLaunchBrowser.Name = "AutoLaunchBrowser";
            this.AutoLaunchBrowser.Size = new System.Drawing.Size(187, 36);
            this.AutoLaunchBrowser.TabIndex = 5;
            this.AutoLaunchBrowser.Text = "Launch browser on server start";
            this.AutoLaunchBrowser.UseVisualStyleBackColor = true;
            // 
            // ServerLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.LaunchServerButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "ServerLauncher";
            this.Text = "Multiplayer Solitaire Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerLauncher_FormClosing);
            this.Load += new System.EventHandler(this.ServerLauncher_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button LaunchServerButton;
        private System.Windows.Forms.TextBox ServerAdress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox AutoLaunchBrowser;
    }
}

