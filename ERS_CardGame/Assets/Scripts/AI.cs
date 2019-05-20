using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Queue<Card> hand;
    public bool empty;
 
    void Start()
    {
        hand = new Queue<Card>(); empty = false;
    }

    void Update()
    {
        if (hand.Count == 0) empty = true;
        else empty = false;
    }

    public double CheckSlap()
    {
        if (Pile.ValidSlap()) return Random.Range(0f, 3f);
        return -1.0;
    }

    public void PlayCard()
    {
        Pile.AddToTop(hand.Dequeue());
    }

    public void SetHand(Queue<Card> cards) { hand = cards; }
    public void AddCard(Card a) { hand.Enqueue(a); }
}
