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

    bool megEnterTriggered = false;

    public float fadeDuration;

    bool fadeAway;
    float startTime;

    [HideInInspector]
    public Ball Instance;

    public Vector3 startScale;

    bool wasKicked = false;

    void Awake()
    {
        startScale = transform.localScale;
    }
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        material = GetComponent<MeshRenderer>().material;
        colorOfBall = material.color;
        Instance = this;
    }

    public void WasKicked()
    {
        startTime = Time.timeSinceLevelLoad;
        wasKicked = true;
    }

    public void PrepareForSpawn()
    {
        transform.localScale = startScale;
        numOfMegs = 0;
    }

    void CheckForFadeAway()
    {
        if (Time.timeSinceLevelLoad - startTime >= timeBeforeDeath)
        {
            fadeAway = true;
        }
    }

    void Update()
    {
        if (wasKicked)
        {
            CheckForFadeAway();
        }

        if (fadeAway)
        {

            float value = Time.deltaTime / fadeDuration;
            transform.localScale -= new Vector3(value, value, value);
            if (transform.localScale.x < 0.01f)
            {
                fadeAway = false;
                wasKicked = false;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("MegStart"))
        {
            megEnterTriggered = true;
            //Debug.Log("Start meg, MegParent: " + other.transform.root.name + "Meg position" + other.transform.position);
            fadeAway = false;
            startTime = Time.timeSinceLevelLoad;
        }

        // if had contact with meg trigger
        if (other.CompareTag("MegTrigger") && megEnterTriggered)
        {
            //Debug.Log("End meg, MegParent: " + other.transform.root.name + "Meg position" + other.transform.position);

            numOfMegs++;

            if (numOfCollisions == 0)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.Clean, numOfMegs, transform.position);
               

            }
            else if (numOfCollisions > 0 && numOfCollisions <= 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.FewTouches, numOfMegs, transform.position);
                

            }
            else if (numOfCollisions > 1)
            {
                scoreManager.UpdateScore(ScoreManager.MegType.ManyTouches, numOfMegs, transform.position);
                

            }
            

            megEnterTriggered = false;
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
