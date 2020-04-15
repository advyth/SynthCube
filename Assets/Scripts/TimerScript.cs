using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerScript : MonoBehaviour
{
    public bool startTimer = false;
    public float duration = 5f;
    private Slider progressBarBoost;
   
    // Start is called before the first frame update
    void Start()
    {
        progressBarBoost = this.gameObject.GetComponent<Slider>();
    }
    public void ResetTimer()
    {
        duration = 5f;
    }
    public void AddDuration(float d)
    {
        duration = duration +  (d - duration) ;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            progressBarBoost.value = duration * 0.2f;
            duration = duration - Time.deltaTime;
        }
        if (startTimer && duration < 0)
        {
            progressBarBoost.transform.gameObject.SetActive(false);
        }
    }
}
