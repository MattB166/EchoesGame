using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip menuMusic;

    [Header("SFX")]
    private List<AudioSource> sfxSourcePool = new List<AudioSource>();
    private int currentSFXSourceIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        if(sfxSourcePool.Count == 0)
        {
            for (int i = 0; i < 10; i++)
            {
               AudioSource newSFXSource = gameObject.AddComponent<AudioSource>();
               newSFXSource.playOnAwake = false;
               newSFXSource.outputAudioMixerGroup = sfxGroup;
               sfxSourcePool.Add(newSFXSource);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(AudioClip clip, bool loop)
    {
        if(musicSource.clip == clip)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, Vector3? position = null, float? volume = null)
    {
        if (clip == null)
        {
            return;
        }

        if (position == null)
        {
            //sfxSource.outputAudioMixerGroup = sfxGroup;
            //sfxSource.PlayOneShot(clip);
            AudioSource currentSource = sfxSourcePool[currentSFXSourceIndex];
            currentSource.outputAudioMixerGroup = sfxGroup;
            if(volume != null)
            {
                currentSource.volume = volume.Value;
            }
            else
            {
                currentSource.volume = 1f;
            }
            currentSource.PlayOneShot(clip);
            currentSFXSourceIndex = (currentSFXSourceIndex + 1) % sfxSourcePool.Count;
        }
        else
        {
            GameObject tmpObj = new GameObject("TempSFX_" + clip.name);
            tmpObj.transform.position = position.Value;
            AudioSource tmpSource = tmpObj.AddComponent<AudioSource>();
            if(volume != null)
            {
                tmpSource.volume = volume.Value;
            }
            else
            {
                tmpSource.volume = 1f;
            }
            tmpSource.clip = clip;
            tmpSource.outputAudioMixerGroup = sfxGroup;
            tmpSource.spatialBlend = 1f; 
            tmpSource.minDistance = 1f;
            tmpSource.maxDistance = 20f;
            tmpSource.Play();
            Destroy(tmpObj, clip.length + 1.0f);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop(); //change to a fade out. 
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value,0.0001f,1f)) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }
}
