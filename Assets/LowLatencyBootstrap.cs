using UnityEngine;

public class LowLatencyBootstrap : MonoBehaviour
{
    [Tooltip("Set -1 to uncap in Unity; cap in driver (RTSS/NVIDIA/AMD) if you want tighter pacing.")]
    public int targetFrameRate = -1;

    [Tooltip("0 = VSync Off. Critical for mouse snappiness.")]
    public int vSyncCount = 0;

    [Tooltip("Try 1 to reduce render queue depth (not on all platforms/GPUs).")]
    public int maxQueuedFrames = 1;

    private void Awake()
    {
        //QualitySettings.vSyncCount = vSyncCount;
        //Application.targetFrameRate = targetFrameRate;

        // Reduce driver queuing where supported
        //QualitySettings.maxQueuedFrames = maxQueuedFrames;

        // (Optional) Disable expensive defaults at runtime if you ship multiple quality tiers:
        // QualitySettings.antiAliasing = 0; // prefer post AA like FXAA at most
        // QualitySettings.shadows = ShadowQuality.Disable;
        // QualitySettings.softParticles = false;
        // QualitySettings.realtimeReflectionProbes = false;
        // QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        // QualitySettings.blendWeights (legacy) / skinning settings kept default unless heavy characters.
    }
}