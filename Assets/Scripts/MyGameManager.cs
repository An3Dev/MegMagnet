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
using UnityEngine.UDP;
using UnityEngine.Advertisements;
public class MyGameManager : MonoBehaviour, IUnityAdsListener
{

    public bool play = false;

    public bool gameOver = false;

    public static MyGameManager Instance;

    TextMeshProUGUI timer;

    public Light thisLight;

    public float timeScale;

    private const string playStoreID = "3706691";
    private const string rewardAdID = "rewardedVideo";
    UniversalRenderPipelineAsset currentPipelineAsset;

    public GameObject settingsPanel;

    public Slider renderScaleSlider;
    public Toggle shadowsToggle;
    public Toggle antiAliasingToggle;
    public Button lowPresetBtn, mediumPresetBtn, highPresetBtn;
    public Toggle postProcessingToggle;
    public GameObject fpsGameObject;
    public Toggle fpsToggle;

    public GameObject googlePlayImage;

    public GameObject continueButton;

    SROptions options;

    public bool showingAd = false;

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

    public TextMeshProUGUI googlePlayGameInfo;

    const string graphicsPresetName = "GraphicsPresets";
    const string renderScaleName = "RenderScale";
    const string castShadowsName = "CastShadows";
    const string antiAliasingName = "AntiAliasing";
    const string showFPSName = "ShowFPS";
    const string postProcessingName = "PostProcessing";

    public bool inSettings;

    public GameObject[] objectsToDisableInSettings;

    int totalMegs;
    const string totalMegsKey = "TotalMegsKey";

    int totalDoubleMegs;
    const string totalDoubleMegsKey = "DoubleMegs";

    public GameObject adButton;

    bool showedAdThisRound = false;

    public Image adButtonImage;
    float timeForContinueGame = 20;
    float adImageTimer = 0;

    private void OnEnable()
    {
        graphicsPreset = PlayerPrefs.GetString(graphicsPresetName, "Medium");

        renderScale = PlayerPrefs.GetFloat(renderScaleName, 1);
        castShadows = bool.Parse(PlayerPrefs.GetString(castShadowsName, "true"));
        antiAliasing = bool.Parse(PlayerPrefs.GetString(antiAliasingName, "false"));
        showFPS = bool.Parse(PlayerPrefs.GetString(showFPSName, "false"));
        postProcessing = bool.Parse(PlayerPrefs.GetString(postProcessingName, "false"));

        totalMegs = PlayerPrefs.GetInt(totalMegsKey, 0);
        totalDoubleMegs = PlayerPrefs.GetInt(totalDoubleMegsKey, 0);

        if (Application.platform == RuntimePlatform.Android)
        {
            if (PlayGamesPlatform.Instance == null)
            {
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
                PlayGamesPlatform.InitializeInstance(config);

                // recommended for debugging:
                PlayGamesPlatform.DebugLogEnabled = true;

                // Activate the Google Play Games platform
                PlayGamesPlatform.Activate();
            }


            Advertisement.Initialize(playStoreID);
            Advertisement.Load(rewardAdID);
            Advertisement.AddListener(this);

            if (!PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
                {
                    if (result == SignInStatus.Failed)
                    {
                        googlePlayGameInfo.text = "Sign-in failed";
                    }
                    else if (result == SignInStatus.Success)
                    {
                        googlePlayGameInfo.text = PlayGamesPlatform.Instance.localUser.userName;
                    }
                    else if (result == SignInStatus.NotAuthenticated)
                    {
                        googlePlayGameInfo.text = "Not Authenticated";
                    }
                    else if (result == SignInStatus.UiSignInRequired)
                    {

                        googlePlayGameInfo.text = "UI sign in required";
                    }
                    else if (result == SignInStatus.NetworkError)
                    {
                        googlePlayGameInfo.text = "Network error";
                    }
                    else
                    {
                        googlePlayGameInfo.text = result.ToString();
                    }
                    Debug.Log(result);
                });
            }
            else
            {
                googlePlayGameInfo.text = PlayGamesPlatform.Instance.localUser.userName;
            }
        }       

