using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    //UI elements
    public GameObject TouchToStartPanel;
    public GameObject PausePanel;
    public GameObject PauseButton;
    public GameObject ScoreTextMesh;
    public GameObject endui;
    public Text endText;
    public Button restartButton;

    //Game state management vars
    public int gameScore;
    public string gameState;
    
    //Audio Sources
    public AudioSource BackgroundMusic;
    public AudioSource PointSource;
    public AudioSource GameOverAudio;
    public AudioSource ShipEngineAudio;

    
    
    void Start()
    {
        TouchToStartPanel.SetActive(true);
        ScoreTextMesh.transform.position = new Vector2(100, Screen.height - 60);
        PauseButton.transform.position = new Vector2(Screen.width - 80, Screen.height - 60);
        gameState = "init";
        PausePanel.SetActive(false);
        endui.SetActive(false);
        BackgroundMusic.volume = 0;
        BackgroundMusic.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic,2f, 0.4f));
        gameScore = 0;
        ScoreTextMesh.GetComponent<TextMeshProUGUI>().text = gameScore.ToString();
        //gameScoreText.text = gameScore.ToString();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) && gameState.Equals("init"))
        {
            TouchToStartPanel.SetActive(false);
            gameState = "play";
        }
    }
    public void EndGame()
    {
        
        gameState = "over";
        ShipEngineAudio.Stop();
        GameOverAudio.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic,1f, 0f));
        endText.text = gameScore.ToString();
        endui.SetActive(true);
        //Invoke("RestartScene", 2f);
        
    }

    public void IncrementScore()
    {
        PointSource.Play();
        ++gameScore;
        ScoreTextMesh.GetComponent<TextMeshProUGUI>().text = gameScore.ToString();
        //gameScoreText.text = gameScore.ToString();

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameState = "paused";
        PausePanel.SetActive(true);
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        gameState = "play";
    }
     
}
