using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    ScoreManager scoreManager;

    int numOfCollisions;
    int numOfMegs = 0;

    public float timeBeforeDeath = 10;

    bool megEnterTriggered = false;

    public float fadeDuration;

    bool fadeAway;
    float startTime;

    [HideInInspector]
    public Ball Instance;

    public Vector3 startScale;

    bool wasKicked = false;

    bool isCheckingForMeg = false;

    Transform leftFoot, rightFoot;
    Person person;

    Vector3 initialMegPosition;

    bool isMegExitOnFarSideOfPlayer;
    bool isPersonFacingRight;
    bool wasExactlyBetweenLegs;

    public GameObject trail;

    MyGameManager gameManager;

    Camera mainCamera;
    void Awake()
    {
        startScale = transform.localScale;
        gameManager = FindObjectOfType<MyGameManager>();
    }
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        Instance = this;
        mainCamera = Camera.main;
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
        if (!gameManager.play)
        {          
            return;
        }

        if (wasKicked)
        {
            if (transform.position.z < mainCamera.transform.position.z) {
                DisableBall();
                Debug.Log("Disable");
            }
        }
        if (isCheckingForMeg)
        {
            // if transform was destroyed stop checking for megs.
            if (leftFoot == null || rightFoot == null || person == null)
            {
                isCheckingForMeg = false;
                return;
            }
            if (!wasExactlyBetweenLegs)
            {
                // Debug.Log("Left foot: " + leftFoot.position + " Right foot: " + rightFoot.position);
                Vector3 ballPos = transform.position;
                Vector3 rightFootPos = rightFoot.position;
                Vector3 leftFootPos = leftFoot.position;

                // if ball is between both feet on the x axis
                if (ballPos.x < rightFootPos.x && ballPos.x > leftFootPos.x ||
                        ballPos.x > rightFootPos.x && ballPos.x < leftFootPos.x)
                {
                    //Debug.Log("Between both legs");
                    // if player is facing right and meg should go away from camera and ball is between the legs on the z axis
                    if (isMegExitOnFarSideOfPlayer && isPersonFacingRight && 
                        ballPos.z >= rightFootPos.z + ((leftFootPos.z - rightFootPos.z) / 2) 
                            || !isMegExitOnFarSideOfPlayer && isPersonFacingRight
                                && ballPos.z <= rightFootPos.z + ((leftFootPos.z - rightFootPos.z) / 2))
                    {
                        wasExactlyBetweenLegs = true;
                        Debug.Log("Exactly between!");
                    } 
                    else if (isMegExitOnFarSideOfPlayer && !isPersonFacingRight
                        && ballPos.z >= leftFootPos.z + ((rightFootPos.z - leftFootPos.z) / 2)
                            || !isMegExitOnFarSideOfPlayer && !isPersonFacingRight
                                && ballPos.z <= leftFootPos.z + ((rightFootPos.z - leftFootPos.z) / 2)) 
                    {
                        wasExactlyBetweenLegs = true;
                        Debug.Log("Exactly between and person facing left");
                    }
                }
            }   
            else // if ball was between the legs already
            {
                Vector3 ballPos = transform.position;
                Vector3 rightFootPos = rightFoot.position;
                Vector3 leftFootPos = leftFoot.position;
                
                // If this is true, the ball is past the exit foot
                if (isMegExitOnFarSideOfPlayer && isPersonFacingRight && ballPos.z >= leftFootPos.z
                     || !isMegExitOnFarSideOfPlayer && isPersonFacingRight && ballPos.z <= rightFootPos.z
                     || isMegExitOnFarSideOfPlayer && !isPersonFacingRight && ballPos.z >= rightFootPos.z
                     || !isMegExitOnFarSideOfPlayer && !isPersonFacingRight && ballPos.z <= leftFootPos.z)
                {
                    // if the ball is within the feet on the x axis
                    if (ballPos.x < rightFootPos.x && ballPos.x > leftFootPos.x ||
                        ballPos.x > rightFootPos.x && ballPos.x < leftFootPos.x)
                    {
                        HandleMeg();
                        isCheckingForMeg = false;
                    }                        
                }
            }
        }

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
                DisableBall();
            }
        }
    }

    void DisableBall()
    {
        fadeAway = false;
        wasKicked = false;
        gameObject.SetActive(false);
    }

    private void HandleMeg()
    {
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

        //Debug.Break();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("MegStart"))
        {
            isCheckingForMeg = true;
            person = other.transform.root.GetComponent<Person>();
            leftFoot = person.Instance.leftFoot;
            rightFoot = person.Instance.rightFoot;

            initialMegPosition = transform.position;

            Vector3 rightFootPos = rightFoot.position;
            Vector3 leftFootPos = leftFoot.position;

            // if person is facing right
            if (leftFootPos.z > rightFootPos.z)
            {
                isPersonFacingRight = true;
                // if ball started on right side of person
                if (initialMegPosition.z < rightFoot.root.transform.position.z)
                {
                    isMegExitOnFarSideOfPlayer = true;
                }
                else
                {
                    isMegExitOnFarSideOfPlayer = false;
                }
            }
            else
            {
                isPersonFacingRight = false;

                // if ball started on right side of person
                if (initialMegPosition.z > rightFoot.root.transform.position.z)
                {
                    isMegExitOnFarSideOfPlayer = false;
                }
                else
                {
                    isMegExitOnFarSideOfPlayer = true;
                }
            }

            // cancel the fade away
            fadeAway = false;
            startTime = Time.timeSinceLevelLoad;

            numOfCollisions = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MegStart"))
        {
            isCheckingForMeg = false;
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
