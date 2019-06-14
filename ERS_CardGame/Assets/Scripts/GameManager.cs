using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region vars
    public Button one;
    public Button two;
    public Button three;
    public Text playerText;
    public Text text;
    public AI prefabAI;
    public float timePlayed;
    [SerializeField]
    public List<Card> deck;
    public GameObject cardTable;

    private int faceCardIndex;
    private int faceCardValue;
    private bool basecase;

    private int numOpponents;
    private int numPlayers;
    private List<AI> players;
    private Player player;
    private bool isOver;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        one.onClick.AddListener(StartGameOne);
        two.onClick.AddListener(StartGameTwo);
        three.onClick.AddListener(StartGameThree);
        players = new List<AI>();
        player = GameObject.Find("Player").GetComponent<Player>();
        isOver = false;
        Card.pilePosition = GameObject.Find("Pile").transform;
        cardTable.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void Deal() {
        Helper.Shuffle(deck);
        int index = 0;
        while(deck.Count > 0)
        {
            {
                Card temp = deck[0];
                if (index != numOpponents) { players[index].AddCard(temp); temp.SetPlayerPos(players[index].transform); index++; }
                else { player.AddCard(temp); temp.SetPlayerPos(player.transform); index = 0; }
                deck.RemoveAt(0);
            }
        }
        StartCoroutine(Game());
    }

    public IEnumerator Game()
    {
        Debug.Log("Game started");
        while (!isOver)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                if (i == numPlayers - 1) {
                    player.isTurn = true; Debug.Log("Player turn");
                    while (!Input.GetKeyDown(KeyCode.Mouse0))
                        yield return null;
                }
                else { yield return new WaitForSeconds(1.5f); players[i].PlayCard(); }
                Card temp = Pile.GetTopCard();


                /*if (Pile.ValidSlap()) { 
                    //find minimum time of AI
                    float minAI = players[0].SlapTime(); int lowest = 0;
                    for (int j = 1; j < numPlayers-1; j++) {
                        float tempSlap = players[j].SlapTime();
                        if (minAI > tempSlap) { minAI = tempSlap; lowest = j; }
                    }
                    //Compare to player (wait until have played?)
                    if (minAI < player.slapTime - timePlayed) { players[lowest].AddToHand();  text.text = "Player " + 1 + lowest + " slapped first!"; i = lowest - 1; }
                    else { player.AddToHand();  text.text = "You slapped first!"; i = numPlayers - 2; }

                    continue;
                }*/
                if (temp.IsFaceCard()) { faceCardIndex = i; faceCardValue = temp.value;  yield return StartCoroutine("FaceCard"); i = faceCardIndex; }
            }   
        }   
    }   

    public IEnumerator FaceCard() {
        basecase = false;
        int index;
        if (faceCardIndex == numPlayers - 1)  index = 0; 
        else index = faceCardIndex+1; Debug.Log("Player " + faceCardIndex + " played Face Card; player " + index + " will respond");
        if (faceCardValue == 11) {
            if (index == numPlayers - 1)
            {
                player.isTurn = true;
                while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); }
                else basecase = true;
            }
            else {
                yield return new WaitForSeconds(1.5f);  players[index].PlayCard();
                if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); }
                else basecase = true;
            }
        }
        else if (faceCardValue == 12)
        {
            if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 2)
                {
                    yield return new WaitForSeconds(0.5f);  player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                    Debug.Log("Iterated through " + count + " time/s");
                }
                if (count == 2) basecase = true;
            }
            else
            {
                int count = 0;
                while (count<2)
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 2) basecase = true;
            }
        }
        else if (faceCardValue == 13)
        {
            if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 3)
                {
                    yield return new WaitForSeconds(0.5f); player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 3) basecase = true;
            }
            else
            {
                int count = 0;
                while (count < 3)
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 3) basecase = true;
            }
        }
        else if (faceCardValue == 1)
        {
            if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 4)
                {
                    yield return new WaitForSeconds(0.5f); player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 4) basecase = true;
            }
            else
            {
                int count = 0;
                while (count < 4)
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 4) basecase = true;
            }
        }
        if (basecase && index == 0)
        {
            yield return new WaitForSeconds(1f);
            faceCardIndex = numPlayers - 1;
            player.AddToHand();
            faceCardIndex--;
            basecase = false;
        }
        else if (basecase && index == 1 )
        {
            yield return new WaitForSeconds(1f);
            faceCardIndex = index -1 ;
            players[faceCardIndex].AddToHand();
            faceCardIndex = numPlayers - 1;
            basecase = false;
        }
        else if (basecase)
        {
            yield return new WaitForSeconds(1f);
            faceCardIndex = index - 1;
            players[faceCardIndex].AddToHand();
            faceCardIndex--;
            basecase = false;
        }
            
    }

    #region start
    public void StartGameOne() {
        numOpponents = 1; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        cardTable.SetActive(true);
        players.Add(GameObject.Find("Two Person").GetComponent<AI>());
        Destroy(GameObject.Find("Three Person"));
        Destroy(GameObject.Find("Four Person"));
        Deal();
    }
    public void StartGameTwo() {
        numOpponents = 2; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        cardTable.SetActive(true);
        Destroy(GameObject.Find("Two Person"));
        Destroy(GameObject.Find("Four Person"));
        players.Add(GameObject.Find("3 Players - 1").GetComponent<AI>());
        players.Add(GameObject.Find("3 Players - 2").GetComponent<AI>());
        Deal();
    }
    public void StartGameThree() {
        numOpponents = 3; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        cardTable.SetActive(true);
        Destroy(GameObject.Find("Three Person"));
        players.Add(GameObject.Find("4 Players - 3").GetComponent<AI>());
        players.Add(GameObject.Find("Two Person").GetComponent<AI>());
        players.Add(GameObject.Find("4 Players - 1").GetComponent<AI>());
        Deal();
    }
    #endregion
}
