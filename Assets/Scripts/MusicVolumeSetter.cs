using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class MusicVolumeSetter : MonoBehaviour {

    public float volume;
    public bool useGlobal;

    
    private AudioSource aus;

	void Start () {
        aus = GetComponent<AudioSource>();
        aus.volume = 0;
        aus.Play();
	}

    void Update()
    {
        if (useGlobal)
            aus.volume = OptionsValues.musicVolume;
        else
            aus.volume = volume;
    }
}
