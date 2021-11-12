using System.Collections.Generic;

namespace MSG
{
    public class Player : AMG.ISerializable
    {
        public const int BoardWidth = 3;
        public const int HandSize = 3;

        public Card[] Hand = new Card[Player.BoardWidth];
        public Card[] Board = new Card[Player.BoardWidth];

        public int Index;
        public int Health;
        public int Score;

        public Player(int index)
        {
            this.Index = index;
        }

        public void Serialize(AMG.Serializer serializer)
        {
            this.Index = serializer.Serialize("Index", this.Index);
            this.Health = serializer.Serialize("Health", this.Health);
            this.Score = serializer.Serialize("Score", this.Score);
            this.Hand = serializer.Serialize("Hand", this.Hand);
            this.Board = serializer.Serialize("Board", this.Board);
        }
    }
}
