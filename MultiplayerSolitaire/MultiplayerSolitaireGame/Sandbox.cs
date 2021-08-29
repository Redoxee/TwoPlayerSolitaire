namespace MultiplayerSolitaireGame
{
    public class Sandbox
    {
        public Deck Deck;
        public Player[] Players;

        public CardStack DiscardPile;

        public int CurrentTurn;
        public int CurrentPlayer;

        public int OtherPlayer()
        {
            return (this.CurrentPlayer + 1) % this.Players.Length;
        }
    }
}
