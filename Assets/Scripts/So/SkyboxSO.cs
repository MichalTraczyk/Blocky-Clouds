using UnityEngine;

[CreateAssetMenu(fileName = "Skybox preset", menuName = "ScriptableObjects/SkyboxPreset", order = 1)]
public class SkyboxSO : ScriptableObject
{
    public Material defaultMaterial;
    public bool atNight;
    public bool HQ;
    [Header("High quality skybox settings")]
    public Cubemap cubemap;
    public Color lightColor;
    [Range(-1, 1)]
    public float lightInensity;

    [Header("Low quality skybox settings")]
    public Texture _FrontTex;
    public Texture _BackTex;
    public Texture _LeftTex;
    public Texture _RightTex;
    public Texture _UpTex;
    public Texture _DownTex;
}