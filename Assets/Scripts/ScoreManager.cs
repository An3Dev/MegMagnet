using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public float timeScale;

    Transform lastBallThatMegged = null;

    long megScore;
    public enum MegType {Clean, FewTouches, ManyTouches};
    int pointsPerMeg = 20;
    public float fewTouchReduction = 0.75f;
    public float manyTouchesReduction = 0.5f;
    public float bonusAmount = 2;

    public GameObject floatingTextPrefab;

    public static ScoreManager Instance;

    public Transform canvas;

    void Start()
    {
        Instance = this;
        canvas = GameObject.Find("Canvas").transform;
    }
    private void Update()
    {
        Time.timeScale = timeScale;

    }


    public void UpdateScore(MegType megType, int numOfMegs, Vector3 ballPos)
    {
        long valueAdded = 0;
         
        if (megType == MegType.Clean)
        {
            valueAdded = (long)(pointsPerMeg * ((numOfMegs - 1 > 0) ? ((numOfMegs - 1) * bonusAmount) : 1));
        }
        else if (megType == MegType.FewTouches)
        {
            valueAdded = Mathf.CeilToInt(pointsPerMeg * fewTouchReduction);
        }
        else if (megType == MegType.ManyTouches)
        {
            valueAdded = Mathf.CeilToInt(pointsPerMeg * manyTouchesReduction);
        }

        megScore += valueAdded;
        ShowFloatingText(ballPos, valueAdded);

        //megScore += pointsPerMeg;
        scoreText.text = "Score: " + megScore;
        Debug.Log("Score: " + megScore);
    }


    void ShowFloatingText(Vector3 ballPos, float pointsAdded)
    {
        float textSize = 1 - ((Camera.main.transform.position - ballPos).magnitude / 10);

        textSize = Mathf.Clamp(textSize, 0.7f, 1f);
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(ballPos + Vector3.right / 8); 

        GameObject text = Instantiate(floatingTextPrefab, spawnPos , Quaternion.identity, canvas);

        if (spawnPos.x > Screen.width / 2)
        {
            // spawn text that moves right
            text.GetComponent<Animator>().SetTrigger("Left");
        }

        text.transform.localScale = new Vector3(textSize, textSize, 1);

        text.GetComponentInChildren<TextMeshProUGUI>().text = "+" + pointsAdded.ToString();
        Destroy(text, 5);
    }

}
