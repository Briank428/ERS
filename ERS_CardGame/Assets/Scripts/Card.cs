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
        currentPos = null;
        currentSprite = GetComponent<SpriteRenderer>();
        currentSprite.sprite = back;
        speed = 1f;
    }

    public void Flip()
    {
        if (currentSprite.sprite == back) currentSprite.sprite = front;
        else currentSprite.sprite = back;

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
    public void MoveToPile()
    {
        oldPos = currentPos;
        currentPos = pilePosition;
        Move();
    }
    public void Move()
    {
        transform.position = Vector3.Lerp(transform.position, currentPos.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentPos.rotation, speed * Time.deltaTime);
    }
}
