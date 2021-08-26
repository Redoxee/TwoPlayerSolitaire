namespace MultiplayerSolitaireGame
{
    public class Sandbox
    {
        public Deck Deck;
        public Player[] Players;

        public PlayedCard[] DiscardPile;

        public int CurrentTurn;
        public int CurrentPlayer;

        public int NumberOfPlayers
        {
            get => this.Players.Length;
        }
    }
}
