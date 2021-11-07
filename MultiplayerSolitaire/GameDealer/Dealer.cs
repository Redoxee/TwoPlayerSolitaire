namespace GameDealer
{
    using System.Threading.Tasks;

    public class Dealer
    {
        private static Dealer instance;

        private readonly Docker.DotNet.DockerClient dockerClient;

        private const string GameImageName = "game-instance";
        private const ushort GamePublicPort = 8081;

        private string gameCrateId = null;

        public static Dealer Instance
        {
            get
            {
                if (Dealer.instance == null)
                {
                    System.Diagnostics.Debug.WriteLine("Dealer not initialized");
                    throw new System.NullReferenceException();
                }

                return Dealer.instance;
            }
        }


        private Dealer()
        {
            this.dockerClient = new Docker.DotNet.DockerClientConfiguration(new System.Uri(Dealer.DockerApiUri())).CreateClient();

            var containerListParameter = new Docker.DotNet.Models.ContainersListParameters
            {
                Filters = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IDictionary<string, bool>>()
                {
                    {
                        "ancestor",
                        new System.Collections.Generic.Dictionary<string,bool>()
                        {
                            { Dealer.GameImageName, true }
                        }
                    }
                },
            };

            var containerListTask = this.dockerClient.Containers.ListContainersAsync(containerListParameter);
            if (!containerListTask.Wait(3000))
            {
                throw new System.Exception("Failed to querry containers");
            }
            
            var containerList = containerListTask.Result;
            foreach (var container in containerList)
            {
                System.Diagnostics.Debug.WriteLine(container.ToString());
                System.Console.WriteLine($"{container.ID} [{string.Join("-", container.Names)}] [{string.Join(",", container.Ports)}] [{container.Image} | {container.ImageID}]");
                if (container.State == "running")
                {
                    foreach (var port in container.Ports)
                    {
                        if (port.PublicPort == Dealer.GamePublicPort)
                        {
                            this.gameCrateId = container.ID;
                        }
                    }
                };
            }
        }

        public static void Initialize()
        {
            Dealer.instance = new Dealer();
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
            string status = this.GetStatus(out _);

            if (status == "Running")
            {
                System.Diagnostics.Debug.WriteLine("Trying to run multiples game instance at a time");
                result.Success = false;
                return;
            }

            var createGameTask = this.RunDockerGameInstance(Dealer.GamePublicPort.ToString());
            createGameTask.Wait();
            result.Success = true;
            result.Port = Dealer.GamePublicPort.ToString();
        }

        public string GetStatus(out string port)
        {
            port = string.Empty;
            if (string.IsNullOrEmpty(this.gameCrateId))
            {
                return "Idle";
            }

            System.Threading.Tasks.Task<Docker.DotNet.Models.ContainerInspectResponse> containerInspectTask = this.dockerClient.Containers.InspectContainerAsync(this.gameCrateId);

            if(!containerInspectTask.Wait(200))
            {
                return "Hanging";
            }

            switch (containerInspectTask.Result.State.Status)
            {
                case "running":
                    {
                        port = containerInspectTask.Result.NetworkSettings.Ports["80/tcp"][^1].HostPort;
                        return "Running";
                    }
                case "exited":
                    return "Exited";
                default:
                    return "Unkown";
            }
        }

        // Based on https://www.danieldonbavand.com/dockerdotnet/
        private async Task<bool> RunDockerGameInstance(string externalPort)
        {
            // Ensuring the image is available.
            await this.dockerClient.Images.CreateImageAsync(new Docker.DotNet.Models.ImagesCreateParameters
            {
                FromImage = Dealer.GameImageName,
            },
            new Docker.DotNet.Models.AuthConfig(),
            new System.Progress<Docker.DotNet.Models.JSONMessage>());

            const string internalPort = "80";

            var creationResponse = await this.dockerClient.Containers.CreateContainerAsync(new Docker.DotNet.Models.CreateContainerParameters
            {
                Image = Dealer.GameImageName,
                ExposedPorts = new System.Collections.Generic.Dictionary<string, Docker.DotNet.Models.EmptyStruct>
                {
                    {
                        internalPort, default
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
