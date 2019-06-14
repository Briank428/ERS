using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region vars
    public Button one;
    public Button two;
    public Button three;
    public Text playerText;
    public Text text;
    public Text left4;
    public Text right4;
    public Text top;
    public Text left3;
    public Text right3;
    public Text bottom;

    public AI prefabAI;
    public static float timePlayed;
    [SerializeField]
    public List<Card> deck;
    public GameObject cardTable;

    private int faceCardIndex;
    private int faceCardValue;
    private bool basecase;
    private bool SlapInterrupt;
    private int slapIndex;

    private int lastFace;
    private int numOpponents;
    private int numPlayers;
    private List<AI> players;
    private Player player;
    private bool isOver;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SlapInterrupt = false;
        left3.gameObject.SetActive(false); right3.gameObject.SetActive(false);
        left4.gameObject.SetActive(false); right4.gameObject.SetActive(false);
        top.gameObject.SetActive(false); bottom.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
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
    private void Update()
    {
        if (bottom.gameObject.activeSelf) bottom.text = "YOU\n" + player.HandSize() + " CARDS";
        if (top.gameObject.activeSelf && numPlayers == 2) top.text = "PLAYER 1\n" + players[0].HandSize() + " CARDS";
        else if (top.gameObject.activeSelf) top.text = "PLAYER 2\n" + players[1].HandSize() + " CARDS";
        if (left3.gameObject.activeSelf) { left3.text = "PLAYER 1\n" + players[0].HandSize() + " CARDS"; right3.text = "PLAYER 2\n" + players[1].HandSize() + " CARDS"; }
        if (left4.gameObject.activeSelf) { left4.text = "PLAYER 1\n" + players[0].HandSize() + " CARDS"; right4.text = "PLAYER 3\n" + players[2].HandSize() + " CARDS"; }
    }
    void CheckOver() { int count = 0;
        if (player.GetEmpty()) count++;
        foreach (AI ai in players)
            if (ai.GetEmpty()) count++;
        if (count == numPlayers - 1) isOver = true; }
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
        text.text = "MOUSE: PLAY CARD\nSPACEBAR: SLAP";
        yield return new WaitForSeconds(3f);
        while (!isOver)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                text.text = "";
                CheckOver();
                if (isOver) break;
                if (i == numPlayers - 1 && !player.GetEmpty())
                {
                    player.isTurn = true; 
                    while (!Input.GetKeyDown(KeyCode.Mouse0))
                        yield return null;
                }
                else if (i < numPlayers -1 && !players[i].GetEmpty()) { yield return new WaitForSeconds(1.5f); players[i].PlayCard(); }
                else continue;
                Card temp = Pile.GetTopCard();

                if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); i = slapIndex; Debug.Log("" + i); }
                else if (temp.IsFaceCard()) { faceCardIndex = i; faceCardValue = temp.value; lastFace = faceCardIndex;
                    yield return StartCoroutine("FaceCard"); if (SlapInterrupt) { i = slapIndex; SlapInterrupt = false; } else i = faceCardIndex; }
            }
        }
        if (!player.GetEmpty()) text.text = "YOU WIN!";
        else text.text = "YOU LOSE";
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Title");
    }   
    public IEnumerator Slap()
    {
        
        Debug.Log("Card played " + timePlayed);
        //find minimum time of AI
        float minAI = players[0].SlapTime(); int lowest = 0;
        for (int j = 1; j < numPlayers - 1; j++)
        {
            float tempSlap = players[j].SlapTime();
            if (minAI > tempSlap) { minAI = tempSlap; lowest = j; }
        }
        Debug.Log("Player " + lowest + ": " + minAI + " sec");
        while (Time.time - timePlayed <= minAI && !Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        Debug.Log("Your time: " + (player.slapTime- timePlayed));
        if (player.slapTime - timePlayed < 0 || minAI < player.slapTime - timePlayed)
        {
            players[lowest].AddToHand();
            text.text = "Player " + (1 + lowest) + "\nslapped first!"; yield return new WaitForSeconds(1.75f);
            if (lowest == 0) slapIndex = numPlayers - 1; else slapIndex = lowest -1;
        }
        else
        { 
            player.AddToHand(); text.text = "You slapped first!"; yield return new WaitForSeconds(.5f); slapIndex = numPlayers-2; 
        }
        Debug.Log("SlapIndex: " + slapIndex);
    }
    public IEnumerator FaceCard() {
        basecase = false;
        int index;
        if (faceCardIndex == numPlayers - 1)  index = 0; 
        else index = faceCardIndex+1; 
        if (faceCardValue == 11) {
           text.text = "JACK: ONE TRY";
            if (index == numPlayers - 1 && player.GetEmpty()) { faceCardIndex = index;  yield return StartCoroutine("FaceCard"); }
            else if (index == numPlayers - 1)
            {
                
                player.isTurn = true;
                while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true;  yield break; }
                if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex;  yield return StartCoroutine("FaceCard"); }
                else basecase = true;
            }
            else if (players[index].GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else {
                yield return new WaitForSeconds(1.5f);  players[index].PlayCard();
                if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); }
                else basecase = true;
            }
        }
        else if (faceCardValue == 12)
        {
            text.text = "QUEEN: TWO TRIES";
            if (index == numPlayers - 1 && player.GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 2 && !player.GetEmpty())
                {
                    yield return new WaitForSeconds(0.5f); player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 2 || player.GetEmpty()) basecase = true;
            }
            else if (players[index].GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else 
            {
                int count = 0;
                while (count <2 && !players[index].GetEmpty())
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 2 || players[index].GetEmpty()) basecase = true;
            }
        }
        else if (faceCardValue == 13)
        {
            text.text = "KING: THREE TRIES";
            if (index == numPlayers - 1 && player.GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 3 && !player.GetEmpty())
                {
                    yield return new WaitForSeconds(0.5f); player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true;  yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 3 || player.GetEmpty()) basecase = true;
            }
            else if (players[index].GetEmpty()) { faceCardIndex = index;  yield return StartCoroutine("FaceCard"); }
            else
            {
                int count = 0;
                while (count < 3 && !players[index].GetEmpty())
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 3||players[index].GetEmpty()) basecase = true;
            }
        }
        else if (faceCardValue == 1)
        {
            text.text = "ACE: FOUR TRIES";
            if (index == numPlayers - 1 && player.GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else if (index == numPlayers - 1)
            {
                int count = 0;
                while (count < 4 && !player.GetEmpty())
                {
                    yield return new WaitForSeconds(0.5f); player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 4 || player.GetEmpty()) basecase = true;
            }
            else if (players[index].GetEmpty()) { faceCardIndex = index; yield return StartCoroutine("FaceCard"); }
            else
            {
                int count = 0;
                while (count < 4 && !players[index].GetEmpty())
                {
                    yield return new WaitForSeconds(1.5f); players[index].PlayCard();
                    if (Pile.ValidSlap()) { yield return StartCoroutine("Slap"); SlapInterrupt = true; yield break; }
                    if (Pile.GetTopCard().IsFaceCard()) { faceCardIndex = index; faceCardValue = Pile.GetTopCard().value; lastFace = faceCardIndex; yield return StartCoroutine("FaceCard"); break; }
                    count++;
                }
                if (count == 4 || players[index].GetEmpty()) basecase = true;
            }
        }
        #region basecase
        if (basecase)
        {
            Debug.Log("Last person to Add: " + index);
            yield return new WaitForSeconds(1f);
            faceCardIndex = lastFace;
            Debug.Log("Player " + faceCardIndex + " will pick up");
            if (faceCardIndex == numPlayers - 1) player.AddToHand();
            else players[faceCardIndex].AddToHand();
            if (faceCardIndex == 0) faceCardIndex = numPlayers - 1; else faceCardIndex--;
            basecase = false;
        }
        #endregion

    }

    #region start
    public void StartGameOne() {
        numOpponents = 1; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        bottom.gameObject.SetActive(true); bottom.text = "YOU";
        top.gameObject.SetActive(true); top.text = "PLAYER 1";

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
        bottom.gameObject.SetActive(true); bottom.text = "YOU";
        left3.gameObject.SetActive(true); left3.text = "PLAYER 1";
        right3.gameObject.SetActive(true); right3.text = "PLAYER 2";
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
        bottom.gameObject.SetActive(true); bottom.text = "YOU";
        left4.gameObject.SetActive(true); left4.text = "PLAYER 1";
        right4.gameObject.SetActive(true); right4.text = "PLAYER 3";
        top.gameObject.SetActive(true); top.text = "PLAYER 2";
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
