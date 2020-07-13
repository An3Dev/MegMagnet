using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        text.text = (1 / Time.deltaTime).ToString("0");
    }
}
