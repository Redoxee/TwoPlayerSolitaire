using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGWeb
{
    [System.Serializable]
    public class PlayerViewUpdate : JSONResponse
    {
        public override string MessageType => "PlayerViewUpdate";

        public bool IsNextRoundState = false;

        public MSG.GameStateID GameStateID;

        public Player CurrentPlayer;
        public Player OtherPlayer;

        public int PlayerTurn;
        public int RoundIndex;
        public int CardsInDeck;
        public int CardsInDiscardPile;

        public int PairComboSize;
        public int ScoreTarget;

        public struct Player
        {
            public int Index;
            public int Score;
            public int Health;
            public int PairCombo;
            public MSG.Card[] Hand;
            public MSG.Card[] Board;
            public int FaceIndex;
        }
    }
}
