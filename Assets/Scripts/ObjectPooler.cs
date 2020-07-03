using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    List<GameObject> spawnedBallsList = new List<GameObject>();
    public int startingBallNumber = 3;
    public GameObject ballPrefab;

    List<GameObject> spawnedPersonsList = new List<GameObject>();
    int startingPersonsNumber = 1;
    public GameObject personPrefab;

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;

        ballPrefab = Resources.Load("Balls/" + PlayerPrefs.GetString(ShopScript.equippedBallKey, "Classic").ToString()) as GameObject;

        for (int i = 0; i < startingBallNumber; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            spawnedBallsList.Add(ball);
        }

        for (int i = 0; i < startingPersonsNumber; i++)
        {
            GameObject person = Instantiate(personPrefab);
            person.SetActive(false);
            spawnedPersonsList.Add(person);
        }
    }

    public GameObject EnableBall()
    {
        for(int i = 0; i < spawnedBallsList.Count - 1; i++)
        {
            GameObject ball = spawnedBallsList[i];
            if (!ball.activeInHierarchy)
            {
                ball.GetComponent<Ball>().PrepareForSpawn();
                ball.SetActive(true);
                return ball;
            }
        }

        // There aren't any available balls so spawn one.
        GameObject spawnedBall = Instantiate(ballPrefab);
        spawnedBallsList.Add(spawnedBall);
        return spawnedBall;
    }

    public GameObject EnablePerson()
    {
        for (int i = 0; i < spawnedPersonsList.Count - 1; i++)
        {
            GameObject person = spawnedPersonsList[i];
            if (!person.activeInHierarchy)
            {
                person.SetActive(true);
                return person;
            }
        }

        // There aren't any available balls so spawn one.
        GameObject spawnedPerson = Instantiate(personPrefab);
        spawnedPersonsList.Add(spawnedPerson);
        return spawnedPerson;
    }

}
