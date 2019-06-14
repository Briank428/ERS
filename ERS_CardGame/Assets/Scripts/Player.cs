using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Queue<Card> hand= new Queue<Card>();
    public bool isTurn;
    public float slapTime;
    public bool isFirstToSlap;

    private bool empty;
    // Update is called once per frame
    void Start()
    {
        slapTime = 0f;
        isTurn = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Slap();
        if (hand.Count >  0 && isTurn && Input.GetKeyDown(KeyCode.Mouse0)) {
            Pile.AddToTop(hand.Dequeue());
            isTurn = false;
        }
    }
    public void Slap() {
        if (slapTime < GameManager.timePlayed) slapTime = Time.time;
    }
    public void AddCard(Card a) { hand.Enqueue(a); }
    public void AddToHand() { Pile.PickUp(hand, this.gameObject.transform); }
    public int HandSize() { return hand.Count; }
    public bool GetEmpty() { if (hand.Count == 0) empty = true;else empty = false; return empty; }
}
