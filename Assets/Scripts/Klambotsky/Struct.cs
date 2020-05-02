public static class Struct
{
    public enum Gender
    {
        Undefined, Woman, Man, Granny
    }

    public static Gender[] genders = { Gender.Woman, Gender.Man, Gender.Granny };

    public enum Media
    {
        Undefined, TV, Newspaper, Youtube,
    }

    public static Media[] medias = { Media.TV, Media.Newspaper, Media.Youtube };

    public class Event
    {
        public string name;
        Gender gender;
        float[] multiplier = new float[3];
        public int probability;

        public Event(Gender gender, float increase, float decrease, int probability, string name)
        {
            this.gender = gender;
            for (int i = 0; i < 3; i++)
            {
                if (gender == genders[i])
                {
                    multiplier[i] = increase;
                }
                else
                {
                    multiplier[i] = decrease;
                }
            }
            this.name = name;
            this.probability = probability;
        }

        public float GetWomanMultiplier()
        {
            return multiplier[0];
        }

        public float GetManMultiplier()
        {
            return multiplier[1];
        }

        public float GetGrannyMultiplier()
        {
            return multiplier[2];
        }
    }



    public class CardType
    {
        public enum Type
        {
            Undefined, News, Hater, Coin, Protect, Glitch, Trap
        }

        public Type type;
        public int probability;

        public CardType(Type type, int probability)
        {
            this.type = type;
            this.probability = probability;
        }
    }
    // Первый элемент пустой с вероятностью 0
    // дальше сумма всех вероятностей должна быть = 100, иначе будет эксепшн
    //В эвентах вероятность это 4й параметр (поставлены по 5%) 
    //В карточках это второй параметр
    public static Event[] events =
    {
        new Event(Gender.Undefined, 0, 0, 0, "Blank"),
        new Event(Gender.Granny, 2, 0.5f, 5, "Granny invades"),
        new Event(Gender.Woman, 2, 0.5f, 5, "Woman invades"),
        new Event(Gender.Man, 2, 0.5f, 5, "Man invades"),
        new Event(Gender.Undefined, 1, 1, 85, "No event")
    };

    public static CardType[] cardTypes =
    {
        new CardType(CardType.Type.Undefined, 0),
        new CardType(CardType.Type.Hater, 10),
        new CardType(CardType.Type.Coin, 10),
        new CardType(CardType.Type.News, 70),
        new CardType(CardType.Type.Protect, 5),
        new CardType(CardType.Type.Glitch, 5),
    };
}
