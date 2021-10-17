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
        public ServerLauncher()
        {
            InitializeComponent();
        }

        private void ServerLauncher_Load(object sender, EventArgs e)
        {

        }

        private void LauncheGameButton_click(object sender, EventArgs e)
        {
            MSGWeb.MSGWeb.Parameters parameters = MSGWeb.MSGWeb.Parameters.Default();
            parameters.OnEveryClientDisconected = this.Server_EveryClientDisconected;
            Program.LaunchGame(parameters);
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
