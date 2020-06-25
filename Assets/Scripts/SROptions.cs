using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

public partial class SROptions
{
    private System.Type universalRenderPipelineAssetType;
    private FieldInfo mainLightShadowmapResolutionFieldInfo;

    private void InitializeShadowMapFieldInfo()
    {
        universalRenderPipelineAssetType = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetType();
        mainLightShadowmapResolutionFieldInfo = universalRenderPipelineAssetType.GetField("m_MainLightShadowmapResolution", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public ShadowResolution MainLightShadowResolution
    {
        get
        {
            if (mainLightShadowmapResolutionFieldInfo == null)
            {
                InitializeShadowMapFieldInfo();
            }
            return (ShadowResolution)mainLightShadowmapResolutionFieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
        }
        set
        {
            if (mainLightShadowmapResolutionFieldInfo == null)
            {
                InitializeShadowMapFieldInfo();
            }
            mainLightShadowmapResolutionFieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
        }
    }
}