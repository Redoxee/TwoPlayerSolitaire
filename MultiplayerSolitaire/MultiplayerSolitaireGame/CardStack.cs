namespace MultiplayerSolitaireGame
{
    public class CardStack
    {
        public Card[] Data;
        public int Count;

        public CardStack()
        {
            this.Data = new Card[54];
            this.Count = 0;
        }

        public void AddCard(Card card)
        {
            if (this.Count == this.Data.Length - 1)
            {
                System.Array.Resize(ref this.Data, this.Data.Length * 2);
            }

            this.Data[this.Count++] = card;
        }

        public void Clear()
        {
            this.Count = 0;
        }
    }
}
