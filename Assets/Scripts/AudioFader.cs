using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator Fader(AudioSource audioSource, float fadeDuration, float finalVolume) //couroutine to fade audio
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while(currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, finalVolume, currentTime/fadeDuration);
            yield return null;
        }
        yield break;
    }
}
