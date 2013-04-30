using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EffectVolumeSetter : MonoBehaviour {

    public float volume;
    public float timer = 0.1f;

    private float countdown;
    void Start()
    {
        GetComponent<AudioSource>().volume = 0;
        countdown = 0f;
    }

    void Update()
    {
        GetComponent<AudioSource>().volume = OptionsValues.sfxVolume;
        if (countdown > 0)
            countdown -= Time.deltaTime;
        else
            countdown = 0;
    }

    public void Play()
    {
        if (countdown == 0)
        {
            GetComponent<AudioSource>().Play();
            countdown = timer;
        }
    }
}
