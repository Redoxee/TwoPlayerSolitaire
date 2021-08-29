namespace MultiplayerSolitaireGame
{
    public struct GameChange
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameChangeType ChangeType;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public GameStateID GameState;
        public int PlayerIndex;
        public int IndexInHand;
        public int IndexOnBoard;
        public Card Card;
        public PlayerProperties PlayerProperty;
        public int NewValue;

        public enum GameChangeType
        {
            GameStateChange,
            PlayedCard,
            PickedCard,
            PlayerPropertyChanged,
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
