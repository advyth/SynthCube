using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Game Manager


    public AudioSource zapSound;
    //Map Game Objects
    public GameObject track_set;
    private GameObject track_set_clone;
    public GameObject mountains;
    private GameObject mountain_clone;
    
    //Overhead Bar Objects
    

    //Physics Forces
    private float sidewaysForce = 2f;
    private float forwardsForce = 50f;

    //Player Movement vars
    private bool moveLeft = false;
    private bool moveRight = false;

    //Track State Management
    public int state = 2;

    //Offset For Track Spawning
    public float offset = 60f;

    private float mountainOffset = 120;
    

    private float screenWidth;

    //Variable To Check If Speed has been Incremented for x speed
    private float updatedForSpeed = 0f;

    //Locally setting game state to "Just Began"
    private string gameState = "init";

    private float oldSpeed = 0f;

    private bool speedUpdated = false;


    private float prevDestroyTime = 20f;

    private Touch touch;


    public GameObject MainCamera;

    

    
    // Start is called before the first frame update
    public void ResetEverything()
    {
        moveLeft = false;
        moveRight = false;
        state = 2;
        offset = 60f;
        mountainOffset = 85f;
        forwardsForce = 50f;
        updatedForSpeed = 0f;
        oldSpeed = 0f;
        speedUpdated = false;
        prevDestroyTime = 20f;
        touch = new Touch();
    }
    void Start()
    {
        
        screenWidth = Screen.width;
        mountain_clone = (GameObject)Instantiate(mountains, new Vector3(22, 3.5f, mountainOffset), new Quaternion(0, 0, 0, 0));
        mountain_clone.tag = "mountains";
        offset += 85;
    }


    //Function for incrementing speed gradually based on points earned
    //TODO Set speed bar
    //TODO Stepped Increments {+5, +10, +14, +17}
    private void SpeedUpdates()
    {
        if (FindObjectOfType<GameManager>().PowerupStatus() && !speedUpdated)
        {
            oldSpeed = forwardsForce;
            forwardsForce = 120f;
            speedUpdated = true;
        }
        else if(!FindObjectOfType<GameManager>().PowerupStatus())
        {
            if (forwardsForce == 120f)
            {
                speedUpdated = false;
                forwardsForce = oldSpeed;
            }
            if (updatedForSpeed != FindObjectOfType<GameManager>().gameScore && FindObjectOfType<GameManager>().gameScore != 0)
            {
                if (FindObjectOfType<GameManager>().gameScore == 10)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 5f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 20)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 4f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 30)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 3f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 40)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 2f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 50)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 1f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 70)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 1f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 80)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 1f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 90)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 1f;
                }
                else if (FindObjectOfType<GameManager>().gameScore == 100)
                {
                    updatedForSpeed = FindObjectOfType<GameManager>().gameScore;
                    forwardsForce += 1f;
                }

            }
        }
        
        
    }


    // Update is called once per frame

    void Update()
    {
        //Retrieve current state from GameManager Component
        gameState = FindObjectOfType<GameManager>().gameState;

        SpeedUpdates();

        
        //Mobile Input
        if (FindObjectOfType<GameManager>().allowControl)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if ((touch.position.x < screenWidth / 4) && touch.position.y < Screen.height / 1.5f && ((state - 1) == 1 || (state - 1) == 2))
                    {
                        moveLeft = true;
                        state -= 1;
                    }
                    if ((touch.position.x > (screenWidth -  (screenWidth / 4))) && touch.position.y < Screen.height / 1.5f && ((state + 1) == 2 || (state + 1) == 3))
                    {
                        moveRight = true;
                        state += 1;
                    }
                }

            }
            //Keyboard Input
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
        if (!FindObjectOfType<GameManager>().PowerupStatus())
        {
            
            if (_collider.tag.Equals("obstacle_clone_yellow") && state != 1)
            {
                
                FindObjectOfType<GameManager>().EndGame();
            }
            else if (_collider.tag.Equals("obstacle_clone_yellow") && state == 1)
            {
                _collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                _collider.gameObject.GetComponent<ParticleSystem>().Play();
                FindObjectOfType<GameManager>().IncrementScore();
            }
            if (_collider.tag.Equals("obstacle_clone_red") && state != 2)
            {
                FindObjectOfType<GameManager>().EndGame();

            }
            else if (_collider.tag.Equals("obstacle_clone_red") && state == 2)
            {
                _collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                _collider.gameObject.GetComponent<ParticleSystem>().Play();
                FindObjectOfType<GameManager>().IncrementScore();
            }


            if (_collider.tag.Equals("obstacle_clone_blue") && state != 3)
            {
                FindObjectOfType<GameManager>().EndGame();
            }
            else if (_collider.tag.Equals("obstacle_clone_blue") && state == 3)
            {
                _collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                _collider.gameObject.GetComponent<ParticleSystem>().Play();
                FindObjectOfType<GameManager>().IncrementScore();
            }
        }
        else
        {
            FindObjectOfType<GameManager>().IncrementScore();
            Destroy(_collider.gameObject,1);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("CreateTrack"))
        {
            for (int i = 0; i < 3; i++)
            {
                track_set_clone = (GameObject)Instantiate(track_set, new Vector3(0, 0, offset), new Quaternion(0, 0, 0, 0));
                mountain_clone = (GameObject)Instantiate(mountains, new Vector3(22, 3.5f, mountainOffset), new Quaternion(0, 0, 0, 0));
                track_set_clone.tag = "clone_track";
                mountain_clone.tag = "mountains";
                //overhead_bar_clone = (GameObject)Instantiate(overhead_bars, new Vector3(0, 35.4f, overheadOffset), new Quaternion(0, 0, 0, 0));

                offset += 60f;
                mountainOffset += 85;
                Destroy(track_set_clone, prevDestroyTime + i*10);
                Destroy(mountain_clone, prevDestroyTime + i * 10);
                prevDestroyTime = prevDestroyTime + i * 10;

            }
            
           
        }
        if (other.tag.Equals("Coin"))
        {
            FindObjectOfType<GameManager>().IncrementCoinScore();
            Destroy(other.gameObject);
        }
        
        if (!other.tag.Equals("CreateTrack")  && !other.tag.Equals("PowerUp_1"))
        {
            CheckIfPass(other);
            Destroy(other.gameObject, 1f);
        }
        
        if (other.tag.Equals("PowerUp_1"))
        {
            FindObjectOfType<CameraController>().zoomOut = true;
            FindObjectOfType<GameManager>().ActivatePowerUp();
        }

    }
}
