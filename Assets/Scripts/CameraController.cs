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

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisableAnimation", 2f);
    }

    // Update is called once per frame
    void Update()
    {

            
            AdjustToOffset();
        
        
    }

    void AdjustToOffset()
    {
        cameraTransform.position = playerTransform.position + new Vector3(x_camera_offset, y_camera_offset, z_camera_offset);
        
    }
    void DisableAnimation()
    {
        CameraObject.GetComponent<Animator>().enabled = false;
    }
}
