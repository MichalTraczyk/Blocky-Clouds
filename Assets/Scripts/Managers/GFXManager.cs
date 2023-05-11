using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFXManager : MonoBehaviour
{
    public static GFXManager Instance;

    [SerializeField] private float skyboxScrollSpeed;

    private SkyboxSO currSkybox;
    [SerializeField] private GameObject DayVolume;
    [SerializeField] private GameObject NightVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SetRenderSettings();
    }
    void ScrollSkybox()
    {
        if (currSkybox != null && !currSkybox.HQ)
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxScrollSpeed);
        }
    }
    private void Update()
    {
        ScrollSkybox();
    }
    public void SetRenderSettings()
    {
        int hq = PlayerPrefs.GetInt("goodGfx");
        bool highQuality = (hq == 1) ? true : false;
        if (highQuality)
        {
            int r = Random.Range(1, 6);

            string name = "HQSkybox" + r;
            string path = "SkyboxSO/HQ/" + name;

            SkyboxSO skybox = Resources.Load<SkyboxSO>(path);

            currSkybox = skybox;

            //if (RenderSettings.skybox != currSkybox.defaultMaterial)
            Debug.Log(currSkybox.defaultMaterial);
            RenderSettings.skybox = currSkybox.defaultMaterial;

            RenderSettings.skybox.SetTexture("_Tex", currSkybox.cubemap);
            //Debug.Log(skybox.material.mainTexture);
            RenderSettings.ambientSkyColor = currSkybox.lightColor;
            RenderSettings.ambientIntensity = currSkybox.lightInensity;
        }
        else
        {
            int r = Random.Range(1, 5);

            string name = "LQSkybox" + r;
            string path = "SkyboxSO/LQ/" + name;

            SkyboxSO skybox = Resources.Load<SkyboxSO>(path);

            currSkybox = skybox;

            //if (RenderSettings.skybox != currSkybox.defaultMaterial)
            Debug.Log(currSkybox.defaultMaterial);
            RenderSettings.skybox = currSkybox.defaultMaterial;

            RenderSettings.skybox.SetTexture("_FrontTex", currSkybox._FrontTex);
            RenderSettings.skybox.SetTexture("_BackTex", currSkybox._BackTex);
            RenderSettings.skybox.SetTexture("_LeftTex", currSkybox._LeftTex);
            RenderSettings.skybox.SetTexture("_RightTex", currSkybox._RightTex);
            RenderSettings.skybox.SetTexture("_UpTex", currSkybox._UpTex);
            RenderSettings.skybox.SetTexture("_DownTex", currSkybox._DownTex);

        }
        UpdatepostProcessingVolume();
    }
    public void UpdatepostProcessingVolume()
    {
        bool volumeEnabled = (PlayerPrefs.GetInt("useVolume") == 1) ? true : false;
        if (volumeEnabled)
        {
            if (currSkybox.atNight)
            {
                NightVolume.SetActive(true);
                DayVolume.SetActive(false);
            }
            else
            {
                NightVolume.SetActive(false);
                DayVolume.SetActive(true);
            }
        }
        else
        {
            NightVolume.SetActive(false);
            DayVolume.SetActive(false);
        }
    }
}
