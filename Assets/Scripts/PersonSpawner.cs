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

    public List<Transform> personList;

    float doubleMegProbability = 5; // percent

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
            bool doubleMeg = Random.Range(0, 100) <= doubleMegProbability;

            int randomSpawnerIndex = Random.Range(0, spawnerContainer.childCount);
            Transform spawner = spawnerContainer.GetChild(randomSpawnerIndex);

            Vector3 position = new Vector3(Random.Range(spawner.position.x - positionStandardDeviation, 
                spawner.position.x), spawner.position.y, 
                    spawner.position.z);

            if (doubleMeg)
            {
                //Time.timeScale = 0.1f;
                for(int i = 0; i < 2; i++)
                {
                    GameObject person = objectPooler.EnablePerson();
                    Person personScript = person.GetComponent<Person>();

                    personScript.ForceAnimation(0);
                    personScript.involvedInDoubleMeg = true;
                    personScript.SetPerson();

                    // makes persons spawn in different spawners
                    spawner = spawnerContainer.GetChild(i);
                    
                    position = new Vector3(spawner.position.x + (spawner.position.x < 0 ? + 0.5f : - 0.5f), spawner.position.y, spawner.position.z);
                    
                    //Debug.Log("Double meg");

                    personList.Add(person.transform);

                    person.transform.position = position;
                    person.transform.rotation = spawner.rotation;
                    person.name = clones.ToString();

                    StartCoroutine(DisablePerson(person, 8));

                    lastSpawnTime = Time.time;
                    clones++;
                    spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
                }              
            } else
            {
                GameObject person = objectPooler.EnablePerson();
                Person personScript = person.GetComponent<Person>();

                personScript.SetPerson();

                personList.Add(person.transform);

                person.transform.position = position;
                person.transform.rotation = spawner.rotation;
                person.name = clones.ToString();
                person.GetComponent<Person>().involvedInDoubleMeg = false;

                StartCoroutine(DisablePerson(person, 6));

                lastSpawnTime = Time.time;
                clones++;
                spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
            }

            
        }
    }
    
    IEnumerator DisablePerson(GameObject person, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        personList.Remove(person.transform);
        person.SetActive(false);
    }
}
