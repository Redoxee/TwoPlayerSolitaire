using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebGUILauncher
{
    public partial class ServerLauncher : Form
    {
        MSGWeb.MSGWeb.Parameters serverParameters = MSGWeb.MSGWeb.Parameters.Default();

        private States currentState;
        private string publicIp;
        private string localIp;

        public enum States
        {
            Configuration,
            Running,
        }

        public ServerLauncher()
        {
            InitializeComponent();
            this.currentState = States.Configuration;
            this.serverParameters.OnEveryClientDisconected = this.Server_EveryClientDisconected;
        }

        private void ServerLauncher_Load(object sender, EventArgs e)
        {
            this.publicIp = Program.GetPublicIp().ToString();
            this.localIp = Program.GetLocalIp();

            this.PortInput.Text = this.serverParameters.Port;

            this.refreshServerAdresses();
        }

        private void LauncheGameButton_click(object sender, EventArgs e)
        {
            if (this.currentState != States.Configuration)
            {
                return;
            }

            Program.StartServer(this.serverParameters);
            this.currentState = States.Running;
            this.LaunchServerButton.Text = "Server Running";
            this.LaunchServerButton.Enabled = false;
            this.PortInput.Enabled = false;

            if (this.AutoLaunchBrowser.Checked)
            {
                Program.OpenBrowser(this.LocalServerAdress.Text);
            }
        }

        private void ServerLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.StopServer();
        }

        private void Server_EveryClientDisconected()
        {
            if (this.CloseOnServerEnd.Checked)
            {
                this.Invoke(new Action(() => this.Close()));
                return;
            }

            this.Invoke(new Action(() => { 
                Program.StopServer();
                this.currentState = States.Configuration;
                this.LaunchServerButton.Text = "Launch Server";
                this.LaunchServerButton.Enabled = true;
                this.PortInput.Enabled = true;
            }));
        }

        private void PortInput_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(this.PortInput.Text, out int proposedPort))
            {
                this.PortInput.Text = this.serverParameters.Port;
                return;
            }

            if (proposedPort < 0 || proposedPort > 65536)
            {
                this.PortInput.Text = this.serverParameters.Port;
                return;
            }

            this.serverParameters.Port = this.PortInput.Text;
            this.refreshServerAdresses();
        }

        private void refreshServerAdresses()
        {
            this.PublicServerAdress.Text = $"http://{this.publicIp}:{this.serverParameters.Port}/{this.serverParameters.EndPoint}";
            this.LocalServerAdress.Text = $"http://{this.localIp}:{this.serverParameters.Port}/{this.serverParameters.EndPoint}";
        }
    }
}
