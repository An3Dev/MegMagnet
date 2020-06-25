using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;
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
    public GameObject fpsGameObject;

    SROptions options;

    float renderScale;
    bool castShadows;
    bool antiAliasing = true;
    string graphicsPreset;
    bool showFPS = false;

    Camera mainCamera;
    UniversalAdditionalCameraData cameraData;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        mainCamera = Camera.main;
        cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        currentPipelineAsset = UniversalRenderPipeline.asset;

        graphicsPreset = PlayerPrefs.GetString("GraphicsPresets", "High");

        renderScale = PlayerPrefs.GetFloat("RenderScale", 1);
        castShadows = bool.Parse(PlayerPrefs.GetString("CastShadows", "true"));
        antiAliasing = bool.Parse(PlayerPrefs.GetString("AntiAliasing", "true"));
        showFPS = bool.Parse(PlayerPrefs.GetString("ShowFPS", "false"));

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

        if (antiAliasing)
        {
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            cameraData.antialiasing = AntialiasingMode.None;
        }
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

        antiAliasingToggle.isOn = antiAliasing;

        fpsGameObject.SetActive(showFPS);
    }

    public void ToggleFPS(bool enable)
    {
        fpsGameObject.SetActive(enable);
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

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        UpdateUI();

    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        PlayerPrefs.SetString("GraphicsPresets", graphicsPreset);

        PlayerPrefs.SetFloat("RenderScale", renderScale);
        PlayerPrefs.SetString("CastShadows", castShadows.ToString());
        PlayerPrefs.SetString("AntiAliasing", antiAliasing.ToString());
        PlayerPrefs.SetString("ShowFPS", showFPS.ToString());
    }

    public void StartTime()
    {
        play = true;
    }
}
