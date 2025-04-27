using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambienceSlider;

    private AudioSettingsSaveData audioSettingsSaveData = new AudioSettingsSaveData();
    // Start is called before the first frame update
    void Start()
    {
        audioSettingsSaveData = SavingSystem.LoadAudioSettings(GameManager.instance.currentSaveSlot);

        musicSlider.value = audioSettingsSaveData.musicVolume;
        sfxSlider.value = audioSettingsSaveData.sfxVolume;
        ambienceSlider.value = audioSettingsSaveData.ambienceVolume;
        masterSlider.value = audioSettingsSaveData.masterVolume;

        //apply the loaded settings to the music manager.
        MusicManager.instance.SetMusicVolume(musicSlider.value);
        MusicManager.instance.SetSFXVolume(sfxSlider.value);
        MusicManager.instance.SetAmbienceVolume(ambienceSlider.value);
        MusicManager.instance.SetMasterVolume(masterSlider.value);
        //add members for ambience and master volume in the music manager. 

        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        ambienceSlider.onValueChanged.AddListener(OnAmbienceValueChanged);
        masterSlider.onValueChanged.AddListener(OnMasterValueChanged);

    }


    public void OnMusicValueChanged(float value)
    {
        MusicManager.instance.SetMusicVolume(value);
        audioSettingsSaveData.musicVolume = value;
        SavingSystem.SaveAudioSettings(audioSettingsSaveData, GameManager.instance.currentSaveSlot);
    }
    public void OnSFXValueChanged(float value)
    {
        MusicManager.instance.SetSFXVolume(value);
        audioSettingsSaveData.sfxVolume = value;
        SavingSystem.SaveAudioSettings(audioSettingsSaveData, GameManager.instance.currentSaveSlot);
    }
    public void OnAmbienceValueChanged(float value)
    {
        MusicManager.instance.SetAmbienceVolume(value);
        audioSettingsSaveData.ambienceVolume = value;
        SavingSystem.SaveAudioSettings(audioSettingsSaveData, GameManager.instance.currentSaveSlot);
    }
    public void OnMasterValueChanged(float value)
    {
        MusicManager.instance.SetMasterVolume(value);
        audioSettingsSaveData.masterVolume = value;
        SavingSystem.SaveAudioSettings(audioSettingsSaveData, GameManager.instance.currentSaveSlot);
    }
}
