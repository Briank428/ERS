using System.Collections;
using UnityEngine;
public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public Transform pilePosition, handPosition, currentPos, oldPos;
    public static Sprite back;
    private float speed;
    private Rigidbody2D rb;

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
        rb = GetComponent<Rigidbody2D>();
        currentPos = pilePosition;
        oldPos = null;
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
        oldPos = currentPos;
        currentPos = t;
        Move();
    }

    public IEnumerator Move()
    {
        yield return null;
    }
}
