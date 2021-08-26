namespace MultiplayerSolitaireGame
{
    public class GameManager
    {
        internal Sandbox Sandbox;

        internal GameStateMachine stateMachine;

        public GameManager(GameChangePool gameChanges = null)
        {
            const int numberOfPlayers = 2;
            this.Sandbox = new Sandbox();
            this.Sandbox.Deck = new Deck();
            this.Sandbox.Players = new Player[numberOfPlayers];

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
            return this.stateMachine.GetDebugString();
        }
    }
}
