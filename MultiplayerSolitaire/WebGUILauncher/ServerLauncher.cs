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
            string publicIp = Program.GetPublicIp().ToString();
            this.ServerAdress.Text = $"http://{publicIp}:{this.serverParameters.Port}/{this.serverParameters.EndPoint}";
        }

        private void LauncheGameButton_click(object sender, EventArgs e)
        {
            if (this.currentState != States.Configuration)
            {
                return;
            }

            Program.LaunchGame(this.serverParameters);
            this.currentState = States.Running;
            this.LaunchServerButton.Text = "Server Running";
            this.LaunchServerButton.Enabled = false;

            if (this.AutoLaunchBrowser.Checked)
            {
                Program.OpenBrowser(this.ServerAdress.Text);
            }
        }

        private void ServerLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.CloseGame();
        }

        private void Server_EveryClientDisconected()
        {
            this.Invoke(new Action(()=>this.Close()));
        }
    }
}
