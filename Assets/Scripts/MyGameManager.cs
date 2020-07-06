using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;
using UnityEngine.Rendering;
using GooglePlayGames.Android;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
public class MyGameManager : MonoBehaviour
{

    public bool play = false;

    public bool gameOver = false;

    public static MyGameManager Instance;

    TextMeshProUGUI timer;

    public Light thisLight;

    public float timeScale;

    UniversalRenderPipelineAsset currentPipelineAsset;

    public GameObject settingsPanel;

    public Slider renderScaleSlider;
    public Toggle shadowsToggle;
    public Toggle antiAliasingToggle;
    public Button lowPresetBtn, mediumPresetBtn, highPresetBtn;
    public Toggle postProcessingToggle;
    public GameObject fpsGameObject;

    public GameObject continueButton;

    SROptions options;

    float renderScale;
    bool castShadows;
    bool antiAliasing = true;
    string graphicsPreset;
    bool postProcessing;
    bool showFPS = false;

    Camera mainCamera;
    UniversalAdditionalCameraData cameraData;
    public ScoreManager scoreManager;

    bool inShop = false;

    public GameObject shopUI;

    public GameObject gameOverUI;

    const string graphicsPresetName = "GraphicsPresets";
    const string renderScaleName = "RenderScale";
    const string castShadowsName = "CastShadows";
    const string antiAliasingName = "AntiAliasing";
    const string showFPSName = "ShowFPS";
    const string postProcessingName = "PostProcessing";

    public bool inSettings;

    public GameObject[] objectsToDisableInSettings;

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        Instance = this;
        mainCamera = Camera.main;
        cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        currentPipelineAsset = UniversalRenderPipeline.asset;

        graphicsPreset = PlayerPrefs.GetString(graphicsPresetName, "High");

        renderScale = PlayerPrefs.GetFloat(renderScaleName, 1);
        castShadows = bool.Parse(PlayerPrefs.GetString(castShadowsName, "true"));
        antiAliasing = bool.Parse(PlayerPrefs.GetString(antiAliasingName, "true"));
        showFPS = bool.Parse(PlayerPrefs.GetString(showFPSName, "false"));
        postProcessing = bool.Parse(PlayerPrefs.GetString(postProcessingName, "true"));

        if (graphicsPreset == "Low")
        {
            QualitySettings.SetQualityLevel(0);
        } else if (graphicsPreset == "Medium")
        {
            QualitySettings.SetQualityLevel(1);
        } else if (graphicsPreset == "High")
        {
            QualitySettings.SetQualityLevel(2);
        }

        currentPipelineAsset = UniversalRenderPipeline.asset;

        currentPipelineAsset.renderScale = renderScale;

        cameraData.renderShadows = castShadows;

        cameraData.renderPostProcessing = postProcessing;

        if (antiAliasing)
        {
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            cameraData.antialiasing = AntialiasingMode.None;
        }
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void EnablePostProcessing(bool enable)
    {
        postProcessing = enable;
        cameraData.renderPostProcessing = postProcessing;
    }

    public void EnableShopUI(bool enable)
    {
        if (scoreManager)
            scoreManager.ResetGame();
        //play = !enable;
        //shopUI.SetActive(enable);
        //gameOverUI.SetActive(!enable);
        //scoreManager.SetGameOverScreenUI();

        // go to shop screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("Shop");
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    void UpdateUI()
    {
        if (graphicsPreset == "Low")
        {
            lowPresetBtn.Select();
        }
        else if (graphicsPreset == "Medium")
        {
            mediumPresetBtn.Select();
        }
        else if (graphicsPreset == "High")
        {
            highPresetBtn.Select();
        }

        renderScaleSlider.value = currentPipelineAsset.renderScale;
        if (currentPipelineAsset.shadowDistance > 0)
        {
            shadowsToggle.isOn = true;
        } else
        {
            shadowsToggle.isOn = false;
        }

        cameraData.renderPostProcessing = postProcessing;
        antiAliasingToggle.isOn = antiAliasing;

        fpsGameObject.SetActive(showFPS);
    }

    public void ToggleFPS(bool enable)
    {
        fpsGameObject.SetActive(enable);
        showFPS = enable;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            gameOverUI.SetActive(true);
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        play = true;
        gameOverUI.SetActive(false);
        inSettings = false;
        Time.timeScale = 1;
    }

    public void ChangeRenderScale(float value)
    {
        renderScale = value;
        currentPipelineAsset.renderScale = value;
    }

    public void ChangePresetSetting(int index)
    {
        QualitySettings.SetQualityLevel(index);
        currentPipelineAsset = UniversalRenderPipeline.asset;

        if (index == 0)
        {
            graphicsPreset = "Low";
        } else if (index == 1)
        {
            graphicsPreset = "Medium";
        } else if (index == 2)
        {
            graphicsPreset = "High";
        }

        UpdateUI();
    }

    public void ToggleShadows(bool enable)
    {
        castShadows = enable;
        cameraData.renderShadows = enable;
    }

    public void ToggleAntiAliasing(bool enable)
    {
        antiAliasing = enable;
        if (enable)
        {
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            cameraData.antialiasing = AntialiasingMode.None;
        }
    }

    public void ChangeShadowResolution(int res)
    {
        ShadowResolution resolution = (ShadowResolution) Enum.Parse(Type.GetType(typeof(ShadowResolution).Name), "test");
        options.MainLightShadowResolution = resolution;
        UpdateUI();    
    }

    public void StopGame()
    {
        play = false;
        gameOverUI.SetActive(true);
        inSettings = true;
        continueButton.SetActive(true);

    }

    public void OpenSettings(bool enable)
    {
        settingsPanel.SetActive(enable);



        for(int i = 0; i < objectsToDisableInSettings.Length; i++)
        {
            objectsToDisableInSettings[i].SetActive(!enable);
        }
        
        inSettings = enable;
        if (enable)
        {
            Time.timeScale = 0.01f;
        } else
        {
            Time.timeScale = 1;
        }

        // if shop is active
        if (shopUI.activeInHierarchy && enable)
        {
            shopUI.SetActive(false);
        }

        if (!enable)
        {
            PlayerPrefs.SetString("GraphicsPresets", graphicsPreset);

            PlayerPrefs.SetFloat(renderScaleName, renderScale);
            PlayerPrefs.SetString(castShadowsName, castShadows.ToString());
            PlayerPrefs.SetString(antiAliasingName, antiAliasing.ToString());
            PlayerPrefs.SetString(showFPSName, showFPS.ToString());
            PlayerPrefs.SetString(postProcessingName, postProcessing.ToString());
        } else
        {
            UpdateUI();
        }     
    }

    public void StartTime()
    {
        play = true;
    }
}
