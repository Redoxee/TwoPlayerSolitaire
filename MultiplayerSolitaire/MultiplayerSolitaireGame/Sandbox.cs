namespace MSG
{
    public class Sandbox
    {
        public Deck Deck;
        public CardStack DiscardPile;

        public Player[] Players;
        public int CurrentPlayer;

        public int RoundCount;

        public int OtherPlayerIndex()
        {
            return (this.CurrentPlayer + 1) % this.Players.Length;
        }
    }
}
