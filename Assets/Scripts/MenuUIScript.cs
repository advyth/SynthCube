﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource menuAudio;
    public AudioSource ClickSound;
    public GameObject loaderPanel;
    public GameObject coinScoreText;
    public GameObject highScoreText;
    public Slider progressBar;
    void Start()
    {
        loaderPanel.SetActive(false);
        if (PlayerPrefs.HasKey("topScore"))
        {
            highScoreText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("topScore").ToString();
        }
        if (PlayerPrefs.HasKey("coinAmount"))
        {
            coinScoreText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("coinAmount").ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Application.platform == RuntimePlatform.Android)
        {
            Application.Quit();
        }
    }
    public void buttonClick()
    {
        ClickSound.Play();
        StartCoroutine(AudioFader.Fader(menuAudio, 2.2f, 0));
        loaderPanel.SetActive(true);
        Invoke("loaderRoutineStart", 2f);
        //Invoke("LoadScene", 2.2f);

        
    }

    
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation loadSync = SceneManager.LoadSceneAsync(1);
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
