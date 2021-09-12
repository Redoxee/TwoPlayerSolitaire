using MSG;

namespace ConsoleCardGame
{
    class Program
    {
        static void Main(string[] _)
        {
            // Testing combo code
            {
                /*
                Card[] cards = new Card[3];
                Deck deck = new Deck();
                for (int i = 0; i < 0; ++i)
                {
                    deck.Refill();
                    deck.Shuffle();

                    for (int ci = 0; ci < 3; ++ci)
                    {
                        cards[ci] = deck.PickCard();
                    }

                    byte usedFlags;
                    CardCombo combo = Combo.Compute(cards, out usedFlags);
                    System.Console.WriteLine($"{cards[0]}{cards[1]}{cards[2]} : {combo} | {usedFlags}");
                }
                */
            }

            GameChangePool gameChanges = new GameChangePool();
            GameManager gameManager = new GameManager(GameManager.GameParameters.Default(), gameChanges);
            bool quit = false;
            do
            {

                System.Console.WriteLine("-------------------------------\n" + gameManager.GetDebugString());

                string line = System.Console.ReadLine();
                string[] splitted = line.Split(' ');

                if (splitted.Length == 0)
                {
                    continue;
                }

                GameOrder order = TryParseGameOrder(splitted);
                if (order != null)
                {
                    Failures failure = gameManager.ProcessOrder(order, gameChanges);
                    System.Console.WriteLine(failure.ToString());
                }

                if (splitted[0].Trim().ToLower() == "quit")
                {
                    quit = true;
                }

            } while (!quit);
        }

        private static GameOrder TryParseGameOrder(string[] input)
        {
            if (input.Length < 4)
            {
                return null;
            }

            GameOrder order = null;

            string stringOrder = input[2].Trim().ToLower();

            if (stringOrder == "play")
            {
                if (!int.TryParse(input[1], out int playerIndex))
                {
                    return null;
                }

                if (!int.TryParse(input[3], out int cardIndex))
                {
                    return null;
                }

                if (!int.TryParse(input[5], out int boardIndex))
                {
                    return null;
                }

                order = new PlayCardOrder()
                {
                    PlayerIndex = playerIndex,
                    CardIndex = cardIndex,
                    BoardIndex = boardIndex,
                };
            }


            return order;
        }
    }
}
