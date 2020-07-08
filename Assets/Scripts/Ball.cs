using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
public class Ball : MonoBehaviour
{
    ScoreManager scoreManager;

    int numOfCollisions;
    int numOfMegs = 0;

    float timeBeforeDeath = 5;

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
        if (wasKicked)
        {
            if (transform.position.z < mainCamera.transform.position.z) {
                DisableBall();
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
        numOfMegs = 0;
        numOfCollisions = 0;
        gameObject.SetActive(false);
    }

    public void HandleMeg()
    {
        numOfMegs++;

        gameManager.IncreaseTotalMegsNum(numOfMegs > 1);

        if (numOfMegs == 2)
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) =>
            {
                foreach (IAchievement thisAchievement in achievements)
                {
                    if (thisAchievement.id == GPGSIds.achievement_double_meg && !thisAchievement.completed)
                    {
                        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement_double_meg);
                    }
                    break;
                }
            });
            
        }
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

        //scoreManager.ResetTime();

        megEnterTriggered = false;
        // Disable the meg collider
        //other.GetComponent<BoxCollider>().enabled = false;
        numOfCollisions = 0;

        //Debug.Break();
    }

    void ResetTimeScale()
    {
        gameManager.timeScale = 1;
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
