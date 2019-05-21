using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Queue<Card> hand = new Queue<Card>();
    public bool empty;
    public double CheckSlap()
    {
        if (Pile.ValidSlap()) return Random.Range(0.5f, 2f);
        return -1.0;
    }

    public void PlayCard()
    {
        Pile.AddToTop(hand.Dequeue());
    }
    public void IsEmpty()
    {
        if (hand.Count == 0) empty = true;
        else empty = false;
    }
    public void AddCard(Card a) { hand.Enqueue(a); }
    public void AddToHand() { Pile.PickUp(hand);  }
    public int HandSize() { return hand.Count; }
}
