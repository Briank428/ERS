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
    public GameObject cardPrefab;
    public float timePlayed;

    private int faceCardIndex;
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
    }

    public void Deal() {
        Debug.Log("Number of Opponents: " + numOpponents);
        for (int i =0; i< numOpponents; i++)
        {
            players.Add(Instantiate(prefabAI) as AI);
        }
        List<Card> deck = new List<Card>();
        foreach(string s in Card.suits)
        {
            for(int i = 1; i < 14; i++)
            {
                GameObject temp = Instantiate(cardPrefab);
                temp.GetComponent<Card>().CardInit(i,s);
                deck.Add(temp.GetComponent<Card>());
            }
        }
        Helper.Shuffle(deck);
        int index = 0;
        while(deck.Count > 0)
        {
            {
                if (index != numOpponents) { players[index].AddCard(deck[0]); index++; }
                else { player.AddCard(deck[0]); index = 0; }
                deck.RemoveAt(0);
            }
        }
        Debug.Log("Player: " + player.HandSize()); for (int i = 0; i < numOpponents; i++) Debug.Log("Opponent " + i + 1 + ": " + players[i].HandSize());
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
                Debug.Log("Player #" + i+ "'s Value: " + Pile.GetTopCard().value);
                Card temp = Pile.GetTopCard();


                if (Pile.ValidSlap()) { 
                    //find minimum time of AI
                    float minAI = players[0].SlapTime(); int lowest = 0;
                    for (int j = 1; j < numPlayers-1; j++) {
                        float tempSlap = players[j].SlapTime();
                        if (minAI > tempSlap) { minAI = tempSlap; lowest = j; }
                    }
                    //Compare to player (wait until have played?)
                    if (minAI < player.slapTime - timePlayed) { players[lowest].AddToHand(); /*animation? */ text.text = "Player " + 1 + lowest + " slapped first!"; i = lowest - 1; }
                    else { player.AddToHand(); /*animation?*/ text.text = "You slapped first!"; i = numPlayers - 2; }

                    continue;
                }
                if (temp.IsFaceCard()) { FaceCard(temp, i); i = faceCardIndex; }
            }   
        }   
    }   

    public IEnumerator FaceCard(Card a, int index) {
        if (index == numPlayers - 1) index = 0; else index++;
        if (a.value == 11) {
            if (index == numPlayers - 1) { player.isTurn = true; while (!Input.GetKeyDown(KeyCode.Mouse0)) yield return null;
                if (Pile.GetTopCard().IsFaceCard()) FaceCard(Pile.GetTopCard(), index); 
            }
            else { players[index].PlayCard(); if (Pile.GetTopCard().IsFaceCard()) FaceCard(Pile.GetTopCard(), index); }
        }
        else if (a.value == 12)
        {

        }

        //faceCardIndex = num-1 of index 
    }

    #region start
    public void StartGameOne() {
        numOpponents = 1; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        Deal();
    }
    public void StartGameTwo() {
        numOpponents = 2; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        Deal();
    }
    public void StartGameThree() {
        numOpponents = 3; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        Deal();
    }
    #endregion
}
