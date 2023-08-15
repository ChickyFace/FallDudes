using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Start")]
    public GameObject Player;
    public AiSpawner spawner;
    public TextMeshProUGUI countdownText;
    public Button startButton;
    public bool countdownStarted;
    float timer = 3f;

    [Header("Finish")]
    public GameObject leaderboard;
    public RectTransform leaderboardRectTransform;
    public Vector2 leaderboardWinPosition ; // middle of the screen
    public TextMeshProUGUI winLoseText;
    public GameObject winLosePanel;
    AnimatorManager animatorManager;


    private void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
    }
    private void Update()
    {
       

        if (countdownStarted)
        {

            startButton.gameObject.SetActive(false);
            timer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(timer).ToString();


            if (timer == 1f)
            {
                countdownText.text = "Get Ready!";
                
            }
            if (timer <= 0f)
            {

                
                StartGame();
                countdownText.text = "0";
                countdownText.text = "";
                //countdownStarted = false;
                //spawner.gameStart = true;



            }
         
        }
    }
    public void StartCountdown()
    {
        countdownStarted = true;
    }
    public void StartGame()
    {

        spawner.gameStart = true;
        Player.SetActive(true);
        countdownStarted = false;


    }
    private void ShowWinPanel()
    {
        winLosePanel.SetActive(true);
        winLoseText.text = "You Win!";
        MoveLeaderboardToCenter();
        animatorManager.PlayTargetAnimation("Victory_Dance", true);
    }
    private void ShowLosePanel()
    {
        winLosePanel.SetActive(true);
        winLoseText.text = "You Lose!";
        MoveLeaderboardToCenter();
    }
    private void MoveLeaderboardToCenter()
    {
        leaderboardRectTransform.anchoredPosition = leaderboardWinPosition;
    }
    public void OnPlayerFinished()
    {
        ShowWinPanel();
    }
    public void OnOpponentFinished()
    {
        ShowLosePanel();
    }

}
