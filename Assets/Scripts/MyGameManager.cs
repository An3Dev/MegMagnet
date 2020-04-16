using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MyGameManager : MonoBehaviour
{

    public bool play = false;

    public static MyGameManager Instance;

    TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
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
