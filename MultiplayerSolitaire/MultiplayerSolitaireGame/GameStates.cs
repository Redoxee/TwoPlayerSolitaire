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
                for (int cardIndex = 0; cardIndex < player.Board.Length; ++cardIndex)
                {
                    player.Board[cardIndex].Value = Card.None;
                }

                for (int cardIndex = 0; cardIndex < player.Hand.Length; ++cardIndex)
                {
                    player.Hand[cardIndex].Value = Card.None;
                }

                player.HandCount = 0;
                int numberOfCardsInHand = player.Index == sandbox.CurrentPlayer ? 3 : 4;
                for (int cardIndex = 0; cardIndex < numberOfCardsInHand; ++cardIndex)
                {
                    player.Hand[player.HandCount++] = sandbox.Deck.PickCard();
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
            if (order is not PlayCardOrder playCardOrder)
            {
                return Failures.WrongOrder;
            }

            Sandbox sandbox = stateMachine.GameManager.Sandbox;
            if (playCardOrder.PlayerIndex != sandbox.CurrentPlayer)
            {
                return Failures.WrongPlayer;
            }

            Player player = sandbox.Players[playCardOrder.PlayerIndex];
            if (playCardOrder.CardIndex < 0 || playCardOrder.CardIndex >= player.Hand.Length)
            {
                return Failures.CardOutOfBounds;
            }

            if (!player.Hand[playCardOrder.CardIndex].IsValide())
            {
                return Failures.CardOutOfBounds;
            }

            if (playCardOrder.BoardIndex < 0 || playCardOrder.BoardIndex >= player.Board.Length)
            {
                return Failures.CardOutOfBounds;
            }

            if (player.Board[playCardOrder.BoardIndex].IsValide())
            {
                sandbox.Deck.AddCardUnder(player.Board[playCardOrder.BoardIndex]);
                sandbox.Deck.Shuffle();
            }

            player.Board[playCardOrder.BoardIndex] = player.Hand[playCardOrder.CardIndex];
            
            player.Hand[playCardOrder.CardIndex].Value = Card.None;
            player.HandCount--;

            ref GameChange playedCard = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayedCard);
            playedCard.PlayerIndex = playCardOrder.PlayerIndex;
            playedCard.IndexInHand = playCardOrder.CardIndex;
            playedCard.IndexOnBoard = playCardOrder.BoardIndex;
            playedCard.Card = player.Board[playCardOrder.BoardIndex];

            if (sandbox.Deck.NumberOfCards > 0 && player.HandCount < 3)
            {
                player.Hand[playCardOrder.CardIndex] = sandbox.Deck.PickCard();
                player.HandCount++;

                playedCard.PackHand = false;

                ref GameChange pickedCard = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PickedCard);
                pickedCard.PlayerIndex = playCardOrder.PlayerIndex;
                pickedCard.IndexOnBoard = -1;
                pickedCard.IndexInHand = playCardOrder.CardIndex;
                pickedCard.Card = player.Hand[playCardOrder.CardIndex];
            }
            else
            {
                // Packing hand
                for (int index = 0; index < player.Hand.Length - 1; ++index)
                {
                    if (!player.Hand[index].IsValide())
                    {
                        player.Hand[index] = player.Hand[index + 1];
                        player.Hand[index + 1].Value = Card.None;
                    }
                }

                playedCard.PackHand = true;
            }

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
            Player playingPlayer = sandbox.Players[sandbox.CurrentPlayer];
            int otherPlayerIndex = sandbox.OtherPlayerIndex();
            Player otherPlayer = sandbox.Players[otherPlayerIndex];

            CardCombo combo = Combo.Compute(playingPlayer.Board, out byte usedCards);

            if (combo != CardCombo.None)
            {
                for (int index = 0; index < playingPlayer.Board.Length; ++index)
                {
                    if ((usedCards & 1 << index) != 0)
                    {
                        sandbox.DiscardPile.AddCard(playingPlayer.Board[index]);
                        playingPlayer.Board[index].Value = Card.None;
                    }
                }

                ref GameChange comboChanges = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerCombo);
                comboChanges.PlayerIndex = playingPlayer.Index;
                comboChanges.CardCombo = combo;
                comboChanges.UsedCards = usedCards;

                if (combo == CardCombo.Pair)
                {
                    for (int index = 0; index < otherPlayer.Board.Length; ++index)
                    {
                        if ((usedCards & 1 << index) != 0 && otherPlayer.Board[index].Value != Card.None)
                        {
                            sandbox.Deck.AddCardUnder(otherPlayer.Board[index]);

                            ref GameChange cardRemoved = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.CardRemovedFromBoard);
                            cardRemoved.Card = otherPlayer.Board[index];
                            cardRemoved.PlayerIndex = otherPlayerIndex;
                            cardRemoved.IndexOnBoard = index;
                            otherPlayer.Board[index].Value = Card.None;

                        }
                    }
                }
                else if (combo == CardCombo.Flush)
                {
                    if (playingPlayer.Health < sandbox.MaxHealth)
                    {
                        playingPlayer.Health++;
                    }

                    ref GameChange propertyChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.Health;
                    propertyChanged.NewValue = playingPlayer.Health;
                    propertyChanged.PlayerIndex = playingPlayer.Index;
                }
                else if (combo == CardCombo.Chain)
                {
                    otherPlayer.Health--;

                    ref GameChange propertyChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.Health;
                    propertyChanged.NewValue = otherPlayer.Health;
                    propertyChanged.PlayerIndex = otherPlayerIndex;
                }
                else if (combo == CardCombo.Royal)
                {
                    otherPlayer.Health-=2;

                    ref GameChange propertyChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                    propertyChanged.PlayerProperty = GameChange.PlayerProperties.Health;
                    propertyChanged.NewValue = otherPlayer.Health;
                    propertyChanged.PlayerIndex = otherPlayerIndex;
                }
            }

            if (otherPlayer.Health <= 0)
            {
                playingPlayer.Score++;

                ref GameChange scoreChanged = ref gameChanges.AllocateGameChange(GameChange.GameChangeType.PlayerPropertyChanged);
                scoreChanged.PlayerProperty = GameChange.PlayerProperties.Score;
                scoreChanged.NewValue = playingPlayer.Score;
                scoreChanged.PlayerIndex = playingPlayer.Index;

                sandbox.RoundIndex++;
                if (playingPlayer.Score >= sandbox.ScoreTarget)
                {
                    stateMachine.SetNextState(new EndGameState());
                    return;
                }

                sandbox.CurrentPlayer = otherPlayerIndex;
                stateMachine.SetNextState(new InitializeRoundState());
                return;
            }

            if (sandbox.Deck.NumberOfCards == 0)
            {
                int bestPlayerGrade = -1;
                int bestPlayerIndex = -1;
                bool even = false;

                for (int playerIndex = 0; playerIndex < sandbox.Players.Length; ++playerIndex)
                {
                    int playerGrad = 0;
                    Player player = sandbox.Players[playerIndex];
                    playerGrad += player.Health * 100;

                    if (playerGrad > bestPlayerGrade)
                    {
                        bestPlayerGrade = playerGrad;
                        bestPlayerIndex = playerIndex;
                    }
                    else if (playerGrad == bestPlayerGrade)
                    {
                        even = true;
                        break;
                    }
                }

                sandbox.CurrentPlayer = even ? -1 : bestPlayerIndex;
                stateMachine.SetNextState(new EndGameState());
                return;
            }

            sandbox.CurrentPlayer = otherPlayerIndex;
            stateMachine.SetNextState(new PlayTurnState());
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
