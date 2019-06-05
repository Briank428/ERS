using System.Collections.Generic;

public class Pile
{
    private static List<Card> pile = new List<Card>();
    private static Card topCard;

    public static void AddToTop(Card a) { 
        pile.Add(a);
        topCard = a;
    }
    public static void AddToBottom(Card a)
    {
        pile.Insert(0, a);
    }
    public static void PickUp(Queue<Card> player)
    {
        while (pile.Count != 0)
        {
            player.Enqueue(pile[0]);
            pile.RemoveAt(0);
        }
    }
    public static bool ValidSlap()
    {
        if (pile.Count < 2) return false;
        if (pile[pile.Count - 1].value == pile[pile.Count - 2].value || pile[pile.Count - 1].value == pile[pile.Count - 3].value) return true;
        return false;
    }
    public static Card GetTopCard() { return topCard; }
}