using System.Collections.Generic;

namespace MSG
{
    public class Player : AMG.ISerializable
    {
        private const int BoardWidth = 3;
        private const int HandSize = 4;

        public Card[] Hand = new Card[Player.HandSize];
        public Card[] Board = new Card[Player.BoardWidth];

        public int Index;
        public int Health;
        public int Score;
        public int HandCount;

        public Player(int index)
        {
            this.Index = index;
            this.HandCount = 0;
        }

        public void Serialize(AMG.Serializer serializer)
        {
            this.Index = serializer.Serialize("Index", this.Index);
            this.Health = serializer.Serialize("Health", this.Health);
            this.Score = serializer.Serialize("Score", this.Score);
            this.Hand = serializer.Serialize("Hand", this.Hand);
            this.HandCount = serializer.Serialize("HandCount", this.HandCount);
            this.Board = serializer.Serialize("Board", this.Board);
        }
    }
}
