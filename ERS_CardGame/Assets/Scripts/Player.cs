using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Queue<Card> hand;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Slap();
        if (Input.GetKeyDown(KeyCode.Mouse0)) Pile.AddToTop(hand.Dequeue()); 

    }

    public void Slap() { }
}
