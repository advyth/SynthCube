using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Game Manager
    private GameManager game_manager;

    public AudioSource zapSound;
    
    //Map Game Objects
    public GameObject track_set;
    private GameObject track_set_clone;
    
    //Overhead Bar Objects
    

    //Physics Forces
    private float sidewaysForce = 2f;
    private float forwardsForce = 40f;

    //Player Movement vars
    private bool moveLeft = false;
    private bool moveRight = false;

    //Track State Management
    private int state = 2;

    //Offset For Track Spawning
    private float offset = 60f;
    
    //Offset For Overhead Bar Spawning
    private float overheadOffset = 60f;

    private float screenWidth;

    //Variable To Check If Speed has been Incremented for x speed
    private float updatedForSpeed = 0f;

    //Locally setting game state to "Just Began"
    private string gameState = "init";

    private float oldSpeed = 0f;

    private bool speedUpdated = false;

    private int createTriggersPassed = 0;

    private float prevDestroyTime = 20f;





    // Start is called before the first frame update

    void Start()
    {
        game_manager = FindObjectOfType<GameManager>();
        screenWidth = Screen.width;
    }

    //Function for incrementing speed gradually based on points earned
    //TODO Set speed bar
    //TODO Stepped Increments {+5, +10, +14, +17}
    private void SpeedUpdates()
    {
        if (game_manager.PowerupStatus() && !speedUpdated)
        {
            oldSpeed = forwardsForce;
            forwardsForce = 120f;
            speedUpdated = true;
        }
        else if(!game_manager.PowerupStatus())
        {
            if (forwardsForce == 120f)
            {
                speedUpdated = false;
                forwardsForce = oldSpeed;
            }
            if (updatedForSpeed != game_manager.gameScore && game_manager.gameScore != 0)
            {
                if (game_manager.gameScore == 10)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 10f;
                }
                else if (game_manager.gameScore == 20)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 9f;
                }
                else if (game_manager.gameScore == 30)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 8f;
                }
                else if (game_manager.gameScore == 40)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 7f;
                }
                else if (game_manager.gameScore == 50)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 6f;
                }
                else if (game_manager.gameScore == 70)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 4f;
                }
                else if (game_manager.gameScore == 80)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 3f;
                }
                else if (game_manager.gameScore == 90)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 2f;
                }
                else if (game_manager.gameScore == 100)
                {
                    updatedForSpeed = game_manager.gameScore;
                    forwardsForce += 1f;
                }

            }
        }
        
        
    }

    
    // Update is called once per frame
    void Update()
    {
        //Retrieve current state from GameManager Component
        gameState = game_manager.gameState;

        SpeedUpdates();

        //Mobile Input
        if (game_manager.allowControl)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if ((touch.position.x < screenWidth / 2) && touch.position.y < Screen.height / 1.5f && ((state - 1) == 1 || (state - 1) == 2))
                    {
                        moveLeft = true;
                        state -= 1;
                        SetGlow(state);
                    }
                    if ((touch.position.x > screenWidth / 2) && touch.position.y < Screen.height / 1.5f && ((state + 1) == 2 || (state + 1) == 3))
                    {
                        moveRight = true;
                        state += 1;
                        SetGlow(state);
                    }
                }

            }
            //Keyboard Input
            if (Input.GetKeyDown(KeyCode.LeftArrow) && ((state - 1) == 1 || (state - 1) == 2))
            {
                moveLeft = true;

                state -= 1;
                SetGlow(state);

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && ((state + 1) == 2 || (state + 1) == 3))
            {
                moveRight = true;

                state += 1;
                SetGlow(state);
            }
        }
        
    }
    void SetGlow(int index)
    {
        /*
        Material left_mat = track_set.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
        Material center_mat = track_set.transform.GetChild(2).gameObject.GetComponent<Renderer>().material;
        Material right_mat = track_set.transform.GetChild(3).gameObject.GetComponent<Renderer>().material;
        if (index == 1)
        {


            left_mat.EnableKeyword("_EMISSION");
            center_mat.DisableKeyword("_EMISSION");
            right_mat.DisableKeyword("_EMISSION");


        }
        if (index == 2)
        {

            left_mat.DisableKeyword("_EMISSION");
            //left_mat.SetColor("_EmissionColor", non_glow);
            center_mat.EnableKeyword("_EMISSION");
            right_mat.DisableKeyword("_EMISSION");

        }
        if (index == 3)
        {


            left_mat.DisableKeyword("_EMISSION");
            center_mat.DisableKeyword("_EMISSION"); 
            right_mat.EnableKeyword("_EMISSION");


        }*/
    }
    private void FixedUpdate()
    {

        if (gameState.Equals("play"))
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
        if (!game_manager.PowerupStatus())
        {
            if (_collider.tag.Equals("red_obstacle") && state != 1)
            {
                game_manager.EndGame();

            }
            else if (_collider.tag.Equals("red_obstacle") && state == 1)
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
        else
        {
            game_manager.IncrementScore();
            Destroy(_collider.gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        createTriggersPassed++;
        if (other.tag.Equals("CreateTrack"))
        {
            for (int i = 0; i < 3; i++)
            {
                track_set_clone = (GameObject)Instantiate(track_set, new Vector3(0, 0, offset), new Quaternion(0, 0, 0, 0));
                //overhead_bar_clone = (GameObject)Instantiate(overhead_bars, new Vector3(0, 35.4f, overheadOffset), new Quaternion(0, 0, 0, 0));

                offset += 60f;
                overheadOffset += 60;
                Destroy(track_set_clone,prevDestroyTime + i*10);
                prevDestroyTime = prevDestroyTime + i * 10;

            }
            
           
        }
        
        
        if (!other.tag.Equals("CreateTrack")  && !other.tag.Equals("PowerUp_1"))
        {
            CheckIfPass(other);
            Destroy(other.gameObject);
        }
        
        if (other.tag.Equals("PowerUp_1"))
        {
            game_manager.ActivatePowerUp();
            game_manager.PowerUp.SetActive(false);
        }

    }
}
