using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 500;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (1 / Time.deltaTime).ToString("0");
    }
}
