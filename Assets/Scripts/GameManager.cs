using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int gameScore;
    public Text gameScoreText;
    public Text endText;
    public GameObject endui;
    public bool GameOver = false;
    public Button restartButton;
    
    void Start()
    {
       
        gameScore = 0;
        gameScoreText.text = gameScore.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EndGame()
    {
        GameOver = true;
        Debug.Log("Game over");
        endText.text = gameScore.ToString();
        endui.SetActive(true);
        //Invoke("RestartScene", 2f);
        
    }

    public void IncrementScore()
    {
        ++gameScore;
        gameScoreText.text = gameScore.ToString();

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
     
}
