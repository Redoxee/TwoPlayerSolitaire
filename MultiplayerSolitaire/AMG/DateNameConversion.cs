namespace AMG
{
    public static class DateNameConversion
    {
        private static string[] monthNames = new string[]
        {
            "Big",
            "Brave",
            "Fast",
            "Freaking",
            "Magic",
            "Small",
            "Spinning",
            "Static",
            "Strange",
            "Strong",
            "Super",
            "Unsettling",
            "Weird",
        };

        private static string[] hourNames = new string[]
            {
                "Atomic",
                "Bright",
                "Cheesy",
                "Creamy",
                "Devious",
                "Dropping",
                "Extra",
                "Grim",
                "Hyper",
                "Integral",
                "Joyous",
                "Knowing",
                "Loving",
                "Melancholic",
                "Melted",
                "Notorious",
                "Oblong",
                "Optimistic",
                "Rambling",
                "Stubborn",
                "Trampling",
                "Upmost",
                "Vicious",
                "Wild",
                "Youthful",
            };

        private static string[] minuteNames = new string[]
            {
                "Abyss",
                "Adventurer",
                "Ant",
                "Bass",
                "Battle",
                "Bear",
                "Bird",
                "Cake",
                "Calvin",
                "Car",
                "Catcher",
                "Checker",
                "Critters",
                "Cyclone",
                "Deer",
                "Elf",
                "Fighter",
                "Fire",
                "French",
                "Game",
                "Gauthier",
                "Goal",
                "Goblin",
                "Hex",
                "Hiker",
                "Hotdog",
                "Jack",
                "Jon",
                "Kiwi",
                "Lamp",
                "Language",
                "Letter",
                "Lion",
                "Liver",
                "Lobster",
                "Mole",
                "Monkey",
                "Moon",
                "Movie",
                "Nose",
                "Olive",
                "Orb",
                "Orc",
                "Order",
                "Panda",
                "Plane",
                "Planet",
                "Priest",
                "Ring",
                "River",
                "Sailor",
                "Shrimp",
                "Singer",
                "Star",
                "Stout",
                "Streamer",
                "Teacher",
                "Time",
                "Toast",
                "Trombone",
                "Vines",
                "Wolf",
                "Zune",
            };

        private static System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        public static string NameDate(System.DateTime date)
        {
            DateNameConversion.stringBuilder.Clear();
            DateNameConversion.stringBuilder.Append(date.Year);
            if (date.Month < DateNameConversion.monthNames.Length)
            {
                DateNameConversion.stringBuilder.Append(DateNameConversion.monthNames[date.Month]);
            }
            else
            {
                DateNameConversion.stringBuilder.Append(date.Month);
            }

            if (date.Hour < DateNameConversion.hourNames.Length)
            {
                DateNameConversion.stringBuilder.Append(DateNameConversion.hourNames[date.Hour]);
            }
            else
            {
                DateNameConversion.stringBuilder.Append(date.Hour);
            }

            if (date.Minute < DateNameConversion.minuteNames.Length)
            {
                DateNameConversion.stringBuilder.Append(DateNameConversion.minuteNames[date.Minute]);
            }
            else
            {
                DateNameConversion.stringBuilder.Append(date.Minute);
            }

            return DateNameConversion.stringBuilder.ToString();
        }
    }
}
