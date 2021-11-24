namespace MSGWeb
{
    internal class GameProcess
    {
        private static GameProcess instance;

        private readonly MSG.GameManager gameManager;

        private readonly ConnectedClient[] clientByPlayerIndex = null;

        private readonly MSG.GameChangePool workingGameChanges;

        private readonly int numberOfFaces;

        public static GameProcess Initialize(MSGWeb.Parameters parameters)
        {
            if (GameProcess.instance != null)
            {
                System.Console.Error.WriteLine("An instance of game process has already been initialized.");
                return GameProcess.instance;
            }

            GameProcess.instance = new GameProcess(parameters);
            return GameProcess.instance;
        }

        public static GameProcess Instance
        {
            get
            {
                if (GameProcess.instance == null)
                {
                    System.Console.Error.WriteLine("No game process initialized");
                }

                return GameProcess.instance;
            }
        }

        private GameProcess(MSGWeb.Parameters parameters)
        {
            this.workingGameChanges = new MSG.GameChangePool();

            this.gameManager = new MSG.GameManager(parameters.GameParameters, this.workingGameChanges);

            this.clientByPlayerIndex = new ConnectedClient[2];
            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                this.clientByPlayerIndex[index] = null;
            }

            // Faces.
            {
                System.Resources.ResourceManager resourceManager = new("WebCardGame.Properties.Resources", typeof(MSGWeb).Assembly);
                string faceConfigFile = resourceManager.GetString("Config");

                System.IO.StringReader stringReader = new(faceConfigFile);
                Newtonsoft.Json.JsonSerializer serializer = new();
                JSONConfig config = (JSONConfig)serializer.Deserialize(stringReader, typeof(JSONConfig));
                this.numberOfFaces = config.FacesData.Length;
            }

            // Save load.
            if (!string.IsNullOrEmpty(parameters.LoadSavePath))
            {
                AMG.Serializer serializer = new AMG.Serializer();
                System.IO.StreamReader reader = new System.IO.StreamReader(parameters.LoadSavePath);
                serializer.StartRead(reader);

                this.gameManager.GetSandbox().Serialize(serializer);
            }
        }

        public MSG.GameManager GetGameManager()
        {
            return this.gameManager;
        }

        public bool TryRegisterClient(ConnectedClient client, int requestedIndex)
        {
            if (requestedIndex < 0 || requestedIndex >= this.clientByPlayerIndex.Length)
            {
                return false;
            }

            if (client == null)
            {
                return false;
            }

            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                if (this.clientByPlayerIndex[index] == client)
                {
                    return false;
                }
            }

            if (this.clientByPlayerIndex[requestedIndex] != null)
            {
                return false;
            }

            this.clientByPlayerIndex[requestedIndex] = client;
            client.PlayerIndex = requestedIndex;
            return true;
        }

        public bool TryUnregisterClient(ConnectedClient client)
        {
            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                if (this.clientByPlayerIndex[index] == client)
                {
                    this.clientByPlayerIndex[index].PlayerIndex = -1;
                    this.clientByPlayerIndex[index] = null;
                    return true;
                }
            }

            return false;
        }

        public PlayerViewUpdate GetPlayerView(int playerIndex)
        {
            MSG.Sandbox sandbox = this.gameManager.GetSandbox();
            MSG.Player player = sandbox.Players[playerIndex];
            PlayerViewUpdate view = new()
            {
                CardsInDeck = sandbox.Deck.NumberOfCards,
                CardsInDiscardPile = sandbox.DiscardPile.Count,
                PlayerTurn = sandbox.CurrentPlayer,
                ScoreTarget = sandbox.ScoreTarget,
                PairComboSize = sandbox.PairComboSize,
                RoundIndex = sandbox.RoundIndex,
            };


            view.CurrentPlayer.Hand = new MSG.Card[player.Hand.Length];
            player.Hand.CopyTo(view.CurrentPlayer.Hand, 0);
            view.CurrentPlayer.Board = new MSG.Card[player.Board.Length];
            player.Board.CopyTo(view.CurrentPlayer.Board, 0);
            view.CurrentPlayer.Health = player.Health;
            view.CurrentPlayer.Score = player.Score;
            view.CurrentPlayer.Index = player.Index;
            view.CurrentPlayer.FaceIndex = this.clientByPlayerIndex[playerIndex].FaceIndex;

            view.GameStateID = this.gameManager.GetStateID();

            int otherPlayerIndex = sandbox.OtherPlayerIndex(playerIndex);
            MSG.Player otherPlayer = sandbox.Players[otherPlayerIndex];
            view.OtherPlayer.Index = otherPlayerIndex;
            view.OtherPlayer.Score = otherPlayer.Score;
            view.OtherPlayer.Health = otherPlayer.Health;
            if (this.clientByPlayerIndex[otherPlayerIndex] != null)
            {
                view.OtherPlayer.FaceIndex = this.clientByPlayerIndex[otherPlayerIndex].FaceIndex;
            }
            else
            {
                view.OtherPlayer.FaceIndex = -1;
            }

            view.OtherPlayer.Board = new MSG.Card[otherPlayer.Board.Length];
            otherPlayer.Board.CopyTo(view.OtherPlayer.Board, 0);

            return view;
        }

        public void HandleMessage(ConnectedClient client, string messageString)
        {
            System.IO.StringReader stringReader = new(messageString);
            Newtonsoft.Json.JsonSerializer serializer = new();
            JSONOrder order;
            try
            {
                order = (JSONOrder)serializer.Deserialize(stringReader, typeof(JSONOrder));
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error while parsing socket message");
                MSGWeb.ReportException(ex);
                return;
            }

            if (order == null)
            {
                System.Console.WriteLine("Error while parsing socket message into json");
                return;
            }

            switch (order.OrderType)
            {
                case "SelectPlayerFace":
                    {
                        if (order.FaceIndex < 0)
                        {
                            System.Console.WriteLine("Missing FaceIndex");
                            return;
                        }

                        if (client.FaceIndex > -1)
                        {
                            System.Console.WriteLine("Client has already a face selected");
                            return;
                        }
                        
                        int availableIndex = -1;
                        for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
                        {
                            if(this.clientByPlayerIndex[index] == null)
                            {
                                availableIndex = index;
                                break;
                            }
                        }

                        if (availableIndex < 0)
                        {
                            System.Console.WriteLine("No available player index");
                            return;
                        }

                        this.TryRegisterClient(client, availableIndex);
                        client.FaceIndex = order.FaceIndex;

                        OrderAcknowledgement acknowledgement = new() { OrderID = order.OrderID, FailureFlags = MSG.Failures.None, PlayerIndex = availableIndex };
                        GameProcess.SendResponseToClient(acknowledgement, client);

                        AvailableFaces availableFaces = this.RequestAvailableFaces();
                        if (!availableFaces.ReadyToPlay)
                        {
                            GameProcess.BroadCast(availableFaces);
                        }
                        else
                        {
                            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
                            {
                                ConnectedClient connectedClient = this.clientByPlayerIndex[index];
                                if (connectedClient != null)
                                {
                                    PlayerViewUpdate playerViewUpdate = this.GetPlayerView(connectedClient.PlayerIndex);
                                    GameProcess.SendResponseToClient(playerViewUpdate, connectedClient);
                                }
                            }
                        }

                        break;
                    }

                case "RequestPlayerFaces":
                    {
                        JSONResponse response = this.RequestAvailableFaces();
                        GameProcess.SendResponseToClient(response, client);

                        break;
                    }

                case "PlayCard":
                    {
                        int playerIndex = client.PlayerIndex;
                        int cardIndex = order.CardIndex;
                        int boardIndex = order.BoardIndex;

                        MSG.PlayCardOrder playOrder = new()
                        {
                            PlayerIndex = playerIndex,
                            CardIndex = cardIndex,
                            BoardIndex = boardIndex,
                        };

                        this.workingGameChanges.Clear();
                        MSG.Failures failures = this.gameManager.ProcessOrder(playOrder, this.workingGameChanges);

                        OrderAcknowledgement acknowledgement = new()
                        {
                            OrderID = order.OrderID,
                            FailureFlags = failures,
                        };

                        GameProcess.SendResponseToClient(acknowledgement, client);

                        if (failures == MSG.Failures.None)
                        {
                            SandboxChanges sandboxChanges = new()
                            {
                                GameChanges = this.workingGameChanges.GetGameChanges(),
                            };

                            if (GameProcess.DoNeedUpdatePlayerView(sandboxChanges))
                            {
                                for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
                                {
                                    ConnectedClient connectedClient = this.clientByPlayerIndex[index];
                                    if (connectedClient != null)
                                    {
                                        PlayerViewUpdate playerViewUpdate = this.GetPlayerView(connectedClient.PlayerIndex);
                                        playerViewUpdate.IsNextRoundState = true;
                                        GameProcess.SendResponseToClient(playerViewUpdate, connectedClient);
                                    }
                                }
                            }

                            GameProcess.BroadCast(sandboxChanges);
                        }

                        break;
                    }

                default:
                    {
                        System.Console.WriteLine($"Unkown orderType : {order.OrderType}.");
                        return;
                    }
            }
        }

        private static bool DoNeedUpdatePlayerView(SandboxChanges sandboxChanges)
        {
            for (int index = 0; index < sandboxChanges.GameChanges.Length; ++index)
            {
                if (sandboxChanges.GameChanges[index].ChangeType == MSG.GameChange.GameChangeType.GameStateChange)
                {
                    if (sandboxChanges.GameChanges[index].GameStateID == MSG.GameStateID.Initialize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private AvailableFaces RequestAvailableFaces()
        {
            AvailableFaces response = new()
            {
                Faces = new bool[this.numberOfFaces],
                ReadyToPlay = true,
            };

            for (int index = 0; index < response.Faces.Length; ++index)
            {
                response.Faces[index] = true;
            }

            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                if (this.clientByPlayerIndex[index] != null)
                {
                    int faceIndex = this.clientByPlayerIndex[index].FaceIndex;
                    if (faceIndex > -1)
                    {
                        response.Faces[faceIndex] = false;
                    }
                }
                else
                {
                    response.ReadyToPlay = false;
                }
            }

            return response;
        }

        private static void SendResponseToClient(JSONResponse response, ConnectedClient client)
        {
            Newtonsoft.Json.JsonSerializer serializer = new();
            System.IO.StringWriter stringWriter = new();
            Newtonsoft.Json.JsonTextWriter textWriter = new(stringWriter);
            serializer.Serialize(textWriter, response);
            stringWriter.Close();
            string message = stringWriter.ToString();
            System.Console.WriteLine($"Sending message \"{message}\".");
            client.MessageQueue.Add(message);
        }

        private static void BroadCast(JSONResponse response)
        {
            Newtonsoft.Json.JsonSerializer serializer = new();
            System.IO.StringWriter stringWriter = new();
            Newtonsoft.Json.JsonTextWriter textWriter = new(stringWriter);
            serializer.Serialize(textWriter, response);
            stringWriter.Close();
            string message = stringWriter.ToString();
            WebSocketMiddleware.Broadcast(message);
        }
    }
}
