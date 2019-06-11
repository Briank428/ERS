using System.Collections;
using UnityEngine;
public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public Transform handPosition, currentPos, oldPos;
    public static Transform pilePosition;
    [SerializeField]
    public Sprite back, front;
    private float speed;
    private Rigidbody2D rb;
    private SpriteRenderer currentSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        front = GetComponent<SpriteRenderer>().sprite;  
        back = GameManager.back;
        currentPos = null;
        currentSprite = GetComponent<SpriteRenderer>();
        currentSprite.sprite = back;
    }

    public void Flip()
    {
        //flip 90, switch sprite, flip back
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
