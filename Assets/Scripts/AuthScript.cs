using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthData
{
    public string code_string;
    public AuthData(string code)
    {
        code_string = code;
    }
}
public class AuthScript : MonoBehaviour
{
    public InputField inpf;
    public string code;
    public GameObject messageText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("auth", 0) == 1)
        {
            SceneManager.LoadScene("MainGame");
        }
    }
   
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSubmit()
    {
        AuthData ad = new AuthData(inpf.text);
        StartCoroutine(Post("https://synthwaveauth.herokuapp.com/", ad));
        messageText.GetComponent<TextMeshProUGUI>().color = Color.blue;
        messageText.GetComponent<TextMeshProUGUI>().text = "Connecting";
    }
    public void LoadGame()
    {
        
    }

    public IEnumerator Post(string url, AuthData data)
    {
        var jsonData = JsonUtility.ToJson(data);
        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

           
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
        
            if (www.isDone)
            {
                var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                if (result.ToString() == "Authorized")
                {
                    messageText.GetComponent<TextMeshProUGUI>().color = Color.green;
                    messageText.GetComponent<TextMeshProUGUI>().text = "Success, loading.";
                    PlayerPrefs.SetInt("auth", 1);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MainGame");


                }
                else
                {
                    messageText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    messageText.GetComponent<TextMeshProUGUI>().text = "Failed, try again.";
                }

            }
        }
    }
    
    
}
