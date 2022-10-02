using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] tracks;
    [SerializeField] float fadeTime = 1;
    public int currentTrack = 0;
    AudioSource[] audioSources;
    public static MusicController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSources = new AudioSource[tracks.Length];
        for (int i = 0; i < tracks.Length; i++)
        {
            audioSources[i] = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSources[i].clip = tracks[i];
            audioSources[i].volume = 0;
            audioSources[i].loop = true;
            audioSources[i].Play();
        }
        audioSources[currentTrack].volume = 1;
    }
    void Update()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            float currentVolume = audioSources[i].volume;
            if (i == currentTrack)
            {
                if (currentVolume < 1)
                {
                    audioSources[i].volume = Mathf.Min(currentVolume + Time.deltaTime / fadeTime, 1);
                }
            }
            else
            {
                if (currentVolume > 0)
                {
                    audioSources[i].volume = Mathf.Max(currentVolume - Time.deltaTime / fadeTime, 0);
                }
            }
        }
    }
    public void SetTime(float time)
    {
        foreach (AudioSource s in audioSources)
        {
            s.time = time;
        }
    }
    public void Pause()
    {
        foreach (AudioSource s in audioSources)
        {
            s.Pause();
        }
    }
    public void UnPause()
    {
        foreach (AudioSource s in audioSources)
        {
            s.UnPause();
        }
    }
}
