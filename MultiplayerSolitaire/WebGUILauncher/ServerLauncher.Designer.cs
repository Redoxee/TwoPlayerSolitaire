
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortInput = new System.Windows.Forms.TextBox();
            this.PublicAdressLabel = new System.Windows.Forms.Label();
            this.PublicServerAdress = new System.Windows.Forms.TextBox();
            this.LocalAdressLabel = new System.Windows.Forms.Label();
            this.LocalServerAdress = new System.Windows.Forms.TextBox();
            this.AutoLaunchBrowser = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CloseOnServerEnd = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LaunchServerButton
            // 
            this.LaunchServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LaunchServerButton.AutoSize = true;
            this.LaunchServerButton.Location = new System.Drawing.Point(12, 185);
            this.LaunchServerButton.Name = "LaunchServerButton";
            this.LaunchServerButton.Size = new System.Drawing.Size(460, 64);
            this.LaunchServerButton.TabIndex = 2;
            this.LaunchServerButton.Text = "Start game server";
            this.LaunchServerButton.UseVisualStyleBackColor = true;
            this.LaunchServerButton.Click += new System.EventHandler(this.LauncheGameButton_click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PortLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PortInput, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PublicAdressLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PublicServerAdress, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LocalAdressLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LocalServerAdress, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.AutoLaunchBrowser, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.CloseOnServerEnd, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 169);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // PortLabel
            // 
            this.PortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(142, 40);
            this.PortLabel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(29, 19);
            this.PortLabel.TabIndex = 6;
            this.PortLabel.Text = "Port";
            this.PortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PortInput
            // 
            this.PortInput.Location = new System.Drawing.Point(177, 36);
            this.PortInput.Name = "PortInput";
            this.PortInput.Size = new System.Drawing.Size(72, 23);
            this.PortInput.TabIndex = 7;
            this.PortInput.TextChanged += new System.EventHandler(this.PortInput_TextChanged);
            // 
            // PublicAdressLabel
            // 
            this.PublicAdressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PublicAdressLabel.AutoSize = true;
            this.PublicAdressLabel.Location = new System.Drawing.Point(93, 73);
            this.PublicAdressLabel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.PublicAdressLabel.Name = "PublicAdressLabel";
            this.PublicAdressLabel.Size = new System.Drawing.Size(78, 23);
            this.PublicAdressLabel.TabIndex = 4;
            this.PublicAdressLabel.Text = "Public Adress";
            this.PublicAdressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PublicServerAdress
            // 
            this.PublicServerAdress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PublicServerAdress.Location = new System.Drawing.Point(177, 69);
            this.PublicServerAdress.Name = "PublicServerAdress";
            this.PublicServerAdress.ReadOnly = true;
            this.PublicServerAdress.Size = new System.Drawing.Size(280, 23);
            this.PublicServerAdress.TabIndex = 3;
            this.PublicServerAdress.Text = "127.0.0.1";
            // 
            // LocalAdressLabel
            // 
            this.LocalAdressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalAdressLabel.AutoSize = true;
            this.LocalAdressLabel.Location = new System.Drawing.Point(98, 106);
            this.LocalAdressLabel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.LocalAdressLabel.Name = "LocalAdressLabel";
            this.LocalAdressLabel.Size = new System.Drawing.Size(73, 19);
            this.LocalAdressLabel.TabIndex = 8;
            this.LocalAdressLabel.Text = "Local Adress";
            this.LocalAdressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LocalServerAdress
            // 
            this.LocalServerAdress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalServerAdress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LocalServerAdress.Location = new System.Drawing.Point(177, 102);
            this.LocalServerAdress.Name = "LocalServerAdress";
            this.LocalServerAdress.ReadOnly = true;
            this.LocalServerAdress.Size = new System.Drawing.Size(280, 23);
            this.LocalServerAdress.TabIndex = 9;
            this.LocalServerAdress.Text = "127.0.0.1";
            // 
            // AutoLaunchBrowser
            // 
            this.AutoLaunchBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoLaunchBrowser.AutoSize = true;
            this.AutoLaunchBrowser.Checked = true;
            this.AutoLaunchBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoLaunchBrowser.Location = new System.Drawing.Point(177, 3);
            this.AutoLaunchBrowser.Name = "AutoLaunchBrowser";
            this.AutoLaunchBrowser.Size = new System.Drawing.Size(15, 27);
            this.AutoLaunchBrowser.TabIndex = 5;
            this.AutoLaunchBrowser.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "Launch browser on server start";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 139);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Close on server end";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CloseOnServerEnd
            // 
            this.CloseOnServerEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CloseOnServerEnd.AutoSize = true;
            this.CloseOnServerEnd.Checked = true;
            this.CloseOnServerEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CloseOnServerEnd.Location = new System.Drawing.Point(177, 135);
            this.CloseOnServerEnd.Name = "CloseOnServerEnd";
            this.CloseOnServerEnd.Size = new System.Drawing.Size(15, 31);
            this.CloseOnServerEnd.TabIndex = 12;
            this.CloseOnServerEnd.UseVisualStyleBackColor = true;
            // 
            // ServerLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.LaunchServerButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox AutoLaunchBrowser;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortInput;
        private System.Windows.Forms.Label PublicAdressLabel;
        private System.Windows.Forms.TextBox PublicServerAdress;
        private System.Windows.Forms.Label LocalAdressLabel;
        private System.Windows.Forms.TextBox LocalServerAdress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CloseOnServerEnd;
    }
}

