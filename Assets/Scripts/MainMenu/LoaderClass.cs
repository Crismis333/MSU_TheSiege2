using UnityEngine;
using System.Collections;

public class LoaderClass : MonoBehaviour {

	void Start () {
        CurrentGameState.Restart();
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        else
            OptionsValues.musicVolume = PlayerPrefs.GetFloat("MusicVolume");


        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
        else
            OptionsValues.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        if (!PlayerPrefs.HasKey("Highscore1Name"))
            PlayerPrefs.SetString("Highscore1Name", "");
        if (!PlayerPrefs.HasKey("Highscore1Score"))
            PlayerPrefs.SetString("Highscore1Score", "");
        if (!PlayerPrefs.HasKey("Highscore2Name"))
            PlayerPrefs.SetString("Highscore2Name", "");
        if (!PlayerPrefs.HasKey("Highscore2Score"))
            PlayerPrefs.SetString("Highscore2Score", "");
        if (!PlayerPrefs.HasKey("Highscore3Name"))
            PlayerPrefs.SetString("Highscore3Name", "");
        if (!PlayerPrefs.HasKey("Highscore3Score"))
            PlayerPrefs.SetString("Highscore3Score", "");
        if (!PlayerPrefs.HasKey("Highscore4Name"))
            PlayerPrefs.SetString("Highscore4Name", "");
        if (!PlayerPrefs.HasKey("Highscore4Score"))
            PlayerPrefs.SetString("Highscore4Score", "");
        if (!PlayerPrefs.HasKey("Highscore5Name"))
            PlayerPrefs.SetString("Highscore5Name", "");
        if (!PlayerPrefs.HasKey("Highscore5Score"))
            PlayerPrefs.SetString("Highscore5Score", "");
        if (!PlayerPrefs.HasKey("Highscore6Name"))
            PlayerPrefs.SetString("Highscore6Name", "");
        if (!PlayerPrefs.HasKey("Highscore6Score"))
            PlayerPrefs.SetString("Highscore6Score", "");
        if (!PlayerPrefs.HasKey("Highscore7Name"))
            PlayerPrefs.SetString("Highscore7Name", "");
        if (!PlayerPrefs.HasKey("Highscore7Score"))
            PlayerPrefs.SetString("Highscore7Score", "");
        if (!PlayerPrefs.HasKey("Highscore8Name"))
            PlayerPrefs.SetString("Highscore8Name", "");
        if (!PlayerPrefs.HasKey("Highscore8Score"))
            PlayerPrefs.SetString("Highscore8Score", "");
        if (!PlayerPrefs.HasKey("Highscore9Name"))
            PlayerPrefs.SetString("Highscore9Name", "");
        if (!PlayerPrefs.HasKey("Highscore9Score"))
            PlayerPrefs.SetString("Highscore9Score", "");
        if (!PlayerPrefs.HasKey("Highscore10Name"))
            PlayerPrefs.SetString("Highscore10Name", "");
        if (!PlayerPrefs.HasKey("Highscore10Score"))
            PlayerPrefs.SetString("Highscore10Score", "");
        long result;
        for (int i = 1; i <= 10; i++)
        {
            if (long.TryParse(PlayerPrefs.GetString("Highscore" + i + "Score"), out result))
            {
                GameObject ob = new GameObject();
                ob.AddComponent<HighScoreElement>();
                ob.GetComponent<HighScoreElement>().user = PlayerPrefs.GetString("Highscore" + i + "Name");
                ob.GetComponent<HighScoreElement>().score = result;
                CurrentGameState.AddHighscoreElement(ob);
            }

        }
        PlayerPrefs.Save();	
	}
}
