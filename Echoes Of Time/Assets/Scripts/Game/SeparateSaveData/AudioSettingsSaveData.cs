using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSettingsSaveData
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambienceVolume = 1f;

    public AudioSettingsSaveData()
    {
       
    }
    public AudioSettingsSaveData(float masterVolume, float musicVolume, float sfxVolume, float ambienceVolume)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.ambienceVolume = ambienceVolume;
    }
}
