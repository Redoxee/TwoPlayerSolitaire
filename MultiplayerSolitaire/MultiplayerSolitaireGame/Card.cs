namespace MultiplayerSolitaireGame
{
    [System.Serializable]
    public struct Card
    {
        public const short None = -1;

        public short Value;
        public Sigil Sigil;

        public override string ToString()
        {
            if (this.Value == -1)
            {
                return "[   of        ]";
            }

            string cardValue = this.Value < Card.ValueNames.Length ? Card.ValueNames[this.Value] : this.Value.ToString();
            return $"[{cardValue, -2} of {this.Sigil,-7}]";
        }

        public bool IsValide()
        {
            return this.Value != Card.None;
        }

        public static readonly string[] ValueNames = {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
        };
    }

    public enum Sigil
    {
        Spade,
        Club,
        Heart,
        Diamond,
    }
}
