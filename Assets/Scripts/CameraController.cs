using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;

    public GameObject CameraObject;

    //Camera Offset
    public float x_camera_offset;
    public float y_camera_offset; //4
    public float z_camera_offset; //7

    public GameManager game_manager;
    public bool animationOver = false;

    public bool zoomOut = false;
    public bool zoomOutDone = false;

    private float duration = 1.3f;
    private float zoomedInOffset = -7;
    private float zoomedOutOffset = -13;
    private float currOffset = -7;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    public GameObject BGMountain;
    public float mountainZoffset = -89.4f;


    void Start()
    {
        CameraObject.GetComponent<Animator>().enabled = false;
        //Invoke("DisableAnimation", 3f);
    }

    // Update is called once per frame
    public void StartCamera()
    {
        CameraObject.GetComponent<Animator>().enabled = true;
        Invoke("DisableAnimation", 3f);
    }
    void Update()
    {

        mountainZoffset -= 0.0001f * Time.deltaTime;
        AdjustToOffset();
        if (zoomOut)
        {
            if (currOffset > zoomedOutOffset)
            {
                duration -= Time.deltaTime;
                z_camera_offset = z_camera_offset - 0.1f;
                currOffset -= 0.1f;
            }
            else
            {
                zoomOut = false;
                duration = 1.3f;

            }
        }
        if (zoomOutDone)
        {
            if (zoomedInOffset > currOffset)
            {
                duration -= Time.deltaTime;
                z_camera_offset = z_camera_offset + 0.1f;
                currOffset += 0.1f;
            }
            else
            {
                zoomOutDone = false;

            }
        }
        
    }
    
    private void FixedUpdate()
    {
        //Vector3 target = cameraTransform.position + new Vector3(-162.7f, -99.7f, mountainZoffset);
        //BGMountain.transform.position =  Vector3.SmoothDamp(BGMountain.transform.localPosition, new Vector3(-162.7f, -99.7f, cameraTransform.position.z + mountainZoffset), ref velocity, 0.1f);
    }

    void AdjustToOffset()
    {
        
        cameraTransform.position = playerTransform.position + new Vector3(x_camera_offset, y_camera_offset, z_camera_offset);
       
        
    }
    public void DisableAnimation()
    {
        CameraObject.GetComponent<Animator>().enabled = false;
        FindObjectOfType<GameManager>().GoButtonObject.SetActive(true);
        animationOver = true;
    }
    public void CorrectRotation()
    {
        CameraObject.transform.Rotate(100, 0, 0);

    }
}
