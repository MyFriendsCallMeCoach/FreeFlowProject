using UnityEngine;

[System.Serializable]
public class CrossFadeSettings
{
    [Header("Crossfade Settings")]
    public string StateName;
    [Min(-1)] public int Layer = 0;
    [Min(0)] public float TimeOffset;
    [Min(0)] public float TransitionDuration;
}
