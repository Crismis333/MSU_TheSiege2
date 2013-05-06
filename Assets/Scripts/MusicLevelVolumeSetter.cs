using UnityEngine;
using System.Collections;

public class MusicLevelVolumeSetter : MonoBehaviour {

    public float volume;
    public bool useGlobal;

    public AudioClip LevelMusic, ArmyMusic, ArmyMusicClose;
    private AudioSource music1, music2, music3;
    private float distance;
    private Transform player, army;

    void Start()
    {
        GameObject go = new GameObject();
        music1 = go.AddComponent<AudioSource>();
        music1.clip = LevelMusic;
        music1.volume = 0;
        music1.loop = true;
        music1.Play();
        go = new GameObject();
        music2 = go.AddComponent<AudioSource>();
        music2.clip = ArmyMusic;
        music2.volume = 0;
        music2.loop = true;
        music2.Play();
        go = new GameObject();
        music3 = go.AddComponent<AudioSource>();
        music3.clip = ArmyMusicClose;
        music3.volume = 0;
        music3.loop = true;
        music3.Play();
    }

    void Update()
    {
        if (player == null)
            player = ObstacleController.PLAYER.transform;
        if (army == null)
            army = ObstacleController.ARMY.transform;
        distance = Mathf.Clamp(((player.position.z - army.position.z - 4))/30,0,1);
        print("distance: " + distance);
        if (useGlobal)
        {
            if (distance >= 0.5f)
            {
                music1.volume = OptionsValues.musicVolume;
                music2.volume = 1-OptionsValues.musicVolume;
                music3.volume = 0;
            }
            else
            {
                music1.volume = 0;
                music2.volume = OptionsValues.musicVolume;
                music3.volume = 1-OptionsValues.musicVolume;
            }
        }
        else
        {
            music1.volume = volume;
            music2.volume = volume;
            music3.volume = volume;
        }
    }
}
