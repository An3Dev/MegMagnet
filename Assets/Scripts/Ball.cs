using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    ScoreManager scoreManager;

    int numOfCollisions;
    int numOfMegs = 0;

    private void Start()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        // if had contact with meg trigger
        if (other.CompareTag("MegTrigger"))
        {
            numOfMegs++;
            if (numOfCollisions == 0)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.Clean, numOfMegs);
            } else if (numOfCollisions > 0 && numOfCollisions <= 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.FewTouches, numOfMegs);
            }
            else if (numOfCollisions > 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.ManyTouches, numOfMegs);
            }

            // Disable the meg collider
            other.GetComponent<BoxCollider>().enabled = false;
            numOfCollisions = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if collided with legs.
        if (collision.collider.CompareTag("Legs"))
        {
            numOfCollisions++;
            Debug.Log("Collisions: " + numOfCollisions + " " + collision.collider.transform.root.name);
        }
    }
}
