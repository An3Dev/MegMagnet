using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{

    public GameObject personPrefab;
    public Transform spawnerContainer;
    //public Transform[] spawnLocations;

    public float positionStandardDeviation = 1;

    public float spawnRate = 0.5f;

    float lastSpawnTime;

    public float minSpawnRate, maxSpawnRate;

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

            GameObject person = Instantiate(personPrefab, position, spawner.rotation);
            person.name = clones.ToString();
            Destroy(person, 10);
            lastSpawnTime = Time.time;
            clones++;
            spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }
}
