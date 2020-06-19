using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegDetection : MonoBehaviour
{

    Transform leftFoot, rightFoot;

    PersonSpawner personSpawnerScript;

    List<Transform> personList;

    Person personScript;
    Transform closestPerson;

    bool megStartedCloserToCamera = true;

    bool startedMeg = false;

    float megDistance = 0.5f;

    Vector3 megStartPos;

    Transform player;

    bool leftFootIsCloserToCam = true;

    Ball ballScript;
    private void Awake()
    {
        personSpawnerScript = FindObjectOfType<PersonSpawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ballScript = GetComponent<Ball>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        personList = personSpawnerScript.personList;

        // if there are persons spawned
        if (personList.Count > 0)
        {
            Transform tempClosestPerson = null;
            float closestDistanceSquared = 1000000;

            Vector3 ballPos = transform.position;

            // finds the closest person to the ball
            foreach(Transform person in personList)
            {
                if ((ballPos - person.position).sqrMagnitude < closestDistanceSquared)
                {
                    tempClosestPerson = person;
                    closestDistanceSquared = (ballPos - person.position).sqrMagnitude;
                }
            }
       
            // if the closest person changed
            if (tempClosestPerson != closestPerson)
            {
                closestPerson = tempClosestPerson;

                // assign feet
                personScript = closestPerson.transform.root.GetComponent<Person>();
                leftFoot = personScript.Instance.leftFoot;
                rightFoot = personScript.Instance.rightFoot;
            }
        
        
            // if close enough to the person 
            if (closestDistanceSquared <= megDistance * megDistance)
            {
                // if ball is in between feet and the ball hasn't started megging somebody
                if (((ballPos.x < leftFoot.position.x && ballPos.x > rightFoot.position.x) || (ballPos.x > leftFoot.position.x && ballPos.x < rightFoot.position.x)) && !startedMeg)
                {           
                    startedMeg = true;
                    megStartPos = ballPos;

                    // if ball is closer to the camera than the person.
                    if (ballPos.z < closestPerson.position.z)
                    {
                        megStartedCloserToCamera = true;
                    } else
                    {
                        megStartedCloserToCamera = false;
                    }

                    if (leftFoot.transform.position.z < rightFoot.transform.position.z)
                    {
                        leftFootIsCloserToCam = true;
                    } else
                    {
                        leftFootIsCloserToCam = false;
                    }
                    //// if the ball started on the left side of the person.
                    //if (ballPos.z - leftFoot.position.z < ballPos.z - rightFoot.position.z)
                    //{

                    //}
                    Debug.Log("started meg");
                }
            } else if (startedMeg)
            {
                startedMeg = false;

                Debug.Log("Stopped meg");
            }

            if (startedMeg)
            {
                // if person is facing left and the ball passed their right foot.
                if (megStartedCloserToCamera)
                {
                    if (leftFootIsCloserToCam && ballPos.z >= rightFoot.position.z)
                    {
                        ballScript.HandleMeg();
                        startedMeg = false;
                    } else if (!leftFootIsCloserToCam && ballPos.z >= leftFoot.position.z)
                    {
                        ballScript.HandleMeg();
                        startedMeg = false;
                    }
                } 
                // if the person is facing right and the ball passed their left foot
                else if (!megStartedCloserToCamera)
                {
                    if (leftFootIsCloserToCam && ballPos.z <= leftFoot.position.z)
                    {
                        ballScript.HandleMeg();
                        startedMeg = false;
                    }
                    else if (!leftFootIsCloserToCam && ballPos.z <= rightFoot.position.z)
                    {
                        ballScript.HandleMeg();
                        startedMeg = false;
                    }
                }
            }
        }

    }
}
