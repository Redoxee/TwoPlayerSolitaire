namespace GameDealer
{
    using System.Threading.Tasks;
    using MSGWeb;

    public class Dealer
    {
        private static Dealer instance;

        private Docker.DotNet.DockerClient dockerClient;

        public static Dealer Instance
        {
            get
            {
                if (Dealer.instance == null)
                {
                    Dealer.instance = new Dealer();
                }

                return Dealer.instance;
            }
        }

        private Dealer()
        {
            this.dockerClient = new Docker.DotNet.DockerClientConfiguration(new System.Uri(Dealer.DockerApiUri())).CreateClient();
        }

        internal static string DockerApiUri()
        {
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

            if (isWindows)
            {
                return "npipe://./pipe/docker_engine";
            }

            var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

            if (isLinux)
            {
                return "unix:/var/run/docker.sock";
            }

            throw new System.Exception("Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }

        public void LaunchNewGame()
        {
            System.Threading.Tasks.Task.Run(() => { this.RunDockerGameInstance(); });
        }

        // Based on https://www.danieldonbavand.com/dockerdotnet/
        private async void RunDockerGameInstance()
        {
            await this.dockerClient.Images.CreateImageAsync(new Docker.DotNet.Models.ImagesCreateParameters
            {
                FromImage = "game-instance",
            },
            new Docker.DotNet.Models.AuthConfig(),
            new System.Progress<Docker.DotNet.Models.JSONMessage>());
            var creationResponse = await this.dockerClient.Containers.CreateContainerAsync(new Docker.DotNet.Models.CreateContainerParameters
            {
                Image = "game-instance",
                ExposedPorts = new System.Collections.Generic.Dictionary<string, Docker.DotNet.Models.EmptyStruct>
                {
                    {
                        "8080", default(Docker.DotNet.Models.EmptyStruct)
                    }
                },

                HostConfig = new Docker.DotNet.Models.HostConfig
                {
                    PortBindings = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IList<Docker.DotNet.Models.PortBinding>>
                    {
                        {
                            "8080", new System.Collections.Generic.List<Docker.DotNet.Models.PortBinding> {new Docker.DotNet.Models.PortBinding {HostPort = "8081"}}
                        }
                    },

                    PublishAllPorts = true
                }
            });
            string crateId = creationResponse.ID;
            System.Diagnostics.Trace.WriteLine($"Created crate {crateId}.");
            await this.dockerClient.Containers.StartContainerAsync(crateId, null);
        }
    }
}
