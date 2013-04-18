using UnityEngine;
using System.Collections;
using System;

public class OptionsMenuScript : MonoBehaviour {
	
	public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool pauseMenu;
    private float musicvol, effectvol, resolution, quality;
    private bool fullscreen;
    private Resolution mres;

	void Menu_Options() {
        float musicvolstart = musicvol;
        float effectvolstart = effectvol;
        Resolution[] res = Screen.resolutions;
        float f = 0.0f;
        if (res.Length > 1)
            for (int i = 0; i < res.Length; i++)
                if (res[i].width < 800 || res[i].height < 600)
                    f += 1.0f;
        if (backgroundScroll != null) {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
            GUI.DrawTexture(new Rect(0,0,backgroundScroll.width, backgroundScroll.height), backgroundScroll);
            GUI.EndGroup();
        }
        
        GUI.BeginGroup(new Rect(Screen.width/2-395, Screen.height / 2 - 3*70, 790, 6*70));
        GUI.color = Color.black;
        GUI.Label(new Rect(0, 0*70, 790, 64), "Music volume");
        GUI.Label(new Rect(0, 1 * 70, 790, 64), "Effect volume");
        GUI.Label(new Rect(0, 2 * 70, 790, 64), "Fullscreen");
        GUI.Label(new Rect(0, 3 * 70, 790, 64), "Resolution");
        GUI.Label(new Rect(0, 4 * 70, 790, 64), "Quality");
        GUI.color = Color.white;
        musicvol = GUI.HorizontalSlider(new Rect(250, 0 * 70, 512, 64), musicvol, 0.0f, 1.0f);
        effectvol = GUI.HorizontalSlider(new Rect(250, 1 * 70, 512, 64), effectvol, 0.0f, 1.0f);
        fullscreen = GUI.Toggle(new Rect(350, 2 * 70, 64, 64),fullscreen, " ");
        resolution = GUI.HorizontalSlider(new Rect(250, 3 * 70, 512, 64), resolution, f, (float)res.Length - 1);
        resolution = ((int)resolution + 0.5f);
        mres = res[(int)(resolution)];
        
        quality = GUI.HorizontalSlider(new Rect(250, 4 * 70, 512, 64), quality, 0.0f, 5.0f);
        quality = ((int)quality + 0.5f);
        if (GUI.Button(new Rect(60, 5 * 70, 730, 64), "Accept")) { Menu_Options_Back(); }
        GUI.color = Color.red;
        GUI.Label(new Rect(450, 3 * 70, 340, 64), mres.width + "x" + mres.height);
        GUI.Label(new Rect(450, 4 * 70, 340, 64), QualitySettings.names[(int)quality]);
        

		GUI.EndGroup();
        if (musicvolstart != musicvol)
            OptionsValues.musicVolume = musicvol;
        if (effectvolstart != effectvol)
            OptionsValues.sfxVolume = effectvol;
	}
	
	void OnGUI() {
		GUI.skin = gSkin;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu_Options_Back();
        }
        else
		    Menu_Options();
	}

    public void Menu_Options_Startup() {
        musicvol = OptionsValues.musicVolume;
        effectvol = OptionsValues.sfxVolume;
        mres = Screen.currentResolution;
        fullscreen = Screen.fullScreen;
        quality = (float)QualitySettings.GetQualityLevel();
        for (int i = 0; i < Screen.GetResolution.Length; i++)
            if (Screen.GetResolution[i].height >= Screen.height && Screen.GetResolution[i].width >= Screen.width) {
                resolution = i;
                break;
            }
        enabled = true;
    }

    void Menu_Options_Back() {
        if (mres.width != Screen.width || mres.height != Screen.height || fullscreen != Screen.fullScreen)
            Screen.SetResolution(mres.width, mres.height, fullscreen);
        OptionsValues.musicVolume = musicvol;
        PlayerPrefs.SetFloat("MusicVolume", OptionsValues.musicVolume);
        OptionsValues.sfxVolume = effectvol;
        PlayerPrefs.SetFloat("SFXVolume", OptionsValues.sfxVolume);
        PlayerPrefs.Save();
        QualitySettings.SetQualityLevel((int)quality);
        this.enabled = false;
        if (!pauseMenu)
            GetComponent<MainMenuScript>().enabled = true;
        else
            GetComponent<PauseMenuScript>().enabled = true;
    }
}
