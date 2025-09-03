using UnityEngine;

public class LowLatencyBootstrap : MonoBehaviour
{
    [Tooltip("Set -1 to uncap in Unity; cap in driver (RTSS/NVIDIA/AMD) if you want tighter pacing.")]
    [SerializeField] private int _targetFrameRate = -1;

    [Tooltip("0 = VSync Off. Critical for mouse snappiness.")]
    [SerializeField] private int _vSyncCount = 0;

    [Tooltip("Try 1 to reduce render queue depth (not on all platforms/GPUs).")]
    [SerializeField] private int _maxQueuedFrames = 1;

    private void Awake()
    {
        QualitySettings.vSyncCount = _vSyncCount;
        Application.targetFrameRate = _targetFrameRate;

        //  Reduce driver queuing where supported
        QualitySettings.maxQueuedFrames = _maxQueuedFrames;
        //
        //  (Optional) Disable expensive defaults at runtime if you ship multiple quality tiers:
        //  QualitySettings.antiAliasing = 0; // prefer post AA like FXAA at most
        //  QualitySettings.shadows = ShadowQuality.Disable;
        //  QualitySettings.softParticles = false;
        //  QualitySettings.realtimeReflectionProbes = false;
        //  QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        //  QualitySettings.blendWeights (legacy) / skinning settings kept default unless heavy characters.
    }
}