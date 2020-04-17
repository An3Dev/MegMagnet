using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TypingSpeed : MonoBehaviour
{

    public TMP_InputField inputField;
    public TextMeshProUGUI wordsPerMinuteText;
    float startTypingTime = 0;

    bool startedTyping = false;

    string[] words;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputField.text != "" && !startedTyping)
        {
            startTypingTime = Time.time;
            startedTyping = true;
        }

        if (startedTyping)
        {
            float time = Time.time - startTypingTime;
            words = inputField.text.Split(' ');
            float wpm = words.Length / time * 60;
            wordsPerMinuteText.text = wpm.ToString();
        }

    }
}
