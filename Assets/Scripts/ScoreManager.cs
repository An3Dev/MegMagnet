using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using An3Apps;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText, highScoreText;
    public float timeScale;

    int megScore;
    public enum MegType {Clean, FewTouches, ManyTouches};
    public int pointsPerMeg = 20;
    public float fewTouchReduction = 0.75f;
    public float manyTouchesReduction = 0.5f;
    public float bonusAmount = 2;

    public GameObject floatingTextPrefab;

    public static ScoreManager Instance;

    public Transform canvas;

    int startTime = 15;
    int startTimeDecrease = 1; // per meg

    float timeLeft = 10;

    MyGameManager gameManager;

    public TextMeshProUGUI timerText;

    public AnimationCurve curve;

    public float maxScaleOfTimer = 1.25f;

    bool scaledTimer = false;

    public AnimationCurve scaleCurve;

    public GameObject gameOverUI, gameplayUI, startUI;

    public GameObject gameOverHighScoreText;
    public TextMeshProUGUI gameOverScoreText;

    bool setGameOverScreen = false;

    An3Apps.Data data;

    float lastMegTime;
    bool lastDirectionWasLeft;

    int timesThatResetTime = 0;
    float minimumResetTime = 5;

    int currency;

    private void Awake()
    {
        gameManager = FindObjectOfType<MyGameManager>();
        data = transform.gameObject.AddComponent<An3Apps.Data>();
        currency = PlayerPrefs.GetInt(ShopScript.playerCurrencyKey, 25);
    }

    void Start()
    {
        timeLeft = startTime;

        Instance = this;
        canvas = GameObject.Find("Canvas").transform;
        timerText.text = startTime.ToString();

        ResetGame();
    }

    public void SetGameOverScreenUI ()
    {
        // if player beat their high score
        if (megScore > data.GetHighScore())
        {
            data.SetHighScore(megScore);

            gameOverHighScoreText.SetActive(true);
        }
        else
        {
            gameOverHighScoreText.SetActive(false);
        }

        // show score
        gameOverScoreText.text = megScore.ToString();
        setGameOverScreen = true;
    }

    private void Update()
    {

        if (gameManager.gameOver && !setGameOverScreen)
        {
            //gameOverUI.SetActive(true);
            //gameplayUI.SetActive(false);
            //startUI.SetActive(false);
            SetGameOverScreenUI();
            PlayerPrefs.SetInt(ShopScript.playerCurrencyKey, currency);
        }
        // continuously update just in case a ball goes through legs while in the game over screen
        if (gameManager.gameOver || !gameManager.play)
        {
            gameOverScoreText.text = megScore.ToString();
        }

        if (!gameManager.play) return;

        timeLeft -= Time.deltaTime;

        //float number = Mathf.Round(timeLeft * 100f) / 100f % 1;

        // this is true every second.
        if (timeLeft % 1 < Time.deltaTime + 0.01f && !scaledTimer)
        {
            scaledTimer = true;
            timerText.transform.localScale = Vector3.one;
            float scale = 1.05f + (maxScaleOfTimer - 1) * scaleCurve.Evaluate((startTime - timeLeft) / startTime);
            PlayTimerAnimation(new Vector3(scale, scale, scale));
        } else if (scaledTimer)
        {
            scaledTimer = false;
        }

        if (timeLeft <= 0) {
            gameManager.play = false;
            gameManager.gameOver = true;
        }

        if (timeLeft > 10)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
        } else
        {
            if (timeLeft > 0)
            {
                timerText.text = timeLeft.ToString("0.0");
            } else
            {
                timerText.text = "0";
            }
        }
    }

    public void StartGame()
    {
        gameplayUI.SetActive(true);
        startUI.SetActive(false);
        gameOverUI.SetActive(false);

        ResetGame();
    }

    public void ResetGame()
    {
        timeLeft = startTime;
        gameManager.gameOver = false;
        gameManager.play = true;

        scoreText.gameObject.LeanCancel();
        timerText.transform.localScale = new Vector3(1, 1, 1);

        scoreText.text = "0";
        scaledTimer = false;
        // reset the score
        megScore = 0;

        timesThatResetTime = 0;
        highScoreText.text = "Best: " + data.GetHighScore().ToString();

        setGameOverScreen = false;
    }

    public void ResetTime()
    {
        bool doubleMeg = false;
        if(Time.time - lastMegTime < 0.2f)
        {
            doubleMeg = true;
        }

        if (doubleMeg)
        {
            timesThatResetTime -= 2; // minus two because the first meg increased this value by one
        } else
        {
            timesThatResetTime++;
        }

        timeLeft = startTime - timesThatResetTime * startTimeDecrease /*((timesThatResetTime * startTimeDecrease) >= minimumResetTime ? (timesThatResetTime * startTimeDecrease) : minimumResetTime)*/;
        timeLeft = Mathf.Clamp(timeLeft, minimumResetTime, 20);
    }

    void PlayTimerAnimation(Vector3 scale)
    {
        //Debug.Log("Current scale: " + timerText.transform.localScale.ToString() + "DesiredScale: " + scale);
        timerText.gameObject.LeanScale(scale, 0.6f).setEase(curve);
    }

    public void UpdateScore(MegType megType, int numOfMegs, Vector3 ballPos)
    {
        int valueAdded = 0;

        ResetTime();

        if (megType == MegType.Clean)
        {
            valueAdded = (int)(pointsPerMeg * ((numOfMegs - 1 > 0) ? ((numOfMegs - 1) * bonusAmount) : 1));
        }
        else if (megType == MegType.FewTouches)
        {
            valueAdded = Mathf.CeilToInt(pointsPerMeg * fewTouchReduction);
        }
        else if (megType == MegType.ManyTouches)
        {
            valueAdded = Mathf.CeilToInt(pointsPerMeg * manyTouchesReduction);
        }

        currency += valueAdded;

        megScore += valueAdded;
        ShowFloatingText(ballPos, valueAdded);

        //megScore += pointsPerMeg;
        scoreText.text = megScore.ToString();
    }

    void ShowFloatingText(Vector3 ballPos, float pointsAdded)
    {
        //float textSize = 1 - ((Camera.main.transform.position - ballPos).magnitude / 10);

        //textSize = Mathf.Clamp(textSize, 0.7f, 1f);
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(ballPos + Vector3.right / 8);

        GameObject text = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, canvas);


        if (Time.time - lastMegTime < 0.2f)
        {
            if (!lastDirectionWasLeft)
            {
                text.GetComponent<Animator>().SetTrigger("Left");
                lastDirectionWasLeft = true;
            } else
            {
                lastDirectionWasLeft = false;
            }
        } else
        {
            if (spawnPos.x > Screen.width / 2)
            {
                // spawn text that moves right
                //SceneManager.Instance.uiAnimationManager.PointsAnimation(true, text);
                text.GetComponent<Animator>().SetTrigger("Left");
                lastDirectionWasLeft = true;
            }
            else
            {
                lastDirectionWasLeft = false;
            }
        } 


        text.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        text.GetComponentInChildren<TextMeshProUGUI>().text = "+" + pointsAdded.ToString();
        Destroy(text, 5);
        lastMegTime = Time.time;
    }

}
