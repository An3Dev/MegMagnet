﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColorChange : MonoBehaviour
{

    public Color[] colors;

    int currentColorIndex = 1;

    float timeForColorChange = 0.4f;
    float timeBeforeColorChange = 0.5f;

    public MeshRenderer thisRenderer;

    float timer;
    bool changingColor = false;

    bool visible;
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer.materials[1].color = colors[Random.Range(0, colors.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBeforeColorChange && !changingColor)
        {
            timer = 0;
            StartCoroutine(ChangeColor(thisRenderer.materials[1].color, colors[currentColorIndex]));
            changingColor = true;
        }
    }

    IEnumerator ChangeColor(Color oldColor, Color targetColor)
    {
        targetColor = colors[currentColorIndex];
        float fraction = timer / timeForColorChange;
        thisRenderer.materials[1].color = Color.Lerp(oldColor, targetColor, fraction);
        changingColor = true;
        if (timer >= timeForColorChange)
        {
            thisRenderer.materials[1].color = targetColor;
            timer = 0;
            changingColor = false;
            if (currentColorIndex >= colors.Length - 1)
            {
                currentColorIndex = 0;
            } else
            {
                currentColorIndex++;
            }
            StopAllCoroutines();
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(ChangeColor(oldColor, targetColor));
    }

    private void OnDisable()
    {
        changingColor = false;
    }
}
