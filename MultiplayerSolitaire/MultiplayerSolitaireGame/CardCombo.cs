namespace MSG
{
    public enum CardCombo
    {
        None,
        Pair,
        Chain,
        Flush,
        Royal,
    }

    public static class Combo
    {
        public static CardCombo Compute(Card[] cards, out byte cardFlags)
        {
            if (cards == null || cards.Length != 3)
            {
                throw new System.NotSupportedException();
            }

            cardFlags = 0;

            if (cards[0].Value == cards[1].Value && cards[0].IsValide())
            {
                cardFlags = 3;
                return CardCombo.Pair;
            }

            if (cards[0].Value == cards[2].Value && cards[0].IsValide())
            {
                cardFlags = 5;
                return CardCombo.Pair;
            }

            if (cards[1].Value == cards[2].Value && cards[1].IsValide())
            {
                cardFlags = 6;
                return CardCombo.Pair;
            }

            bool flush = true;
            Sigil sigil = cards[0].Sigil;
            short minValue = cards[0].Value;
            short maxValue = cards[0].Value;
            bool isThere2Or9 = cards[0].Value == 1 || cards[0].Value == 8;
            bool allValide = cards[0].IsValide();

            for (int index = 1; index < cards.Length; ++index)
            {
                if (!cards[index].IsValide())
                {
                    allValide = false;
                    break;
                }

                if (cards[index].Sigil != sigil)
                {
                    flush = false;
                }

                if (cards[index].Value > maxValue)
                {
                    maxValue = cards[index].Value;
                }

                if (cards[index].Value < minValue)
                {
                    minValue = cards[index].Value;
                }

                isThere2Or9 |= cards[index].Value == 1 || cards[index].Value == 8;
            }

            if (!allValide)
            {
                return CardCombo.None;
            }

            int delta = maxValue - minValue;
            bool chain = delta == 2;
            if (!chain)
            {
                chain = minValue == 0 && maxValue == 9 && isThere2Or9;
            }

            if (flush || chain)
            {
                cardFlags = 7;
                if (flush == chain)
                {
                    return CardCombo.Royal;
                }

                if (flush)
                {
                    return CardCombo.Flush;
                }

                return CardCombo.Chain;
            }
            
            return CardCombo.None;
        }
    }
}
