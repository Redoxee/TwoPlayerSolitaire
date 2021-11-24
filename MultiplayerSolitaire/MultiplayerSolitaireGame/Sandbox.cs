using AMG;

namespace MSG
{
    public class Sandbox : AMG.ISerializable
    {
        public Deck Deck;
        public CardStack DiscardPile;

        public Player[] Players;
        public int CurrentPlayer;

        public int RoundIndex;

        public int PairComboSize = 3;
        public int HealthBaseValue = 2;
        public int MaxHealth = 2;
        public int ScoreTarget = 2;

        public int OtherPlayerIndex()
        {
            return (this.CurrentPlayer + 1) % this.Players.Length;
        }

        public int OtherPlayerIndex(int lookingPlayerIndex)
        {
            return (lookingPlayerIndex + 1) % this.Players.Length;
        }

        public void Serialize(Serializer serializer)
        {
            this.CurrentPlayer = serializer.Serialize("CurrentPlayer", this.CurrentPlayer);
            this.RoundIndex = serializer.Serialize("RoundIndex", this.RoundIndex);
            this.PairComboSize = serializer.Serialize("PairComboSize", this.PairComboSize);
            this.HealthBaseValue = serializer.Serialize("HealthBaseValue", this.HealthBaseValue);
            this.MaxHealth = serializer.Serialize("MaxHealth", this.MaxHealth);
            this.ScoreTarget = serializer.Serialize("ScoreTarget", this.ScoreTarget);

            this.Deck = serializer.Serialize("Deck", this.Deck);
            this.DiscardPile = serializer.Serialize("DiscradPile", this.DiscardPile);
            this.Players = serializer.Serialize("Players", this.Players);
        }
    }
}
