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

    public float obstacleZ = 90f;
    float[] distances = { 50f, 55f, 60f, 80f };

    float[] powerUpXPositions = { 2f, 0f, -2f };

    int oldObstacleColor = 0;

    public bool powerUpGenerated = false;

    public GameObject coins;

    private int prevTimeCoinSpawned = 5;


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
        int objectColorIndex = returnUniqueColor();
        GameObject obstacle = Instantiate(gameObjectArray[objectColorIndex], new Vector3(0, 1, z), new Quaternion(0, 0, 0, 0));
        if (objectColorIndex == 0)
        {
            obstacle.tag = "obstacle_clone_yellow";
        }
        else if (objectColorIndex == 1)
        {
            obstacle.tag = "obstacle_clone_red";
        }
        else if (objectColorIndex == 2)
        {
            obstacle.tag = "obstacle_clone_blue";
        }
    }
    public void CreateBlock(float z)
    {


        int objectColorIndex = returnUniqueColor();
        GameObject obstacle = Instantiate(gameObjectArray[objectColorIndex], new Vector3(0, 1, z), new Quaternion(0, 0, 0, 0));
        if (objectColorIndex == 0)
        {
            obstacle.tag = "obstacle_clone_yellow";
        }
        else if (objectColorIndex == 1)
        {
            obstacle.tag = "obstacle_clone_red";
        }
        else if (objectColorIndex == 2)
        {
            obstacle.tag = "obstacle_clone_blue";
        }
        if (Random.Range(0,50) == 4 && FindObjectOfType<GameManager>().PowerUpParent.activeSelf)
        {
            GameObject powerUpClone = Instantiate(PowerUp, new Vector3(powerUpXPositions[Random.Range(0, 3)], 2, z + 30), new Quaternion(-90f, 0, 0, 0));
            powerUpClone.tag = "powerup_clone";
            
        }
        if (Random.Range(0,10) == 5 || Random.Range(0,10) == 2)
        {
            int coinSpawnAmount = Random.Range(0, 10);
            int coinSpawnPosition = (int)powerUpXPositions[Random.Range(0, 3)];
            int coinSpawnZPosition = (int)z + 15;
            int coinSpawnOffset = 4;
            for (int i = 0; i < coinSpawnAmount; i++)
            {
                GameObject coinClone = Instantiate(coins, new Vector3(coinSpawnPosition, 2, coinSpawnZPosition + coinSpawnOffset), new Quaternion(0, 0, 0, 0));
                coinClone.tag = "coin_clone";
                coinSpawnOffset += 2;
                Destroy(coinClone,prevTimeCoinSpawned);
                prevTimeCoinSpawned += 5;
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
