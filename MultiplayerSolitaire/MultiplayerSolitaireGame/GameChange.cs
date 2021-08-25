namespace MultiplayerSolitaireGame
{
    public struct GameChange
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameChangeType ChangeType;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameStateID GameState;
        public int PlayerIndex;
        public int BetValue;
        public PlayedCard PlayedCard;

        public enum GameChangeType
        {
            GameStateChange,
            PlayerBet,
            PlayedCard,
            NextPlayer,
        }
    }
}
