using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    
    public float timeRequired = 1f;
    public int state = 0;
    public GameObject PlayerCollection;
    public GameObject LoaderPanel;
    public AudioSource MoveSound;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI selfCoinText;
    public TextMeshProUGUI buyButtonText;
    public TextMeshProUGUI selectButtonText;
    private bool moveLeft = false;
    private bool moveRight = false;
    private Vector3 target;
    private int playerCount = 9;
    List<int> inventory;
    private int[] itemAmount = {0, 100, 200, 300, 400, 500, 600, 700, 800 };
    // Start is called before the first frame update
    void Start()
    {
        coinText.text = itemAmount[state].ToString();
        /*Uncomment To reset stats
        PlayerPrefs.SetInt("coinAmount", 1000);
        PlayerPrefs.SetString("inventory", "_0_");
        PlayerPrefs.SetInt("current_cube", 0);
        PlayerPrefs.Save();
        */

        selfCoinText.text = PlayerPrefs.GetInt("coinAmount").ToString();
        if (!PlayerPrefs.HasKey("inventory"))
        {
            PlayerPrefs.SetString("inventory", "_0_");
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("current_cube"))
        {
            PlayerPrefs.SetInt("current_cube", 0);
            PlayerPrefs.Save();
        }
        string player_inventory = PlayerPrefs.GetString("inventory");
        inventory = new List<int>();
        for (int i = 0; i < player_inventory.Length; i++)
        {
            if (player_inventory[i] != '_')
            {
                inventory.Add(int.Parse(player_inventory[i] + ""));
            }
            
        }
        if (inventory.Contains(state))
        {
            coinText.text = "0";
            buyButtonText.text = "Purchased";
            buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;

        }
        if (PlayerPrefs.GetInt("current_cube") == state)
        {
            selectButtonText.text = "SELECTED";
            buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            buyButtonText.text = "Purchased";
        }


    }
    IEnumerator LoadMainGameAsync()
    {
        AsyncOperation loadMainGame = SceneManager.LoadSceneAsync("MainGame");
        while (!loadMainGame.isDone)
        {
            yield return null;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoaderPanel.SetActive(true);
            Invoke("CallCoroutine", 0.2f);
        }
    }
    void CallCoroutine()
    {
        StartCoroutine(LoadMainGameAsync());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveLeft && !moveRight)
        {
            
            PlayerCollection.transform.position = Vector3.Lerp(PlayerCollection.transform.position, target, 0.6f);
            if (target == PlayerCollection.transform.position)
            {
                moveLeft = false;

            }
        }
        if (moveRight && !moveLeft)
        {

            PlayerCollection.transform.position = Vector3.Lerp(PlayerCollection.transform.position, target, 0.6f);
            if (target == PlayerCollection.transform.position)
            {
                moveRight = false;

            }
        }
    }
    public void MoveLeft()
    {
        if (state > 0)
        {
            state -= 1;
            selectButtonText.text = "SELECT";
            buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            selectButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            buyButtonText.text = "BUY";
            MoveSound.Play();
            target = new Vector3(PlayerCollection.transform.position.x + 6.65f, PlayerCollection.transform.position.y, PlayerCollection.transform.position.z);      
            moveLeft = true;
            coinText.text = itemAmount[state].ToString();
            if (int.Parse(selfCoinText.text) < int.Parse(coinText.text))
            {
                buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            }
            if (inventory.Contains(state))
            {
                buyButtonText.text = "Purchased";
                coinText.text = 0 + "";
                buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            }
            else
            {
                selectButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            }
            if (PlayerPrefs.GetInt("current_cube") == state)
            {
                selectButtonText.text = "SELECTED";
            }
        }
    }
    public void MoveRight()
    {
        if (state < playerCount - 1)
        {
            selectButtonText.text = "SELECT";
            buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            selectButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            buyButtonText.text = "BUY";
            MoveSound.Play();
            target = new Vector3(PlayerCollection.transform.position.x - 6.65f, PlayerCollection.transform.position.y, PlayerCollection.transform.position.z);
            state += 1;
            moveRight = true;
            coinText.text = itemAmount[state].ToString();
            if (int.Parse(selfCoinText.text) < int.Parse(coinText.text))
            {
                buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            }
            if (inventory.Contains(state))
            {
                buyButtonText.text = "Purchased";
                coinText.text = 0 + "";
                buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            }
            else
            {
                selectButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            }
            if (PlayerPrefs.GetInt("current_cube") == state)
            {
                selectButtonText.text = "SELECTED";
            }
        }
    }
    public void BuyItem()
    {
        if (!inventory.Contains(state))
        {
            string str_inventory = PlayerPrefs.GetString("inventory");
            str_inventory = str_inventory + state.ToString() + "_";
            if (int.Parse(selfCoinText.text) >= int.Parse(coinText.text))
            {
                PlayerPrefs.SetInt("coinAmount", int.Parse(selfCoinText.text) - int.Parse(coinText.text));
                PlayerPrefs.Save();
                PlayerPrefs.SetString("inventory", str_inventory);
                PlayerPrefs.Save();
                selfCoinText.text = PlayerPrefs.GetInt("coinAmount").ToString();
                coinText.text = 0 + "";
                inventory.Add(state);
                buyButtonText.text = "Purchased";
                buyButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = false;
                selectButtonText.transform.parent.gameObject.GetComponent<Button>().interactable = true;


            }
        }
       
        
        
        
    }
    public void SetSkin()
    {
        
        if (PlayerPrefs.GetInt("current_cube") == state)
        {
            //al
        }
        else
        {
            selectButtonText.text = "SELECTED";
            PlayerPrefs.SetInt("current_cube", state);
            PlayerPrefs.Save();
        }
    }
}
