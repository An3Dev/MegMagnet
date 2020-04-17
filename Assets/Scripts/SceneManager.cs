using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public UIAnimationManager uiAnimationManager;
    public static SceneManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }
}
