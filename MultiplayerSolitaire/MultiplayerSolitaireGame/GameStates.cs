namespace MultiplayerSolitaireGame
{
    internal class InitializeGameState : GameState
    {
        public override GameStateID StateID => GameStateID.Initialize;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
            Sandbox sandbox = stateMachine.GameManager.Sandbox;
            sandbox.CurrentPlayer = 0;
            sandbox.RoundCount = 0;

            stateMachine.SetNextState(new InitializeRoundState());
        }

        public override Failures ProcessOrder(GameStateMachine stateMachine, GameOrder order, GameChangePool gameChanges)
        {
            return Failures.WrongOrder;
        }
    }

    internal class InitializeRoundState : GameState
    {
        public override GameStateID StateID => GameStateID.Initialize;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
            Sandbox sandbox = stateMachine.GameManager.Sandbox;

            sandbox.DiscardPile.Clear();
            sandbox.Deck.Refill();
            sandbox.Deck.Shuffle();

            for (int index = 0; index < sandbox.Players.Length; ++index)
            {
                Player player = sandbox.Players[index];
                player.Health = 2;
                player.PairBullets = 0;
                player.Shield = 0;
                for (int cardIndex = 0; cardIndex < Player.BoardWidth; ++cardIndex)
                {
                    player.Hand[cardIndex] = sandbox.Deck.PickCard();
                    player.Board[cardIndex].Value = Card.None;
                }
            }

            stateMachine.SetNextState(new PlayTurnState());
        }
        public override Failures ProcessOrder(GameStateMachine stateMachine, GameOrder order, GameChangePool gameChanges)
        {
            return Failures.WrongOrder;
        }
    }

    public class PlayCardOrder : GameOrder
    {
        public int PlayerIndex;
        public int CardIndex;
        public int BoardIndex;
    }

    internal class PlayTurnState : GameState
    {
        public override GameStateID StateID => GameStateID.Playing;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
        }

        public override Failures ProcessOrder(GameStateMachine stateMachine, GameOrder order, GameChangePool gameChanges)
        {
            if (!(order is PlayCardOrder playCardOrder))
            {
                return Failures.WrongOrder;
            }

            Sandbox sandbox = stateMachine.GameManager.Sandbox;
            if (playCardOrder.PlayerIndex != sandbox.CurrentPlayer)
            {
                return Failures.WrongPlayer;
            }

            if (playCardOrder.CardIndex < 0 || playCardOrder.CardIndex >= Player.BoardWidth)
            {
                return Failures.CardOutOfBounds;
            }

            if (playCardOrder.BoardIndex < 0 || playCardOrder.BoardIndex >= Player.BoardWidth)
            {
                return Failures.CardOutOfBounds;
            }

            Player player = sandbox.Players[playCardOrder.PlayerIndex];
            if (player.Board[playCardOrder.BoardIndex].IsValide())
            {
                sandbox.Deck.AddCardUnder(player.Board[playCardOrder.BoardIndex]);
            }

            player.Board[playCardOrder.BoardIndex] = player.Hand[playCardOrder.CardIndex];
            
            player.Hand[playCardOrder.CardIndex] = sandbox.Deck.PickCard();

            ref GameChange playedCard = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayedCard);
            playedCard.PlayerIndex = playCardOrder.PlayerIndex;
            playedCard.IndexInHand = playCardOrder.CardIndex;
            playedCard.IndexOnBoard = playCardOrder.BoardIndex;
            playedCard.Card = player.Board[playCardOrder.BoardIndex];

            ref GameChange pickedCard = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PickedCard);
            pickedCard.PlayerIndex = playCardOrder.PlayerIndex;
            pickedCard.IndexOnBoard = -1;
            pickedCard.Card = player.Hand[playCardOrder.CardIndex];

            stateMachine.SetNextState(new ResolveTurnState());

            return Failures.None;
        }

    }

    internal class ResolveTurnState : GameState
    {
        public override GameStateID StateID => GameStateID.Transitioning;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
            Sandbox sandbox = stateMachine.GameManager.Sandbox;
            Player player = sandbox.Players[sandbox.CurrentPlayer];
            int otherPlayerIndex = sandbox.OtherPlayerIndex();
            Player otherPlayer = sandbox.Players[otherPlayerIndex];

            byte usedCards;
            CardCombo combo = Combo.Compute(player.Board, out usedCards);

            if (combo != CardCombo.None)
            {
                ref GameChange propertyChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);

                for (int index = 0; index < player.Board.Length; ++index)
                {
                    if ((usedCards & 1 << index) != 0)
                    {
                        sandbox.DiscardPile.AddCard(player.Board[index]);
                        player.Board[index].Value = Card.None;
                    }
                }

                if (combo == CardCombo.Pair)
                {
                    player.PairBullets++;

                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.PairBullet;
                    propertyChanged.NewValue = player.PairBullets;
                    propertyChanged.PlayerIndex = player.Index;

                    if (player.PairBullets == 5)
                    {
                        otherPlayer.Health = 0;
                    }
                }
                else if (combo == CardCombo.Flush)
                {
                    player.Shield++;
                
                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.Shield;
                    propertyChanged.NewValue = player.Shield;
                    propertyChanged.PlayerIndex = player.Index;
                }
                else if (combo == CardCombo.Chain)
                {
                    if (otherPlayer.Shield > 0)
                    {
                        otherPlayer.Shield--;

                        propertyChanged.PlayerProperty = GameChange.PlayerProperties.Shield;
                        propertyChanged.NewValue = otherPlayer.Shield;
                        propertyChanged.PlayerIndex = otherPlayerIndex;
                    }
                    else
                    {
                        otherPlayer.Health--;

                        propertyChanged.PlayerProperty = GameChange.PlayerProperties.Health;
                        propertyChanged.NewValue = otherPlayer.Health;
                        propertyChanged.PlayerIndex = otherPlayerIndex;
                    }
                }
                else if (combo == CardCombo.Royal)
                {
                    otherPlayer.Shield = 0;
                    otherPlayer.Health--;

                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.Shield;
                    propertyChanged.NewValue = otherPlayer.Shield;
                    propertyChanged.PlayerIndex = otherPlayerIndex;

                    ref GameChange otherPropertyChange = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                    otherPropertyChange.PlayerProperty = GameChange.PlayerProperties.Health;
                    otherPropertyChange.NewValue = otherPlayer.Health;
                    otherPropertyChange.PlayerIndex = otherPlayerIndex;
                }
            }

            if (otherPlayer.Health <= 0)
            {
                player.Score++;
                sandbox.RoundCount++;
                if (sandbox.RoundCount > 3)
                {
                    stateMachine.SetNextState(new EndGameState());
                }
                else
                {
                    sandbox.CurrentPlayer = otherPlayerIndex;
                    stateMachine.SetNextState(new InitializeRoundState());
                }
            }
            else
            {
                sandbox.CurrentPlayer = otherPlayerIndex;
                stateMachine.SetNextState(new PlayTurnState());
            }
        }

        public override Failures ProcessOrder(GameStateMachine stateMachine, GameOrder order, GameChangePool gameChanges)
        {
            return Failures.WrongOrder;
        }
    }

    internal class EndGameState : GameState
    {
        public override GameStateID StateID => GameStateID.EndGame;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
            ref GameChange playerWon = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerWon);
            playerWon.PlayerIndex = stateMachine.GameManager.Sandbox.CurrentPlayer;
        }

        public override Failures ProcessOrder(GameStateMachine stateMachine, GameOrder order, GameChangePool gameChanges)
        {
            return Failures.WrongOrder;
        }
    }
}
