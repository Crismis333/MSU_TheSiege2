using UnityEngine;
using System.Collections;

public class LoaderClass : MonoBehaviour {

    public string[] highscoresCampaignName, highscoresInfiniteName;
    public int[] highscoresCampaignScore, highscoresInfiniteScore;

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

        for (int i = 1; i <= 10; i++)
        {
            if (!PlayerPrefs.HasKey("Highscore" + i + "CampaignName"))
            {
                PlayerPrefs.SetString("Highscore" + i + "CampaignName", highscoresCampaignName[i-1]);
                PlayerPrefs.SetString("Highscore" + i + "CampaignScore", ""+highscoresCampaignScore[i-1]);
            }
            if (!PlayerPrefs.HasKey("Highscore" + i + "InfiniteName"))
            {
                PlayerPrefs.SetString("Highscore" + i + "InfiniteName", highscoresInfiniteName[i-1]);
                PlayerPrefs.SetString("Highscore" + i + "InfiniteScore", ""+highscoresInfiniteScore[i-1]);
            }
        }


        long result;
        for (int i = 1; i <= 10; i++)
        {
            if (long.TryParse(PlayerPrefs.GetString("Highscore" + i + "CampaignScore"), out result))
            {
                GameObject ob = new GameObject();
                ob.AddComponent<HighScoreElement>();
                ob.GetComponent<HighScoreElement>().user = PlayerPrefs.GetString("Highscore" + i + "CampaignName");
                ob.GetComponent<HighScoreElement>().score = result;
                CurrentGameState.AddHighscoreElement(ob);
            }
            if (long.TryParse(PlayerPrefs.GetString("Highscore" + i + "InfiniteScore"), out result))
            {
                GameObject ob = new GameObject();
                ob.AddComponent<HighScoreElement>();
                ob.GetComponent<HighScoreElement>().user = PlayerPrefs.GetString("Highscore" + i + "InfiniteName");
                ob.GetComponent<HighScoreElement>().score = result;
                CurrentGameState.AddHighscoreElementInfinite(ob);
            }

        }
        PlayerPrefs.Save();	
	}
}
