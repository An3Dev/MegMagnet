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

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public void UpdateScore(MegType megType, int numOfMegs)
    {
        Debug.Log("Num of megs: " + numOfMegs);
        if (megType == MegType.Clean)
        {
            megScore += (long) (pointsPerMeg * ((numOfMegs - 1 > 0) ? ((numOfMegs - 1) * bonusAmount) : 1));
        }
        else if (megType == MegType.FewTouches)
        {
            megScore += Mathf.CeilToInt(pointsPerMeg * fewTouchReduction);
        }
        else if (megType == MegType.ManyTouches)
        {
            megScore += Mathf.CeilToInt(pointsPerMeg * manyTouchesReduction);
        }
        //megScore += pointsPerMeg;
        scoreText.text = "Score: " + megScore;
    }

}
