using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace An3Apps 
{
    public class Data : MonoBehaviour
    {
        private static int highScore;
        private static string highScoreKey = "HighScore";

        public static Data Instance;
        private void Awake()
        {
            Instance = this;
            highScore = GetHighScore();
        }

        public int GetHighScore()
        {
            //Debug.Log("Get high score = " + highScore);
            int highScore = PlayerPrefs.GetInt(highScoreKey, 0);
            return highScore;
        }

        public void SetHighScore(int value)
        {
            PlayerPrefs.SetInt(highScoreKey, value);
            highScore = value;
            //Debug.Log("set high score: " + highScore + " And " + GetHighScore());
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
