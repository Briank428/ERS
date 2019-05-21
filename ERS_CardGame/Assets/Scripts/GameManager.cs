using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button one;
    public Button two;
    public Button three;
    public Text playerText;
    public Text text;
    public AI prefabAI;

    private int numPlayers;
    private List<AI> players;

    // Start is called before the first frame update
    void Start()
    {
        one.onClick.AddListener(StartGameOne);
        two.onClick.AddListener(StartGameTwo);
        three.onClick.AddListener(StartGameThree);
        players = new List<AI>();
    }

    public void StartGame() {
        Debug.Log("Number of Opponents: " + numPlayers);
        for (int i = 0; i< numPlayers; i++)
        {
            players.Add(GameObject.Instantiate(prefabAI) as AI);
        }
        List<Card> deck = new List<Card>();
        foreach(string s in Card.suits)
        {
            for(int i = 1; i <= 14; i++)
            {
                deck.Add(new Card(i,s));
            }
        }
        Helper.Shuffle(deck);
        while(deck.Count > 0)
        {
            // for every player, try to enque card from deck
        }
    }
   
    #region start
    public void StartGameOne() {
        numPlayers = 1;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    public void StartGameTwo() {
        numPlayers = 2;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    public void StartGameThree() {
        numPlayers = 3;
        one.gameObject.SetActive(false); two.gameObject.SetActive(false); three.gameObject.SetActive(false); playerText.gameObject.SetActive(false);
        StartGame();
    }
    #endregion
}
