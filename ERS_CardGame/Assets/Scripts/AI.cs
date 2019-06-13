﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Queue<Card> hand = new Queue<Card>();
    public bool empty;
    public float SlapTime()
    {
        if (Pile.ValidSlap()) return Random.Range(0.5f, 2f);
        return -1.0f;
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
    public void AddToHand() { Pile.PickUp(hand, this.gameObject.transform);  }
    public int HandSize() { return hand.Count; }
}