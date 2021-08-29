using System.Collections.Generic;

namespace MultiplayerSolitaireGame
{
    public class Player
    {
        public readonly int Index;

        public const int BoardWidth = 3;

        public Card[] Hand = new Card[Player.BoardWidth];
        public Card[] Board = new Card[Player.BoardWidth];

        public int Shield;
        public int Health;
        public int PairBullets;

        public int Score;

        public Player(int index)
        {
            this.Index = index;
        }
    }
}
