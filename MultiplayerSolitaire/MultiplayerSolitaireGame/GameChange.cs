namespace MSG
{
    public struct GameChange
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameChangeType ChangeType;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameStateID GameStateID;
        public int PlayerIndex;
        public int IndexInHand;
        public int IndexOnBoard;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardCombo CardCombo;
        public int UsedCards;
        public Card Card;
        public PlayerProperties PlayerProperty;
        public int NewValue;

        public enum GameChangeType
        {
            GameStateChange,
            PlayedCard,
            PickedCard,
            PlayerPropertyChanged,
            PlayerCombo,
            NextPlayer,
            PlayerWon,
        }

        public enum PlayerProperties
        { 
            Health,
            Shield,
            PairBullet,
        }
    }
}
