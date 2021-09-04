using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMultiplayerSolitaire
{
    [System.Serializable]
    public class PlayerViewUpdate : JSONResponse
    {
        public override string MessageType => "PlayerViewUpdate";

        public MSG.GameStateID GameStateID;
        public int PlayerIndex;

        public MSG.Card[] Hand;
        public MSG.Card[] Board;

        public int Score;
        public int Health;
        public int Shield;
        public int PairBullets;

        public Player OtherPlayer;
        public int CurrentPlayer;
        public int Round;
        public MSG.Card[] DiscardPile;
        public int CardsInDeck;

        public struct Player
        {
            public int Index;
            public int Score;
            public int Health;
            public int Shield;
            public int PairBullets;
            public MSG.Card[] Board;
        }
    }
}
