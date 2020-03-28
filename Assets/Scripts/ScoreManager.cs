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
    float fewTouchReduction = 0.75f;
    float manyTouchesReduction = 0.5f;
    public float bonusAmount = 2;

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public void UpdateScore(MegType megType, int numOfMegs)
    {
        if (megType == MegType.Clean)
        {
            megScore += Mathf.CeilToInt(pointsPerMeg * ((numOfMegs - 1) * bonusAmount));
        } else if (megType == MegType.FewTouches)
        {
            megScore += Mathf.CeilToInt(pointsPerMeg * fewTouchReduction);
        }
        else if (megType == MegType.ManyTouches)
        {
            megScore += Mathf.CeilToInt(pointsPerMeg * manyTouchesReduction);
        }
        scoreText.text = "Score: " + megScore;
        Debug.Log("Points: " + megScore);
    }

}
