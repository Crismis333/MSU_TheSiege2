using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicVolumeSetter : MonoBehaviour {

    public float volume;
    public bool useGlobal;
	void Start () {
        GetComponent<AudioSource>().volume = 0;
        GetComponent<AudioSource>().Play();
	}

    void Update()
    {
        if (useGlobal)
            GetComponent<AudioSource>().volume = OptionsValues.musicVolume;
        else
            GetComponent<AudioSource>().volume = volume;
    }
}
