namespace MSG
{
    using AMG;
    using System;
    using System.Text;

    public class Deck : AMG.ISerializable
    {
        public Card[] Cards;
        public int NumberOfCards;
        public int TotalNumberOfCards;

        public Deck()
        {
            this.Refill();
        }

        public void Refill()
        {
            this.NumberOfCards = 0;

            Array sigils = Enum.GetValues(typeof(Sigil));
            this.TotalNumberOfCards = Card.ValueNames.Length * sigils.Length;
            this.Cards = new Card[this.TotalNumberOfCards];

            for (int sigilIndex = 0; sigilIndex < sigils.Length; ++sigilIndex)
            {
                for (short cardValueIndex = 0; cardValueIndex < Card.ValueNames.Length; ++cardValueIndex)
                {
                    ref Card card = ref this.Cards[this.NumberOfCards++];
                    card.Sigil = (Sigil)sigilIndex;
                    card.Value = cardValueIndex;
                }
            }
        }

        public void Shuffle()
        {
            Random random = new ();

            for (int index = 0; index < this.NumberOfCards - 1; ++index)
            {
                int newIndex = random.Next(index, this.NumberOfCards);
                if (index == newIndex)
                {
                    continue;
                }

                Card temp = this.Cards[index];
                this.Cards[index] = this.Cards[newIndex];
                this.Cards[newIndex] = temp;
            }
        }

        public Card PickCard()
        {
            if (this.NumberOfCards < 1)
            {
                throw new ArgumentOutOfRangeException("Picked one toomany cards");
            }

            return this.Cards[--this.NumberOfCards];
        }

        public void AddCardUnder(Card card)
        {
            this.Cards[this.NumberOfCards++] = card;
        }

        public override string ToString()
        {
            StringBuilder builder = new ();
            builder.Append('[');
            for (int index = 0; index < this.NumberOfCards; ++index)
            {
                builder.Append(this.Cards[index].ToString());
                if (index < this.NumberOfCards - 1)
                {
                    builder.Append(',');
                }
            }

            builder.Append($"] ({this.NumberOfCards})");
            return builder.ToString();
        }

        public void Serialize(Serializer serializer)
        {
            this.Cards = serializer.Serialize("Cards", this.Cards);
            this.NumberOfCards = serializer.Serialize("NumberOfCards", this.NumberOfCards);
            this.TotalNumberOfCards = serializer.Serialize("TotalNumberOfCards", this.TotalNumberOfCards);
        }
    }
}
