namespace GameDealer
{
    using System.Threading.Tasks;

    public class Dealer
    {
        private static Dealer instance;

        private Docker.DotNet.DockerClient dockerClient;

        private string gameCrateId = null;

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

        public void LaunchNewGame(out NewGameResult result)
        {
            result.Port = string.Empty;
            result.Success = false;
            if (!string.IsNullOrEmpty(this.gameCrateId))
            {
                System.Diagnostics.Debug.WriteLine("Trying to run multiples game instance at a time");
                result.Success = false;
                return;
            }

            string externalPort = "8081";

            var createGameTask = this.RunDockerGameInstance(externalPort);
            createGameTask.Wait();
            result.Success = true;
            result.Port = externalPort;
        }

        public string GetStatus()
        {
            if (string.IsNullOrEmpty(this.gameCrateId))
            {
                return "Idle";
            }

            System.Progress<Docker.DotNet.Models.ContainerStatsResponse> containerStats = new System.Progress<Docker.DotNet.Models.ContainerStatsResponse>();

            var containerInspectTask = this.dockerClient.Containers.InspectContainerAsync(this.gameCrateId);
            containerInspectTask.Wait(200);

            if(!containerInspectTask.IsCompleted)
            {
                return "Hanging";
            }

            switch (containerInspectTask.Result.State.Status)
            {
                case "running":
                    return "Running";
                case "exited":
                    return "Exited";
                default:
                    return "Unkown";
            }
        }

        // Based on https://www.danieldonbavand.com/dockerdotnet/
        private async Task<bool> RunDockerGameInstance(string externalPort)
        {
            if(!string.IsNullOrEmpty(this.gameCrateId))
            {
                return false;
            }

            // Ensuring the image is available.
            await this.dockerClient.Images.CreateImageAsync(new Docker.DotNet.Models.ImagesCreateParameters
            {
                FromImage = "game-instance",
            },
            new Docker.DotNet.Models.AuthConfig(),
            new System.Progress<Docker.DotNet.Models.JSONMessage>());

            const string internalPort = "80";

            var creationResponse = await this.dockerClient.Containers.CreateContainerAsync(new Docker.DotNet.Models.CreateContainerParameters
            {
                Image = "game-instance",
                ExposedPorts = new System.Collections.Generic.Dictionary<string, Docker.DotNet.Models.EmptyStruct>
                {
                    {
                        internalPort, default(Docker.DotNet.Models.EmptyStruct)
                    }
                },

                HostConfig = new Docker.DotNet.Models.HostConfig
                {
                    PortBindings = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IList<Docker.DotNet.Models.PortBinding>>
                    {
                        {
                            internalPort, new System.Collections.Generic.List<Docker.DotNet.Models.PortBinding> {new Docker.DotNet.Models.PortBinding {HostPort = externalPort}}
                        }
                    },

                    PublishAllPorts = true
                },

                Entrypoint = new System.Collections.Generic.List<string> { "dotnet", "DockerGameClient.dll", "-d", "*", "-p", internalPort },
            });

            string crateId = creationResponse.ID;
            System.Diagnostics.Debug.WriteLine($"Created crate {crateId}.");
            bool started = await this.dockerClient.Containers.StartContainerAsync(crateId, null);
            if(started)
            {
                this.gameCrateId = crateId;
            }
            
            return started;
        }

        public struct NewGameResult
        {
            public bool Success;
            public string Port;
        }
    }
}
