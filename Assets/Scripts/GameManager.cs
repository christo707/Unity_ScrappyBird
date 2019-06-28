using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void gameDelegate();
    public static event  gameDelegate onGameStarted;
    public static event  gameDelegate onGameOver;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    enum PageState {
        None,
        Start,
        GameOver,
        Countdown
    }
    int score = 0;
    bool gameOver = false;

    public bool GameOver{
        get { return gameOver; }
    }

    public int Score{
        get { return score; }
    }

    void Awake(){
        Instance = this;
    }

    void setPageState(PageState state){
        switch(state){
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void confirmGameOver() {
        onGameOver(); //Event
        scoreText.text = "0";
        setPageState(PageState.Start);
    }

    public void StartGame(){
        setPageState(PageState.Countdown);
    }

    void OnEnable(){
        CountDownText.onCountDownFinished += onCountDownFinished;
        TapController.onPlayerDied += onPlayerDied;
        TapController.onPlayerScored += onPlayerScored;
    }

    void onDisable(){
        CountDownText.onCountDownFinished -= onCountDownFinished;
        TapController.onPlayerDied -= onPlayerDied;
        TapController.onPlayerScored -= onPlayerScored;
    }

    void onCountDownFinished() {
        setPageState(PageState.None);
        onGameStarted();
        score = 0;
        gameOver = false;
    }

    void onPlayerDied(){
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if(score > savedScore){
            PlayerPrefs.SetInt("HighScore", score);
        }
        setPageState(PageState.GameOver);
    }

    void onPlayerScored(){
        score++;
        scoreText.text = score.ToString();
    }


   
}
