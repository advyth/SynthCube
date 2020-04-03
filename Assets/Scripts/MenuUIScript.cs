using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI
;
public class MenuUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource menuAudio;
    public GameObject loaderPanel;
    public Slider progressBar;
    void Start()
    {
        loaderPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buttonClick()
    {
        Debug.Log("Clicked");
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
            StartCoroutine(LoadSmoothly(prevProgress, loadSync.progress, 4f, progressBar));
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
