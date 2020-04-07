using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] gameObjectArray;
    float delay = 0.5f;

    public GameObject PowerUp;

    public Transform playerTransform;

    float obstacleZ = 90f;
    float[] distances = { 50f, 60f, 70f, 80f };

    float[] powerUpXPositions = { 2f, 0f, -2f };

    int oldObstacleColor = 0;

    public bool powerUpGenerated = false;

    public GameObject coins;


    void Start()
    {
        CreateFirstObstacle(obstacleZ);
        obstacleZ = obstacleZ + distances[Random.Range(0, 4)];
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > delay)
        {
            delay += 0.5f;
            CreateBlock(obstacleZ);
            obstacleZ = obstacleZ + distances[Random.Range(0, 4)];

        }
    }
    public void CreateFirstObstacle(float z)
    {
        GameObject obstacle = Instantiate(gameObjectArray[returnUniqueColor()], new Vector3(0, 1, z), new Quaternion(0, 0, 0, 0));
    }
    public void CreateBlock(float z)
    {



        GameObject obstacle = Instantiate(gameObjectArray[returnUniqueColor()], new Vector3(0, 1, z), new Quaternion(0, 0, 0, 0));
        if (Random.Range(0,20) == 4 && FindObjectOfType<GameManager>().PowerUp.activeSelf)
        {
            GameObject powerUpClone = Instantiate(PowerUp, new Vector3(powerUpXPositions[Random.Range(0, 3)], 2, z + 30), new Quaternion(-90f, 0, 0, 0));
            
        }
        if (true)
        {
            int coinSpawnAmount = 20; //Random.Range(0, 20);
            int coinSpawnPosition = (int)powerUpXPositions[Random.Range(0, 3)];
            int coinSpawnZPosition = (int)z + 15;
            int coinSpawnOffset = 2;
            for (int i = 0; i < coinSpawnAmount; i++)
            {
                GameObject coinClone = Instantiate(coins, new Vector3(coinSpawnPosition, 2, coinSpawnZPosition + coinSpawnOffset), new Quaternion(0, 0, 0, 0));
                coinSpawnOffset += 2;
            }
        }
        

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
    int returnUniqueColor()
    {
        int randomColor = Random.Range(0, 3);

        while (oldObstacleColor == randomColor)
        {
            randomColor = Random.Range(0, 3);
        }
        oldObstacleColor = randomColor;
        return randomColor;
    }
}
