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
    private Rigidbody2D rb;
    private SpriteRenderer currentSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        front = GetComponent<SpriteRenderer>().sprite;
        oldPos = transform;
        currentPos = transform;
        currentSprite = GetComponent<SpriteRenderer>();
        currentSprite.sprite = back;
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
        StartCoroutine(Move(oldPos,currentPos,.5f));
    }

    IEnumerator Move(Transform source, Transform target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source.position, target.position, (Time.time - startTime) / overTime);
            transform.rotation = Quaternion.Lerp(source.rotation, target.rotation, (Time.time - startTime) / overTime);

            yield return null;
        }
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
