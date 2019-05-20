using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame() {
        Debug.Log("Number of Opponents: " + numPlayers);
        for (int i = 0; i< numPlayers; i++)
        {
            players.Add(GameObject.Instantiate(prefabAI) as AI);
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
