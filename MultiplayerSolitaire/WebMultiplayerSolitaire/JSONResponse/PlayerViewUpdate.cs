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
        public MultiplayerSolitaireGame.Card TrumpCard;
        public MultiplayerSolitaireGame.Card[] Hand;
        [Newtonsoft.Json.JsonProperty("BetFailures", ItemConverterType = typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MultiplayerSolitaireGame.Failures[] BetFailures;
        public int Bet;
        public int PlayedCard;
        public int Score;
        public Player[] OtherPlayers;
        public int CurrentPlayer;

        public struct Player
        {
            public int Bet;
            public int NumberOfCards;
            public int CurrentScore;
        }
    }
}
