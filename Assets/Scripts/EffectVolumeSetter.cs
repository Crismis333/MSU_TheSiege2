using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EffectVolumeSetter : MonoBehaviour {

    public float volume;
    public float timer = 0.1f;

    private float countdown;
    private float downvolume;
    private bool soundvolumeZeroer;
    private AudioSource aus;

    private float startVol;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        startVol = aus.volume;
        aus.volume = 0;
        countdown = 0f;
        downvolume = 1f;
        soundvolumeZeroer = false;
    }

    void Update()
    {
        if (soundvolumeZeroer && downvolume > 0)
        {
            downvolume -= 0.16f;
            aus.volume = downvolume;
        }
        else if (soundvolumeZeroer)
        {
            downvolume = 1f;
            aus.volume = OptionsValues.sfxVolume;
            soundvolumeZeroer = false;
            aus.Play();
        }
        else
            aus.volume = startVol * OptionsValues.sfxVolume;
        if (countdown > 0)
            countdown -= Time.deltaTime;
        else
            countdown = 0;
    }

    public void Play()
    {
        if (countdown == 0)
        {
            if (aus.isPlaying)
                soundvolumeZeroer = true;
            else
                aus.Play();
            countdown = timer;
        }
    }
}
