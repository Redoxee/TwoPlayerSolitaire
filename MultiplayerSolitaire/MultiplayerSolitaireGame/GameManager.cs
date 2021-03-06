namespace MSG
{
    public class GameManager
    {
        internal Sandbox Sandbox;

        internal GameStateMachine stateMachine;

        private readonly System.Text.StringBuilder workingStringBuilder = new ();

        public GameManager(GameParameters gameParameters, GameChangePool gameChanges)
        {
            const int numberOfPlayers = 2;
            this.Sandbox = new Sandbox
            {
                Deck = new Deck(),
                Players = new Player[numberOfPlayers],
                DiscardPile = new CardStack(),
                ScoreTarget = gameParameters.ScoreTarget,
                HealthBaseValue = gameParameters.StaringHealth,
                PairComboSize = gameParameters.PairComboSize,
                MaxHealth = gameParameters.MaxHealth,
            };
        
            for (int index = 0; index < numberOfPlayers; ++index)
            {
                this.Sandbox.Players[index] = new Player(index);
            }

            this.stateMachine = new GameStateMachine(this);
            GameState firstState = new InitializeGameState();
            this.stateMachine.SetInitialState(firstState, gameChanges);
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
            this.workingStringBuilder.AppendLine().Append(" Round : ").Append(this.Sandbox.RoundIndex).AppendLine();

            Player otherPlayer = this.Sandbox.Players[this.Sandbox.OtherPlayerIndex()];
            this.workingStringBuilder.Append("Other Player ").Append(otherPlayer.Index).AppendLine();
            this.workingStringBuilder.Append("Health : ").Append(otherPlayer.Health).AppendLine();
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
            this.workingStringBuilder.Append("Health : ").Append(currentPlayer.Health);
            this.workingStringBuilder.AppendLine();

            this.workingStringBuilder.Append("Hand : ");
            for (int index = 0; index < currentPlayer.Hand.Length; ++index)
            {
                this.workingStringBuilder.Append(currentPlayer.Hand[index]);
            }

            return this.workingStringBuilder.ToString();
        }

        public struct GameParameters
        {
            public int ScoreTarget;
            public int PairComboSize;
            public int StaringHealth;
            public int MaxHealth;

            public static GameParameters Default()
            {
                GameParameters parameters = new()
                {
                    ScoreTarget = 2,
                    PairComboSize = 4,
                    StaringHealth = 2,
                    MaxHealth = 5,
                };

                return parameters;
            }
        }
    }
}
