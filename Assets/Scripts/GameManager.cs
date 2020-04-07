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
    public GameObject LoaderPanel;
    public GameObject GameOverUI;
    
    public Text endText;
    public Button restartButton;
    public Slider progressBar;
    public GameObject BoostTimer;

    //Game state management vars
    public int gameScore;
    public string gameState;
    public bool allowControl = false;
    
    //Audio Sources
    public AudioSource BackgroundMusic;
    public AudioSource PointSource;
    public AudioSource GameOverAudio;
    public AudioSource ShipEngineAudio;
    public AudioSource ClickAudio;
    public AudioSource PowerUpAudio;
    public AudioSource BoostSource;

    private bool powerUpActive;

    public GameObject Player;

    public GameObject PowerUp;
    private TimerScript timerScript;




    void Start()
    {
        //Panel init
        timerScript = FindObjectOfType<TimerScript>();
        PauseButton.SetActive(false);
        PausePanel.SetActive(false);
        GameOverUI.SetActive(false);
        LoaderPanel.SetActive(false);
        BoostTimer.SetActive(false);
        //TouchToStartPanel.SetActive(false);

        //Responsive UI placements
        ScoreTextMesh.transform.position = new Vector2(100, Screen.height - 80);
        PauseButton.transform.position = new Vector2(Screen.width - 100, Screen.height - 80);

        //state init
        gameState = "init";
        gameScore = 0;

        //audio init
        BackgroundMusic.volume = 0;
        BackgroundMusic.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic,2f, 0.4f));
        
        //draw score
        ScoreTextMesh.GetComponent<TextMeshProUGUI>().text = gameScore.ToString();
        //gameScoreText.text = gameScore.ToString();

        Player.transform.GetChild(0).gameObject.SetActive(false) ;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && gameState.Equals("init") && FindObjectOfType<CameraController>().animationOver)
        {
            TouchToStartPanel.SetActive(false);
            allowControl = true;
            gameState = "play";
            PauseButton.SetActive(true);
        }
    }

    public void ActivatePowerUp()
    {
        
        BoostTimer.SetActive(true);
        timerScript.startTimer = true;
        PowerUpAudio.Play();
        BoostSource.Play();
        powerUpActive = true;
        Player.transform.GetChild(0).gameObject.SetActive(true);
        Invoke("PreparePowerDisable", 5f);
    }
    public void PreparePowerDisable()
    {
        BoostTimer.SetActive(false);
        Player.transform.GetChild(0).gameObject.SetActive(false);
        Invoke("EaseOnDisable", 1f);
        timerScript.ResetTimer();
    }
    public void EaseOnDisable()
    {
        powerUpActive = false;
        PowerUp.SetActive(true);

    }
    public bool PowerupStatus()
    {
        return powerUpActive;
    }
    public void EndGame()
    {
        
        gameState = "over";
        ShipEngineAudio.Stop();
        GameOverAudio.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic,1f, 0f));
        endText.text = gameScore.ToString();
        GameOverUI.SetActive(true);
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
        ClickAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        ClickAudio.Play();
        Time.timeScale = 0;
        gameState = "paused";
        PausePanel.SetActive(true);
    }
    public void PlayGame()
    {
        ClickAudio.Play();
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        gameState = "play";
    }
    public void exiitButtonClick()
    {
        ClickAudio.Play();
        //StartCoroutine(AudioFader.Fader(menuAudio, 2.2f, 0));
        LoaderPanel.SetActive(true);
        Invoke("loaderRoutineStart", 2f);
        //Invoke("LoadScene", 2.2f);


    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation loadSync = SceneManager.LoadSceneAsync(0);
        float prevProgress = 0;
        while (!loadSync.isDone)
        {
            StartCoroutine(LoadSmoothly(prevProgress, loadSync.progress, 1f, progressBar));
            prevProgress = loadSync.progress;
            yield return null;
        }
        //yield return null;
    }
    void loaderRoutineStart()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSmoothly(float current, float target, float duration, Slider progress)
    {
        float currtime = 0f;
        while (currtime < duration)
        {
            currtime += Time.deltaTime;
            current = Mathf.Lerp(current, target, 0.1f);
            progress.value = current * 10f;
        }
        yield return null;
    }
    

}
