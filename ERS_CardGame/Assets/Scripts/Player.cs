using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Queue<Card> hand= new Queue<Card>();
    public bool isTurn;
    public float slapTime;
    public bool isFirstToSlap;
    // Update is called once per frame
    void Start()
    {
        slapTime = 0;
       // isTurn = 
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { Slap(); }
        if (hand.Count >  0 && isTurn && Input.GetKeyDown(KeyCode.Mouse0)) {
            Card temp = hand.Dequeue();
            /*flip (where sets timePlayed) and move temp*/
            Pile.AddToTop(temp);
            isTurn = false; }
    }

    public void Slap() {
        slapTime = Time.time; 
        if (Pile.ValidSlap() && isFirstToSlap) Pile.PickUp(hand);
    }
    public void AddCard(Card a) { hand.Enqueue(a); }
    public void AddToHand() { Pile.PickUp(hand); }
    public int HandSize() { return hand.Count; }
}
