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
    public GameObject PausePanel;
    public GameObject PauseButton;
    public GameObject ScoreTextMesh;
    public GameObject LoaderPanel;
    public GameObject GameOverUI;
    public GameObject CoinScoreText;
    public GameObject HighScoreText;
    public GameObject PowerUpParent;
    public GameObject MenuUI;
    public GameObject MenuCoinScore;
    public GameObject QuitGamePanel;
    public GameObject GoButtonObject;
    public GameObject GameCoinImage;

    public Text endText;
    public Button restartButton;
    public Slider progressBar;
    public GameObject BoostTimer;

    //Game state management vars
    public int gameScore;
    private int coinScore;
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
    public AudioSource CoinAudio;
    public AudioSource MenuMusic;

    private bool powerUpActive;

    public GameObject Player;

    public float powerUpDuration = 5;

    public GameObject Store;
    public GameObject CameraController;
    public GameObject MainCamera;




    void Start()
    {
        GoButtonObject.SetActive(false);
        for (int i = 0; i < Player.transform.childCount; i++)
        {
            if (PlayerPrefs.GetInt("current_cube") == i)
            {
                Player.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Player.transform.GetChild(i).gameObject.SetActive(false);

            }
        }
        MenuUI.SetActive(true);
        //Panel init
        CoinScoreText.SetActive(false);
        ScoreTextMesh.SetActive(false);
        GameCoinImage.SetActive(false);
        PauseButton.SetActive(false);
        PausePanel.SetActive(false);
        GameOverUI.SetActive(false);
        LoaderPanel.SetActive(false);
        BoostTimer.SetActive(false);
        HighScoreText.SetActive(false);
        QuitGamePanel.SetActive(false);
        //TouchToStartPanel.SetActive(false);

        //Responsive UI placements
        //ScoreTextMesh.transform.position = new Vector2(100, Screen.height - 80);
        PauseButton.transform.position = new Vector2(Screen.width - 100, Screen.height - 80);
        //CoinScoreText.transform.position = new Vector2(100, Screen.height - 210);

        //state init
        gameState = "init";
        gameScore = 0;
        coinScore = 0;
        if (!PlayerPrefs.HasKey("coinAmount"))
        {
            PlayerPrefs.SetInt("coinAmount", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("topScore"))
        {
            PlayerPrefs.SetInt("topScore", 0);
            PlayerPrefs.Save();
        }

        MenuCoinScore.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("coinAmount").ToString();

        //audio init
        BackgroundMusic.volume = 0;
        BackgroundMusic.Play();
        ShipEngineAudio.Stop();
        StartCoroutine(AudioFader.Fader(MenuMusic, 2f, 0.4f));

        //draw score
        ScoreTextMesh.GetComponent<TextMeshProUGUI>().text = gameScore.ToString();
        //gameScoreText.text = gameScore.ToString();

        Player.transform.GetComponent<TrailRenderer>().enabled = false;

        MenuUI.GetComponent<Animator>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (powerUpActive)
        {
            PowerUpStartTime();
        }
    }
    void PowerUpStartTime()
    {
        if (powerUpDuration > 0)
        {
            powerUpDuration -= Time.deltaTime;
        }
        if (BoostTimer.GetComponent<TimerScript>().duration <= 0)
        {
            PreparePowerDisable();
            BoostTimer.GetComponent<TimerScript>().ResetTimer();

        }
    }

    public void AllowControl()
    {
        allowControl = true;
    }
    public void ActivatePowerUp()
    {
        if (powerUpActive)
        {
            powerUpDuration = BoostTimer.GetComponent<TimerScript>().duration + (5 - (BoostTimer.GetComponent<TimerScript>().duration));
            BoostTimer.GetComponent<TimerScript>().AddDuration(5);
            BoostSource.Play();
        }
        else
        {
            PowerUpParent.SetActive(false);
            powerUpActive = true;
            BoostTimer.SetActive(true);
            BoostTimer.GetComponent<TimerScript>().startTimer = true;
            PowerUpAudio.Play();
            BoostSource.Play();
            Player.transform.GetComponent<TrailRenderer>().enabled = true;
        }
    }
    public void PreparePowerDisable()
    {
        //BoostTimer.SetActive(false);
        Player.transform.GetComponent<TrailRenderer>().enabled = false;
        FindObjectOfType<CameraController>().zoomOutDone = true;
        Invoke("EaseOnDisable", 0.5f);
        BoostTimer.GetComponent<TimerScript>().ResetTimer();
    }
    public void EaseOnDisable()
    {
        powerUpDuration = 0;
        powerUpActive = false;
        PowerUpParent.SetActive(true);


    }
    public void WaitForSecondsAndPlay()
    {

        MenuUI.GetComponent<Animator>().enabled = true;
        CameraController.GetComponent<CameraController>().StartCamera();
        StartCoroutine(AudioFader.Fader(MenuMusic, 1f, 0f));
        MenuMusic.Stop();
        Invoke("DisableGameMenuUI", 2.9f);
        StartCoroutine(AudioFader.Fader(BackgroundMusic, 2f, 0.4f));
        ShipEngineAudio.Play();

    }
    public void DisableGameMenuUI()
    {
        MenuUI.SetActive(false);
        ScoreTextMesh.SetActive(true);
        CoinScoreText.SetActive(true);
        GameCoinImage.SetActive(true);

    }
    
    public void GoToStore()
    {


        LoaderPanel.SetActive(true);
        SceneManager.LoadScene("StoreScene");
    }
    public void QuitGameDialog()
    {
        QuitGamePanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void QuitGameDialogNo()
    {
        QuitGamePanel.SetActive(false);
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public bool PowerupStatus()
    {
        return powerUpActive;
    }
    public void EndGame()
    {
        Handheld.Vibrate();
        if (PlayerPrefs.HasKey("topScore"))
        {
            if (PlayerPrefs.GetInt("topScore") < gameScore)
            {
                HighScoreText.SetActive(true);
                PlayerPrefs.SetInt("topScore", gameScore);
                if (PlayerPrefs.HasKey("coinAmount"))
                {
                    int coinAmount = coinScore + PlayerPrefs.GetInt("coinAmount");
                    PlayerPrefs.SetInt("coinAmount", coinAmount);
                    PlayerPrefs.Save();

                }
            }
            else
            {
                int coinAmount = coinScore + PlayerPrefs.GetInt("coinAmount");
                PlayerPrefs.SetInt("coinAmount", coinAmount);
                PlayerPrefs.Save();
            }

        }
        else
        {
            PlayerPrefs.SetInt("topScore", gameScore);
            PlayerPrefs.SetInt("coinAmount", coinScore);
            PlayerPrefs.Save();
        }
        ScoreTextMesh.SetActive(false);
        CoinScoreText.SetActive(false);
        gameState = "over";
        ShipEngineAudio.Stop();
        GameOverAudio.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic, 1f, 0f));
        endText.text = gameScore.ToString();
        GameOverUI.SetActive(true);
        GameOverUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = coinScore.ToString();
        GameCoinImage.SetActive(false);
        //Invoke("RestartScene", 2f);

    }
    public void IncrementCoinScore()
    {
        CoinAudio.Play();
        ++coinScore;
        CoinScoreText.GetComponent<TextMeshProUGUI>().text = coinScore.ToString();

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
        ResetScene();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResetScene()
    {
        ScoreTextMesh.SetActive(false);
        gameState = "init";
        Player.transform.position = new Vector3(0f, 1.85f, -26.3f);
        MainCamera.transform.localEulerAngles = new Vector3(10, 0, 0);
        var cloneTrack = GameObject.FindGameObjectsWithTag("clone_track");
        var cloneCoin = GameObject.FindGameObjectsWithTag("coin_clone");
        var cloneObstacleYellow = GameObject.FindGameObjectsWithTag("obstacle_clone_yellow");
        var cloneObstacleRed = GameObject.FindGameObjectsWithTag("obstacle_clone_red");
        var cloneObstacleBlue = GameObject.FindGameObjectsWithTag("obstacle_clone_blue");
        var cloneMountain = GameObject.FindGameObjectsWithTag("mountains");

        var clonePowerup = GameObject.FindGameObjectsWithTag("powerup_clone");
        foreach (GameObject obj in cloneMountain)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in cloneTrack)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in cloneCoin)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in cloneObstacleRed)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in cloneObstacleYellow)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in cloneObstacleBlue)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in clonePowerup)
        {
            Destroy(obj);
        }
        FindObjectOfType<ObstacleSpawner>().obstacleZ = 90f;
        FindObjectOfType<PlayerController>().ResetEverything();
        
        GameOverUI.SetActive(false);

        BackgroundMusic.Stop();
        BackgroundMusic.Play();
        StartCoroutine(AudioFader.Fader(BackgroundMusic, 2f, 0.4f));
        allowControl = false;
        coinScore = 0;
        gameScore = 0;
        ScoreTextMesh.GetComponent<TextMeshProUGUI>().text = gameScore.ToString();
        CoinScoreText.GetComponent<TextMeshProUGUI>().text = coinScore.ToString();
        CoinScoreText.SetActive(true);
        GoButtonObject.SetActive(true);
        GameCoinImage.SetActive(true);




    }
    public void GoButtonClicked()
    {
        
        Invoke("GameStateSet", 0.3f);
    }
    public void GameStateSet()
    {
        gameState = "play";
        allowControl = true;
        ScoreTextMesh.SetActive(true);
        PauseButton.SetActive(true);
        GoButtonObject.SetActive(false);
        
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

}
