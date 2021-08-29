﻿namespace MultiplayerSolitaireGame
{
    public class GameManager
    {
        internal Sandbox Sandbox;

        internal GameStateMachine stateMachine;

        System.Text.StringBuilder workingStringBuilder = new System.Text.StringBuilder();

        public GameManager(GameChangePool gameChanges = null)
        {
            const int numberOfPlayers = 2;
            this.Sandbox = new Sandbox();
            this.Sandbox.Deck = new Deck();
            this.Sandbox.Players = new Player[numberOfPlayers];
            this.Sandbox.DiscardPile = new CardStack();

            for (int index = 0; index < numberOfPlayers; ++index)
            {
                this.Sandbox.Players[index] = new Player(index);
            }

            this.Sandbox.CurrentPlayer = 0;
            this.Sandbox.CurrentTurn = 0;

            this.stateMachine = new GameStateMachine(this);
            GameState firstState = new InitializeGameState();
            this.stateMachine.SetInitialState(firstState, gameChanges);
        }

        public bool IsGameFinished()
        {
            return false;
        }

        public Sandbox GetSandbox()
        {
            return this.Sandbox;
        }

        public Failures ProcessOrder(GameOrder order, GameChangePool gameChanges)
        {
            return this.stateMachine.ProcessOrder(order, gameChanges);
        }

        public GameStateID GetStateID()
        {
            return this.stateMachine.GetStateID();
        }

        public string GetDebugString()
        {
            this.workingStringBuilder.Clear();
            this.workingStringBuilder.Append(this.stateMachine.GetDebugString());
            this.workingStringBuilder.AppendLine();

            Player otherPlayer = this.Sandbox.Players[this.Sandbox.OtherPlayer()];
            this.workingStringBuilder.Append("Other Player ").Append(otherPlayer.Index).AppendLine();
            this.workingStringBuilder.Append("Health : ").Append(otherPlayer.Health).Append(" Shield : ").Append(otherPlayer.Shield).Append(" PairBullets : ").Append(otherPlayer.PairBullet).AppendLine();
            this.workingStringBuilder.Append("Board : ");
            for (int index = 0; index < 3; ++index)
            {
                this.workingStringBuilder.Append(otherPlayer.Board[index]);
            }

            this.workingStringBuilder.AppendLine();

            Player currentPlayer = this.Sandbox.Players[this.Sandbox.CurrentPlayer];

            this.workingStringBuilder.Append("Current player ").Append(currentPlayer.Index).AppendLine();
            this.workingStringBuilder.Append("Board : ");
            for (int index = 0; index < 3; ++index)
            {
                this.workingStringBuilder.Append(currentPlayer.Board[index]);
            }

            this.workingStringBuilder.AppendLine();
            this.workingStringBuilder.Append("Health : ").Append(currentPlayer.Health).Append(" Shield : ").Append(currentPlayer.Shield).Append(" PairBullets : ").Append(currentPlayer.PairBullet);
            this.workingStringBuilder.AppendLine();

            this.workingStringBuilder.Append("Hand : ");
            for (int index = 0; index < 3; ++index)
            {
                this.workingStringBuilder.Append(currentPlayer.Hand[index]);
            }

            return this.workingStringBuilder.ToString();
        }
    }
}
