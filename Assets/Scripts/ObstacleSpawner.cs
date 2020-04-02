using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] gameObjectArray;
    int delay = 1;

    public Transform playerTransform;

    float obstacleZ = 90f;
    float[] distances = { 50f, 60f, 70f, 80f };


    void Start()
    {
        CreateBlock(obstacleZ);
        obstacleZ = obstacleZ + distances[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Time.time > delay)
        {
            delay += 1;
            CreateBlock(obstacleZ);
            obstacleZ = obstacleZ + distances[Random.Range(0, 3)];
            
        }
    }

    public void CreateBlock(float z)
    {



        GameObject obstacle = Instantiate(gameObjectArray[Random.Range(0,3)], new Vector3(0, 1, z), new Quaternion(0, 0, 0,0));
       
        

    }

    public bool randomChoose()
    {
        int randomValue = Random.Range(0, 5);
        if (randomValue == 1) 
        {
            return true;
        }
        return false;
    }
}
