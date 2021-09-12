using System.Collections.Generic;

namespace MSG
{
    public class Player
    {
        public readonly int Index;

        public const int BoardWidth = 3;
        public const int HandSize = 3;

        public Card[] Hand = new Card[Player.BoardWidth];
        public Card[] Board = new Card[Player.BoardWidth];

        public int Shield;
        public int Health;
        public int PairCombo;

        public int Score;

        public Player(int index)
        {
            this.Index = index;
        }
    }
}
