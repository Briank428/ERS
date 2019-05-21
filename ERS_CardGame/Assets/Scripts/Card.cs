public class Card
{
    public int value;
    public string suit;

    public static string[] suits =
    {
        "CLUBS",
        "HEARTS",
        "SPADES",
        "DIAMONDS"
    };
    
    public Card(int v, string s)
    {
        value = v;
        suit = s;
    }
    public bool IsFaceCard()
    {
        if (value > 10) return true;
        return false;
    }
}