        Instance = this;
        mainCamera = Camera.main;
        cameraData = mainCamera.GetUniversalAdditionalCameraData();

        currentPipelineAsset = UniversalRenderPipeline.asset;

        if (graphicsPreset == "Low")
        {
            QualitySettings.SetQualityLevel(0);
        }
        else if (graphicsPreset == "Medium")
        {
            QualitySettings.SetQualityLevel(1);
        }
        else if (graphicsPreset == "High")
        {
            QualitySettings.SetQualityLevel(2);
        }

        currentPipelineAsset = UniversalRenderPipeline.asset;

        Debug.Log("Start");
        currentPipelineAsset.renderScale = renderScale;

        if (cameraData == null)
        {
            cameraData = mainCamera.GetUniversalAdditionalCameraData();

        }

        cameraData.renderShadows = castShadows;

        cameraData.renderPostProcessing = postProcessing;

        fpsGameObject.SetActive(showFPS);

        if (antiAliasing)
        {
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            cameraData.antialiasing = AntialiasingMode.None;
        }
    }

    #region Google Play
    public void GooglePlaySignIn()
    {
        //Social.localUser.Authenticate((bool success) =>
        //{
        //   if (success)
        //   {
        //       googlePlayGameInfo.text = Social.localUser.userName;
        //   } else
        //   {
        //       googlePlayGameInfo.text = "Failed";
        //   }
        //});
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
            {
                if (result == SignInStatus.Failed)
                {
                    googlePlayGameInfo.text = "Sign-in failed";
                }
                else if (result == SignInStatus.Success)
                {
                    googlePlayGameInfo.text = PlayGamesPlatform.Instance.localUser.userName;
                }
                else if (result == SignInStatus.NotAuthenticated)
                {
                    googlePlayGameInfo.text = "Not Authenticated";
                }
                else if (result == SignInStatus.UiSignInRequired)
                {

                    googlePlayGameInfo.text = "UI sign in required";
                }
                else if (result == SignInStatus.NetworkError)
                {
                    googlePlayGameInfo.text = "Network error";
                }
                else
                {
                    googlePlayGameInfo.text = result.ToString();
                }
                Debug.Log(result);
            });
        }  else
        {
            // sign out
            PlayGamesPlatform.Instance.SignOut();
            googlePlayGameInfo.text = "Signed out";

        }
    }
    #endregion GooglePlayGames

    public void GoToMenu()
    {
        if (scoreManager)
        {
            scoreManager.SaveScore();
        }
        Time.timeScale = 1;
        SavePrefs();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }


    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void IncreaseTotalMegsNum(bool doubleMeg)
    {
        totalMegs++;

        if (doubleMeg)
        {
            totalDoubleMegs++;
        }
    }

    public void OnClickRateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + "an3apps.megmadness");
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
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (Advertisement.IsReady(rewardAdID))
            {
                adImageTimer += Time.unscaledDeltaTime;
                adButtonImage.fillAmount = 1 - (adImageTimer / timeForContinueGame);
                if (adButtonImage.fillAmount <= 0)
                {
                    adButton.SetActive(false);
                }
                if (!showedAdThisRound)
                {
                    adButton.SetActive(true);
                }
                else
                {
                    adButton.SetActive(false);
                }
            } else
            {
                adButton.SetActive(false);
            }         

            gameOverUI.SetActive(true);
            continueButton.SetActive(false);
            if (scoreManager)
            {
                scoreManager.SaveScore();
            }
        }
    }

    public void ContinueGame()
    {
        play = true;
        gameOverUI.SetActive(false);
        inSettings = false;
        Time.timeScale = 1;
    }


    #region Ads
    public void ShowVideoAd()
    {
        
    }

    public void ShowRewardAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //Advertisement.AddListener();
            Advertisement.Show("rewardedVideo");
            showingAd = true;
            showedAdThisRound = true;
        }      


        gameOver = false;
        gameOverUI.SetActive(false);
        play = true;
        gameOverUI.SetActive(false);
        scoreManager.ResetTime();
        adImageTimer = 0;
        Time.timeScale = 0;
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Time.timeScale = 1;
        showingAd = false;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // mute Unity Audio
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch(showResult)
        {
            case ShowResult.Failed:
                break;

            case ShowResult.Finished:
                Time.timeScale = 1;
                showingAd = false;
                // unmute audio
                break;

            case ShowResult.Skipped:
                break;
        }
    }
    #endregion Ads


    #region Settings
    void UpdateSettingsUI()
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

        shadowsToggle.isOn = castShadows;

        postProcessingToggle.isOn = postProcessing;

        antiAliasingToggle.isOn = antiAliasing;

        fpsToggle.isOn = showFPS;
    }

    public void ToggleFPS(bool enable)
    {
        fpsGameObject.SetActive(enable);
        showFPS = enable;
    }
    public void ChangeRenderScale(float value)
    {
        renderScale = value;
        if (currentPipelineAsset == null)
        {
            currentPipelineAsset = UniversalRenderPipeline.asset;
        }
        currentPipelineAsset.renderScale = value;
    }

    public void ChangePresetSetting(int index)
    {
        QualitySettings.SetQualityLevel(index);
        if (currentPipelineAsset == null)
        {
            currentPipelineAsset = UniversalRenderPipeline.asset;
        }

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

        UpdateSettingsUI();
    }

    public void EnablePostProcessing(bool enable)
    {
        postProcessing = enable;
        if(cameraData == null)
        {
            cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        }
        cameraData.renderPostProcessing = postProcessing;
    }

    public void ToggleShadows(bool enable)
    {
        castShadows = enable;
        if (cameraData == null)
        {
            cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        }
        cameraData.renderShadows = enable;
    }

    public void ToggleAntiAliasing(bool enable)
    {
        antiAliasing = enable;
        if (cameraData == null)
        {
            cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        }
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
        UpdateSettingsUI();    
    }
   
    public void OpenSettings(bool enable)
    {
        // enable settings panel at start
        if (enable)
        {
            settingsPanel.SetActive(enable);
        }

        for (int i = 0; i < objectsToDisableInSettings.Length; i++)
        {
            objectsToDisableInSettings[i].SetActive(!enable);
        }

        inSettings = enable;
        if (enable)
        {
            Time.timeScale = 0.01f;
        }
        else
        {
            Time.timeScale = 1;
        }

        // if shop is active
        if (shopUI && shopUI.activeInHierarchy && enable)
        {
            shopUI.SetActive(false);
        }

        if (!enable)
        {
            PlayerPrefs.SetString(graphicsPresetName, graphicsPreset);

            PlayerPrefs.SetFloat(renderScaleName, renderScale);
            PlayerPrefs.SetString(castShadowsName, castShadows.ToString());
            PlayerPrefs.SetString(antiAliasingName, antiAliasing.ToString());
            PlayerPrefs.SetString(showFPSName, showFPS.ToString());
            PlayerPrefs.SetString(postProcessingName, postProcessing.ToString());

            PlayerPrefs.Save();
        }
        else
        {
            UpdateSettingsUI();
        }

        // disable settings panel at the end
        if (!enable)
        {
            settingsPanel.SetActive(enable);
        }
    }

    #endregion Settings


    public void StopGame()
    {
        play = false;
        gameOverUI.SetActive(true);
        inSettings = true;
        Time.timeScale = 0.01f;
        continueButton.SetActive(true);
        adButton.SetActive(false);
        if (scoreManager)
        {
            scoreManager.SaveScore();
        }
        SavePrefs();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt(totalMegsKey, totalMegs);
        PlayerPrefs.SetInt(totalDoubleMegsKey, totalDoubleMegs);
    }



    public void ShowLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_most_megs_in_a_round);
    }

    public void StartTime()
    {
        gameOverUI.SetActive(false);
        inSettings = false;
        play = true;
        Time.timeScale = 1;
        showedAdThisRound = false;
        adImageTimer = 0;
    }

    
}
