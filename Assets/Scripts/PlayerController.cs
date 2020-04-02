using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public AudioSource zapSound;

    public GameObject track_set;
    private GameObject track_set_clone;
    
    public GameObject overhead_bars;
    private GameObject overhead_bar_clone;

    private float sidewaysForce = 2f;
    private float forwardsForce = 40f;

    

    private bool moveLeft = false;
    private bool moveRight = false;

    private int state = 2;


    private float offset = 60f;
    private float overheadOffset = 60f;

    private float screenWidth;

    private float updatedForSpeed = 0f;

    private GameManager game_manager;

    
    

    // Start is called before the first frame update
   
    void Start()
    {
        game_manager = FindObjectOfType<GameManager>();
        screenWidth = Screen.width;
    }

    private void SpeedUpdates()
    {
        if (game_manager.gameScore % 10 == 0 && game_manager.gameScore != 0 && updatedForSpeed != game_manager.gameScore)
        {
            updatedForSpeed = game_manager.gameScore;
            forwardsForce += 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUpdates();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if ((touch.position.x < screenWidth / 2) && ((state - 1) == 1 || (state - 1) == 2))
                {
                    moveLeft = true;
                    state -= 1;
                }
                if ((touch.position.x > screenWidth / 2) && ((state + 1) == 2 || (state + 1) == 3))
                {
                    moveRight = true;
                    state += 1;
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && ((state - 1) == 1 || (state - 1) == 2))
        {
            moveLeft = true;
            
            state -= 1;
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && ((state + 1) == 2 || (state + 1) == 3))
        {
            moveRight = true;
            
            state += 1;
        }
    }
    private void FixedUpdate()
    {

        if (!game_manager.GameOver)
        {
            transform.Translate(Vector3.down * forwardsForce * Time.deltaTime);
            if (moveLeft)
            {
                transform.Translate(Vector3.left * sidewaysForce);
                zapSound.Play();
                moveLeft = false;
            }
            if (moveRight)
            {
                transform.Translate(Vector3.right * sidewaysForce);
                zapSound.Play();
                moveRight = false;
            }
        }
        
    }

    private void CheckIfPass(Collider _collider)
    {
        if (_collider.tag.Equals("red_obstacle") && state != 1)
        {
            game_manager.EndGame();

        }
        else if(_collider.tag.Equals("red_obstacle") && state == 1)
        {
            game_manager.IncrementScore();
        }

        if (_collider.tag.Equals("yellow_obstacle") && state != 2)
        {
            game_manager.EndGame();
        }
        else if (_collider.tag.Equals("yellow_obstacle") && state == 2)
        {
            game_manager.IncrementScore();
        }

        if (_collider.tag.Equals("blue_obstacle") && state != 3)
        {
            game_manager.EndGame();
        }
        else if (_collider.tag.Equals("blue_obstacle") && state == 3)
        {
            game_manager.IncrementScore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("CreateTrack"))
        {
            
            track_set_clone = (GameObject)Instantiate(track_set, new Vector3(0, 0, offset), new Quaternion(0, 0, 0, 0));
            overhead_bar_clone = (GameObject)Instantiate(overhead_bars, new Vector3(0, 35.4f, overheadOffset), new Quaternion(0, 0, 0, 0));

            offset += 60f;
            overheadOffset += 60;

            Destroy(track_set_clone, 10f);
            Destroy(overhead_bar_clone, 10f);
        }
        
        if (!other.tag.Equals("CreateTrack"))
        {
            CheckIfPass(other);
            Destroy(other.gameObject);
        }

    }
}
