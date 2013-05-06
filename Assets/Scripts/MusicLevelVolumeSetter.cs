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
        distance = Mathf.Clamp(((player.position.z - army.position.z))/30,0,1);
        //print(""+distance);
        if (useGlobal)
        {
            if (distance >= 0.5f)
            {
                float tmpD = (distance - 0.5f) * (1 / 0.5f);
                music1.volume = OptionsValues.musicVolume * tmpD;
                music2.volume = OptionsValues.musicVolume * (1 - tmpD);
                music3.volume = 0;
            }
            else
            {
                float tmpD = distance * 2;
                music1.volume = 0;
                music2.volume = OptionsValues.musicVolume * tmpD;
                music3.volume = OptionsValues.musicVolume * (1 - tmpD);
            }
        }
        else
        {
            if (distance >= 0.5f)
            {
                float tmpD = (distance - 0.5f) * (1 / 0.5f);
                music1.volume = volume * tmpD;
                music2.volume = volume * (1 - tmpD);
                music3.volume = 0;
            }
            else
            {
                float tmpD = distance * 2;
                music1.volume = 0;
                music2.volume = volume * tmpD;
                music3.volume = volume * (1 - tmpD);
            }
        }
    }
}
