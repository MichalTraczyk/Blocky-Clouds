using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class OptionsMenu : MonoBehaviour
{
    public RenderPipelineAsset highSettingsPipeline;
    public RenderPipelineAsset lowSettingsPipeline;
    public Toggle gfxToggle;
    public Toggle volumeToggle;

    public Slider soundEffectsVolumeSlider;
    public Slider musicEffectsVolumeSlider;
    private void Start()
    {
        bool gfxToggleBool = (PlayerPrefs.GetInt("goodGfx") == 1) ? true : false;
        bool volumeBool = (PlayerPrefs.GetInt("useVolume") == 1) ? true : false;
        gfxToggle.isOn = gfxToggleBool;
        volumeToggle.isOn = volumeBool;
        GraphicsSettingsToggleClicked();
        VolumeToggleClicked();

        SetSlidersValue();
        SliderValueChanged();
    }

    void SetSlidersValue()
    {
        float soundEffectsValue = PlayerPrefs.GetFloat("SoundVolume");
        float musicValue = PlayerPrefs.GetFloat("MusicVolume");

        soundEffectsVolumeSlider.value = soundEffectsValue;
        musicEffectsVolumeSlider.value = musicValue;
    }

    public void GraphicsSettingsToggleClicked()
    {
        int i = (gfxToggle.isOn) ? 1 : 0;
        PlayerPrefs.SetInt("goodGfx", i);
        //Change pipeline in render settings
        if (gfxToggle.isOn)
            GraphicsSettings.renderPipelineAsset = highSettingsPipeline;
        else
            GraphicsSettings.renderPipelineAsset = lowSettingsPipeline;

        if (GameManager.Instance != null)
            GameManager.Instance.SetRenderSettings();

    }
    public void VolumeToggleClicked()
    {
        //Change playerprefs
        int i = (volumeToggle.isOn) ? 1 : 0;
        PlayerPrefs.SetInt("useVolume", i);
        if (GameManager.Instance != null)
            GameManager.Instance.UpdatepostProcessingVolume();
    }

    public void SliderValueChanged()
    {
        //Debug.Log("Changing value!");
        float soundEffectsValue = soundEffectsVolumeSlider.value;
        float musicValue = musicEffectsVolumeSlider.value;

        PlayerPrefs.SetFloat("SoundVolume", soundEffectsValue);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);

        SoundManager.Instance.SetSoundEffectsVolume(soundEffectsValue);
        SoundManager.Instance.SetMusicVolume(musicValue);

    }


}
