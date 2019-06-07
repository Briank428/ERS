using System.Collections;
using UnityEngine;
public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public Transform handPosition, currentPos, oldPos;
    public static Transform pilePosition;
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
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPos = null;
    }
    public bool IsFaceCard()
    {
        if (value > 10) return true;
        return false;
    }
    public void SetPlayerPos(Transform t)
    {
        handPosition = t;
        SetPosition(handPosition);
    }
    void SetPilePos(Transform t)
    {
        pilePosition = t;
        SetPosition(t);
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
