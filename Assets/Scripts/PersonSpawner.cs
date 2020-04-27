using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{

    public GameObject legPrefab;
    public Transform spawnerContainer;
    //public Transform[] spawnLocations;

    public float positionStandardDeviation = 1;

    public float spawnRate = 3;

    float lastSpawnTime;

    int clones;

    MyGameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<MyGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!gameManager.play && !gameManager.gameOver) return;

        if (Time.time - lastSpawnTime >= spawnRate)
        {
            int randomSpawnerIndex = Random.Range(0, spawnerContainer.childCount);
            Transform spawner = spawnerContainer.GetChild(randomSpawnerIndex);

            Vector3 position = new Vector3(Random.Range(spawner.position.x - positionStandardDeviation, 
                spawner.position.x + positionStandardDeviation), spawner.position.y, 
                    spawner.position.z);

            GameObject leg = Instantiate(legPrefab, position, spawner.rotation);
            leg.name = clones.ToString();
            Destroy(leg, 10);
            lastSpawnTime = Time.time;
            clones++;
            spawnRate = Random.Range(1.5f, 4);
        }
    }
}
