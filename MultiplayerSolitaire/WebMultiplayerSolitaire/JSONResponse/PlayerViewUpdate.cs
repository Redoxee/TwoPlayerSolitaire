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

        public MultiplayerSolitaireGame.GameStateID GameStateID;
        public int PlayerIndex;

        public MultiplayerSolitaireGame.Card[] Hand;
        public MultiplayerSolitaireGame.Card[] Board;

        public int Score;
        public int Health;
        public int Shield;
        public int PairBullets;

        public Player OtherPlayer;
        public int CurrentPlayer;
        public int Round;
        public MultiplayerSolitaireGame.Card[] DiscardPile;

        public struct Player
        {
            public int Index;
            public int Score;
            public int Health;
            public int Shield;
            public int PairBullets;
            public MultiplayerSolitaireGame.Card[] Board;
        }
    }
}
