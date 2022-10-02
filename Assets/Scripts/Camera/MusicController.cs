using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] tracks;
    [SerializeField] float fadeTime = 1;
    public int currentTrack = 0;
    AudioSource[] audioSources;
    // Start is called before the first frame update
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

    // Update is called once per frame
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
}
