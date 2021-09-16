namespace MSG
{
    internal class InitializeGameState : GameState
    {
        public override GameStateID StateID => GameStateID.Initialize;

        public override void StartState(GameStateMachine stateMachine, GameChangePool gameChanges)
        {
            Sandbox sandbox = stateMachine.GameManager.Sandbox;
            sandbox.CurrentPlayer = 0;
            sandbox.RoundIndex = 0;

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
                player.Health = sandbox.HealthBaseValue;
                player.PairCombo = 0;
                player.Shield = 0;
                for (int cardIndex = 0; cardIndex < Player.BoardWidth; ++cardIndex)
                {
                    player.Board[cardIndex].Value = Card.None;
                }

                for (int cardIndex = 0; cardIndex < Player.HandSize; ++cardIndex)
                {
                    player.Hand[cardIndex] = sandbox.Deck.PickCard();
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
                sandbox.Deck.Shuffle();
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
            pickedCard.IndexInHand = playCardOrder.CardIndex;
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

            CardCombo combo = Combo.Compute(player.Board, out byte usedCards);

            if (combo != CardCombo.None)
            {
                for (int index = 0; index < player.Board.Length; ++index)
                {
                    if ((usedCards & 1 << index) != 0)
                    {
                        sandbox.DiscardPile.AddCard(player.Board[index]);
                        player.Board[index].Value = Card.None;
                    }
                }

                ref GameChange comboChanges = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerCombo);
                comboChanges.PlayerIndex = player.Index;
                comboChanges.CardCombo = combo;
                comboChanges.UsedCards = usedCards;

                ref GameChange propertyChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);

                if (combo == CardCombo.Pair)
                {
                    player.PairCombo++;

                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.PairBullets;
                    propertyChanged.NewValue = player.PairCombo;
                    propertyChanged.PlayerIndex = player.Index;

                    if (player.PairCombo == sandbox.PairComboSize)
                    {
                        otherPlayer.Health = 0;
                    }
                }
                else if (combo == CardCombo.Flush)
                {
                    if(player.Shield < sandbox.MaxShield)
                    {
                        player.Shield++;
                    }
                
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
                
                ref GameChange scoreChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                scoreChanged.PlayerProperty = GameChange.PlayerProperties.Score;
                scoreChanged.NewValue = player.Score;
                scoreChanged.PlayerIndex = player.Index;

                sandbox.RoundIndex++;
                if (player.Score >= sandbox.ScoreTarget)
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
