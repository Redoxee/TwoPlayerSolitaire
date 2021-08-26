namespace MultiplayerSolitaireGame
{
    public struct GameChange
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameChangeType ChangeType;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameStateID GameState;
        public int PlayerIndex;
        public PlayedCard PlayedCard;

        public enum GameChangeType
        {
            GameStateChange,
            PlayedCard,
        }
    }
}
