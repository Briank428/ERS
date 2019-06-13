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
        back = GameManager.instance.back;
        currentPos = null;
        currentSprite = GetComponent<SpriteRenderer>();
        currentSprite.sprite = back;
    }

    public void Flip()
    {
        if (currentSprite.sprite == back) currentSprite.sprite = front;
        if (currentSprite.sprite == front) currentSprite.sprite = back;

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

    public void Move()
    {

        transform.position = currentPos.position;
    }
}
