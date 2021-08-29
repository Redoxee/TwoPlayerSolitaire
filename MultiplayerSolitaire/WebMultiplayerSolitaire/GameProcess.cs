namespace WebMultiplayerSolitaire
{
    public class GameProcess
    {
        private static GameProcess instance;

        private MultiplayerSolitaireGame.GameManager gameManager;

        ConnectedClient[] clientByPlayerIndex = null;

        MultiplayerSolitaireGame.GameChangePool workingGameChanges;

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
        }

        public void InitializeGame()
        {
            this.workingGameChanges = new MultiplayerSolitaireGame.GameChangePool();
            this.gameManager = new MultiplayerSolitaireGame.GameManager(this.workingGameChanges);

            this.clientByPlayerIndex = new ConnectedClient[2];
            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                this.clientByPlayerIndex[index] = null;
            }
        }

        public MultiplayerSolitaireGame.GameManager GetGameManager()
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
            MultiplayerSolitaireGame.Sandbox sandbox = this.gameManager.GetSandbox();
            MultiplayerSolitaireGame.Player player = sandbox.Players[playerIndex];
            PlayerViewUpdate view = new PlayerViewUpdate();
            view.PlayerIndex = playerIndex;
            view.Score = player.Score;
            view.Health = player.Health;
            view.Shield = player.Shield;
            view.PairBullets = player.PairBullets;

            view.Hand = new MultiplayerSolitaireGame.Card[player.Hand.Length];
            view.Board = new MultiplayerSolitaireGame.Card[player.Board.Length];
            player.Hand.CopyTo(view.Hand, player.Hand.Length);
            player.Board.CopyTo(view.Board, player.Board.Length);
            
            view.DiscardPile = new MultiplayerSolitaireGame.Card[sandbox.DiscardPile.Count];
            sandbox.DiscardPile.Data.CopyTo(view.DiscardPile, sandbox.DiscardPile.Count);

            view.GameStateID = this.gameManager.GetStateID();
            view.CurrentPlayer = sandbox.CurrentPlayer;

            int otherPlayerIndex = sandbox.OtherPlayerIndex();
            MultiplayerSolitaireGame.Player otherPlayer = sandbox.Players[otherPlayerIndex];
            view.OtherPlayer.Index = otherPlayerIndex;
            view.OtherPlayer.Score = otherPlayer.Score;
            view.OtherPlayer.Health = otherPlayer.Health;
            view.OtherPlayer.Shield = otherPlayer.Shield;
            view.OtherPlayer.PairBullets = otherPlayer.PairBullets;

            view.OtherPlayer.Board = new MultiplayerSolitaireGame.Card[otherPlayer.Board.Length];
            otherPlayer.Board.CopyTo(view.OtherPlayer.Board, otherPlayer.Board.Length);

            return view;
        }

        public string GetSandboxJson()
        {
            string result = string.Empty;

            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            Newtonsoft.Json.JsonTextWriter textWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter);

            PlayerViewUpdate playerView = this.GetPlayerView(0);

            serializer.Serialize(textWriter, playerView);
            stringWriter.Close();
            result = stringWriter.ToString();
            return result;
        }

        public void HandleMessage(ConnectedClient client, string messageString)
        {
            System.IO.StringReader stringReader = new System.IO.StringReader(messageString);
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            JSONOrder order = null;
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

            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            Newtonsoft.Json.JsonTextWriter textWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter);

            switch (order.OrderType)
            {
                case "SelectPlayerSlot":
                    {
                        if (order.PlayerIndex < 0)
                        {
                            System.Console.WriteLine("Missing PlayerIndex");
                            return;
                        }
                        
                        if (this.TryRegisterClient(client, order.PlayerIndex))
                        {
                            OrderAcknowledgement acknowledgement = new OrderAcknowledgement() { OrderID = order.OrderID, FailureFlags = MultiplayerSolitaireGame.Failures.None };
                            this.SendResponseToClient(acknowledgement, client);

                            PlayerViewUpdate playerView = this.GetPlayerView(client.PlayerIndex);
                            this.SendResponseToClient(playerView, client);
                        }
                        else
                        {
                            System.Console.WriteLine($"Couldn't register to player slot {order.PlayerIndex}");
                        }

                        JSONResponse availableSlots = this.RequestAvailablePlayerSlots();
                        this.BroadCast(availableSlots);

                        break;
                    }
                case "RequestPlayerSlots":
                    {
                        JSONResponse response = this.RequestAvailablePlayerSlots();
                        this.SendResponseToClient(response, client);

                        break;
                    }

                case "PlayCard":
                    {
                        int playerIndex = client.PlayerIndex;
                        int cardIndex = order.CardIndex;
                        int boardIndex = order.BoardIndex;

                        MultiplayerSolitaireGame.PlayCardOrder playOrder = new MultiplayerSolitaireGame.PlayCardOrder()
                        {
                            PlayerIndex = playerIndex,
                            CardIndex = cardIndex,
                            BoardIndex = boardIndex,
                        };

                        this.workingGameChanges.Clear();
                        MultiplayerSolitaireGame.Failures failures = this.gameManager.ProcessOrder(playOrder, this.workingGameChanges);

                        OrderAcknowledgement acknowledgement = new OrderAcknowledgement()
                        {
                            OrderID = order.OrderID,
                            FailureFlags = failures,
                        };

                        this.SendResponseToClient(acknowledgement, client);

                        if (failures == MultiplayerSolitaireGame.Failures.None)
                        {
                            SandboxChanges sandboxChanges = new SandboxChanges()
                            {
                                GameChanges = this.workingGameChanges.GetGameChanges(),
                            };

                            this.AddPlayerViewIfNecessary(sandboxChanges, client.PlayerIndex);

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

        private void AddPlayerViewIfNecessary(SandboxChanges sandboxChanges, int playerIndex)
        {
            for (int index = 0; index < sandboxChanges.GameChanges.Length; ++index)
            {
                if (sandboxChanges.GameChanges[index].ChangeType == MultiplayerSolitaireGame.GameChange.GameChangeType.GameStateChange &&
                    sandboxChanges.GameChanges[index].GameState == MultiplayerSolitaireGame.GameStateID.Initialize)
                {
                    sandboxChanges.PlayerViewUpdate = this.GetPlayerView(playerIndex);
                    return;
                }
            }
        }

        private JSONResponse RequestAvailablePlayerSlots()
        {
            AvailablePlayerSlot response = new AvailablePlayerSlot();
            response.AvaialablePlayerSlots = new bool[this.clientByPlayerIndex.Length];
            for (int index = 0; index < this.clientByPlayerIndex.Length; ++index)
            {
                response.AvaialablePlayerSlots[index] = this.clientByPlayerIndex[index] == null;
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
