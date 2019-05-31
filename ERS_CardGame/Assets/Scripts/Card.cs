using System.Collections;
using UnityEngine;
public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public Transform pilePosition, handPosition, currentPos;

    private float speed;

    public static string[] suits =
    {
        "CLUBS",
        "HEARTS",
        "SPADES",
        "DIAMONDS"
    };
    
    public void CardInit(int v, string s)
    {
        value = v;
        suit = s;
        currentPos = pilePosition;
    }
    public bool IsFaceCard()
    {
        if (value > 10) return true;
        return false;
    }
    public void SetPlayerPos(Transform t)
    {
        handPosition = t;
    }
    public void SetPosition(Transform t)
    {
        currentPos = t;
        Transform oldPos = pilePosition;
        if (currentPos == pilePosition) oldPos = handPosition;
        Move()
    }

}
