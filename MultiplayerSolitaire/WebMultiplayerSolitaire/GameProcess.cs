namespace WebMultiplayerSolitaire
{
    public class GameProcess
    {
        private static GameProcess instance;

        private readonly MSG.GameManager gameManager;

        private readonly ConnectedClient[] clientByPlayerIndex = null;

        private readonly MSG.GameChangePool workingGameChanges;

        private readonly int numberOfFaces;

        public static GameProcess Instance
        {
            get
            {
                if (GameProcess.instance == null)
                {
                    GameProcess.instance = new GameProcess();
                }

                return GameProcess.instance;
            }
        }

        private GameProcess()
        {
            this.workingGameChanges = new MSG.GameChangePool();
            this.gameManager = new MSG.GameManager(MSG.GameManager.GameParameters.Default(), this.workingGameChanges);

            this.clientByPlayerIndex = new ConnectedClient[2];
            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                this.clientByPlayerIndex[index] = null;
            }

            // Faces
            System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("WebCardGame.Properties.Resources", typeof(Program).Assembly);
            string faceConfigFile = resourceManager.GetString("Config");

            System.IO.StringReader stringReader = new System.IO.StringReader(faceConfigFile);
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            JSONConfig config = (JSONConfig)serializer.Deserialize(stringReader, typeof(JSONConfig));
            this.numberOfFaces = config.FacesData.Length;
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
            PlayerViewUpdate view = new PlayerViewUpdate
            {
                CardsInDeck = sandbox.Deck.NumberOfCards,
                CardsInDiscardPile = sandbox.DiscardPile.Count,
                PlayerTurn = sandbox.CurrentPlayer,
                ScoreTarget = sandbox.ScoreTarget,
                PairComboSize = sandbox.PairComboSize,
            };


            view.CurrentPlayer.Hand = new MSG.Card[player.Hand.Length];
            player.Hand.CopyTo(view.CurrentPlayer.Hand, 0);
            view.CurrentPlayer.Board = new MSG.Card[player.Board.Length];
            player.Board.CopyTo(view.CurrentPlayer.Board, 0);
            view.CurrentPlayer.Health = player.Health;
            view.CurrentPlayer.PairCombo = player.PairCombo;
            view.CurrentPlayer.Score = player.Score;
            view.CurrentPlayer.Index = player.Index;
            view.CurrentPlayer.FaceIndex = this.clientByPlayerIndex[playerIndex].FaceIndex;

            view.GameStateID = this.gameManager.GetStateID();

            int otherPlayerIndex = sandbox.OtherPlayerIndex(playerIndex);
            MSG.Player otherPlayer = sandbox.Players[otherPlayerIndex];
            view.OtherPlayer.Index = otherPlayerIndex;
            view.OtherPlayer.Score = otherPlayer.Score;
            view.OtherPlayer.Health = otherPlayer.Health;
            view.OtherPlayer.PairCombo = otherPlayer.PairCombo;
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
            System.IO.StringReader stringReader = new System.IO.StringReader(messageString);
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            JSONOrder order;
            try
            {
                order = (JSONOrder)serializer.Deserialize(stringReader, typeof(JSONOrder));
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error while parsing socket message");
                Program.ReportException(ex);
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

                        OrderAcknowledgement acknowledgement = new OrderAcknowledgement() { OrderID = order.OrderID, FailureFlags = MSG.Failures.None, PlayerIndex = availableIndex };
                        this.SendResponseToClient(acknowledgement, client);

                        AvailableFaces availableFaces = this.RequestAvailableFaces();
                        if (!availableFaces.ReadyToPlay)
                        {
                            this.BroadCast(availableFaces);
                        }
                        else
                        {
                            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
                            {
                                ConnectedClient connectedClient = this.clientByPlayerIndex[index];
                                if (connectedClient != null)
                                {
                                    PlayerViewUpdate playerViewUpdate = this.GetPlayerView(connectedClient.PlayerIndex);
                                    this.SendResponseToClient(playerViewUpdate, connectedClient);
                                }
                            }
                        }

                        break;
                    }

                case "RequestPlayerFaces":
                    {
                        JSONResponse response = this.RequestAvailableFaces();
                        this.SendResponseToClient(response, client);

                        break;
                    }

                case "PlayCard":
                    {
                        int playerIndex = client.PlayerIndex;
                        int cardIndex = order.CardIndex;
                        int boardIndex = order.BoardIndex;

                        MSG.PlayCardOrder playOrder = new MSG.PlayCardOrder()
                        {
                            PlayerIndex = playerIndex,
                            CardIndex = cardIndex,
                            BoardIndex = boardIndex,
                        };

                        this.workingGameChanges.Clear();
                        MSG.Failures failures = this.gameManager.ProcessOrder(playOrder, this.workingGameChanges);

                        OrderAcknowledgement acknowledgement = new OrderAcknowledgement()
                        {
                            OrderID = order.OrderID,
                            FailureFlags = failures,
                        };

                        this.SendResponseToClient(acknowledgement, client);

                        if (failures == MSG.Failures.None)
                        {
                            SandboxChanges sandboxChanges = new SandboxChanges()
                            {
                                GameChanges = this.workingGameChanges.GetGameChanges(),
                            };

                            if (this.DoNeedUpdatePlayerView(sandboxChanges))
                            {
                                for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
                                {
                                    ConnectedClient connectedClient = this.clientByPlayerIndex[index];
                                    if (connectedClient != null)
                                    {
                                        PlayerViewUpdate playerViewUpdate = this.GetPlayerView(connectedClient.PlayerIndex);
                                        playerViewUpdate.IsNextRoundState = true;
                                        this.SendResponseToClient(playerViewUpdate, connectedClient);
                                    }
                                }
                            }

                            this.BroadCast(sandboxChanges);
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

        private bool DoNeedUpdatePlayerView(SandboxChanges sandboxChanges)
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

        private JSONResponse RequestAvailablePlayerSlots()
        {
            AvailablePlayerSlot response = new AvailablePlayerSlot
            {
                AvaialablePlayerSlots = new bool[this.clientByPlayerIndex.Length],
            };

            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                response.AvaialablePlayerSlots[index] = this.clientByPlayerIndex[index] == null;
            }

            return response;
        }

        private AvailableFaces RequestAvailableFaces()
        {
            AvailableFaces response = new AvailableFaces
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

        private void SendResponseToClient(JSONResponse response, ConnectedClient client)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            Newtonsoft.Json.JsonTextWriter textWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter);
            serializer.Serialize(textWriter, response);
            stringWriter.Close();
            string message = stringWriter.ToString();
            System.Console.WriteLine($"Sending message \"{message}\".");
            client.MessageQueue.Add(message);
        }

        private void BroadCast(JSONResponse response)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            Newtonsoft.Json.JsonTextWriter textWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter);
            serializer.Serialize(textWriter, response);
            stringWriter.Close();
            string message = stringWriter.ToString();
            WebSocketMiddleware.Broadcast(message);
        }
    }
}
