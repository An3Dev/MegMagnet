using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    List<GameObject> spawnedBallsList = new List<GameObject>();
    public int startingBallNumber = 3;
    public GameObject ballPrefab;

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
        for(int i = 0; i < startingBallNumber; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            spawnedBallsList.Add(ball);
        }
    }

    public GameObject EnableBall()
    {
        for(int i = 0; i < spawnedBallsList.Count - 1; i++)
        {
            GameObject ball = spawnedBallsList[i];
            if (!ball.activeInHierarchy)
            {
                ball.SetActive(true);
                return ball;
            }
        }

        // There aren't any available balls so spawn one.
        GameObject spawnedBall = Instantiate(ballPrefab);
        spawnedBallsList.Add(spawnedBall);
        return spawnedBall;
    }

}
