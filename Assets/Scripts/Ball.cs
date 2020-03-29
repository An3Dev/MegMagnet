using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    ScoreManager scoreManager;

    int numOfCollisions;
    int numOfMegs = 0;

    public float timeBeforeDeath = 10;
    Material material;
    Color colorOfBall;

    public float fadeDuration;
    private void Start()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        material = GetComponent<MeshRenderer>().material;
        colorOfBall = material.color;

        StartCoroutine(DissolveBall());
    }

    IEnumerator DissolveBall()
    {
        yield return new WaitForSeconds(timeBeforeDeath);

        Debug.Log("Start dissolve");
        //float step = 255 / fadeDuration;
        Color color = colorOfBall;
        for (float a = 255; a >= 0; a -= 5)
        {
            Debug.Log("Alpha: " + a);
            Debug.Log(material.color);
            color.a = a;
            material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

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
                Debug.Log("0 Collisions");
            }
            else if (numOfCollisions > 0 && numOfCollisions <= 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.FewTouches, numOfMegs);
                Debug.Log("1 Touch");

            }
            else if (numOfCollisions > 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.ManyTouches, numOfMegs);
                Debug.Log("Multiple touches");

            }

            // Disable the meg collider
            //other.GetComponent<BoxCollider>().enabled = false;
            numOfCollisions = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if collided with legs.
        if (collision.collider.CompareTag("Legs"))
        {
            numOfCollisions++;
            //Debug.Log("Collisions: " + numOfCollisions + " " + collision.collider.transform.root.name);
        }
    }
}
