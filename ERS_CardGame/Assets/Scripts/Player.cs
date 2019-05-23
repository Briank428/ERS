using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Queue<Card> hand= new Queue<Card>();
    public bool isTurn;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Slap();
        if (hand.Count != 0 && isTurn && Input.GetKeyDown(KeyCode.Mouse0)) { Pile.AddToTop(hand.Dequeue()); isTurn = false; }
    }

    public void Slap() {
        if (Pile.ValidSlap()) Pile.PickUp(hand);
    }
    public void AddCard(Card a) { hand.Enqueue(a); }
    public void AddToHand() { Pile.PickUp(hand); }
    public int HandSize() { return hand.Count; }
}
