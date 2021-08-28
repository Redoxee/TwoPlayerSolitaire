using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerSolitaireGame
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

            if (cards[0].Value == cards[1].Value)
            {
                cardFlags = 0x3;
                return CardCombo.Pair;
            }

            if (cards[0].Value == cards[2].Value)
            {
                cardFlags = 0x5;
                return CardCombo.Pair;
            }

            if (cards[1].Value == cards[2].Value)
            {
                cardFlags = 0x6;
                return CardCombo.Pair;
            }

            bool flush = true;
            bool chain = true;

            Sigil sigil = cards[0].Sigil;
            for (int index = 1; index < cards.Length; ++index)
            {
                if (cards[index].Sigil != sigil)
                {
                    flush = false;
                }
            }

            return CardCombo.None;
        }
    }
}
