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

        public Player CurrentPlayer;
        public Player OtherPlayer;

        public int RoundIndex;
        public int CardsInDeck;
        public int CardsInDiscardPile;

        public int PairComboSize;
        public int NumberOfRounds;

        public struct Player
        {
            public int Index;
            public int Score;
            public int Health;
            public int Shield;
            public int PairCombo;
            public MSG.Card[] Hand;
            public MSG.Card[] Board;
        }
    }
}
