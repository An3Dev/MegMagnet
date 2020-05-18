using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    ObjectPooler objectPooler;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<MyGameManager>();
        objectPooler = ObjectPooler.Instance;
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

            //GameObject person = Instantiate(personPrefab, position, spawner.rotation);
            GameObject person = objectPooler.EnablePerson();
            person.transform.position = position;
            person.transform.rotation = spawner.rotation;
            person.name = clones.ToString();

            StartCoroutine(DisablePerson(person, 6));

            lastSpawnTime = Time.time;
            clones++;
            spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }
    
    IEnumerator DisablePerson(GameObject person, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        person.SetActive(false);
    }
}
