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

    public void StartGame() {
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
                deck.Add(new Card(i,s));
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
        while (!isOver)
        {
            for (int i = 0; i< numPlayers; i++)
            {
                if (i == numPlayers - 1) player.isTurn = true;
                else players[i].PlayCard();
            }
        }
    }
   
    #region start
    public void StartGameOne() {
        numOpponents = 1; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    public void StartGameTwo() {
        numOpponents = 2; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    public void StartGameThree() {
        numOpponents = 3; numPlayers = numOpponents + 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    #endregion
}
