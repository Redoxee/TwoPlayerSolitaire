﻿namespace MSG
{
    public class Sandbox
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
    }
}
