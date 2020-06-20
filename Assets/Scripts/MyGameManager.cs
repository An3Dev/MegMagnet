using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MyGameManager : MonoBehaviour
{

    public bool play = false;

    public bool gameOver = false;

    public static MyGameManager Instance;

    TextMeshProUGUI timer;

    public float timeScale;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = timeScale;
        if (!play)
        {
            if (Input.GetMouseButton(0))
            {
                play = true;
            }
            return;
        }
    }

    public void StartTime()
    {
        play = true;
    }
}
