using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(GUINavigation))]
public class OptionsMenuScript : MonoBehaviour {
	
	public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool pauseMenu;
    public EffectVolumeSetter cancelSound;


    private float musicvol, effectvol, resolution, quality;
    private bool fullscreen;
    private Resolution mres;

    private Texture2D background, backgroundSlider, backgroundToggle, backgroundToggleOn, backgroundSliderOff, backgroundToggleOff, backgroundToggleOnOff;
    private Color activeColor, inactiveColor;
    private bool firstGUI, selectrelease, buttonactive;
    private bool movedLeft, movedRight;
    private Resolution[] res;
    private float f, previouseffectvol;

    private GUINavigation guin;

	void Menu_Options() {
        if (!guin.usingMouse)
        {
            GUI.skin.button.hover.background = null;
            GUI.skin.horizontalSlider.hover.background = backgroundSliderOff;
            GUI.skin.toggle.hover.background = backgroundToggleOff;
            GUI.skin.toggle.onHover.background = backgroundToggleOnOff;
        }
        else
        {
            GUI.skin.button.hover.background = background;
            GUI.skin.horizontalSlider.hover.background = backgroundSlider;
            GUI.skin.toggle.hover.background = backgroundToggle;
            GUI.skin.toggle.onHover.background = backgroundToggleOn;
        }
        if (guin.activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;

        float musicvolstart = musicvol;
        float effectvolstart = effectvol;
        if (backgroundScroll != null) {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
            GUI.DrawTexture(new Rect(0,0,backgroundScroll.width, backgroundScroll.height), backgroundScroll);
            GUI.EndGroup();
        }
        
        GUI.BeginGroup(new Rect(Screen.width/2-395, Screen.height / 2 - 3*70, 790, 6*70));
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.SetNextControlName("title");
        GUI.Label(new Rect(0, 0*70, 790, 64), "Music volume");
        GUI.Label(new Rect(0, 1 * 70, 790, 64), "Effect volume");
        GUI.Label(new Rect(0, 2 * 70, 790, 64), "Fullscreen");
        GUI.Label(new Rect(0, 3 * 70, 790, 64), "Resolution");
        GUI.Label(new Rect(0, 4 * 70, 790, 64), "Quality");
        GUI.color = Color.white;
        GUI.SetNextControlName("Music");
        musicvol = GUI.HorizontalSlider(new Rect(250, 0 * 70, 512, 64), musicvol, 0.0f, 1.0f);
        GUI.SetNextControlName("Effect");
        effectvol = GUI.HorizontalSlider(new Rect(250, 1 * 70, 512, 64), effectvol, 0.0f, 1.0f);
        GUI.SetNextControlName("Fullscreen");
        fullscreen = GUI.Toggle(new Rect(350, 2 * 70, 64, 64),fullscreen, " ");
        GUI.SetNextControlName("Resolution");
        resolution = GUI.HorizontalSlider(new Rect(250, 3 * 70, 512, 64), resolution, f, (float)res.Length - 1);
        resolution = ((int)resolution + 0.5f);
        mres = res[(int)(resolution)];

        GUI.SetNextControlName("Quality");
        quality = GUI.HorizontalSlider(new Rect(250, 4 * 70, 512, 64), quality, 0.0f, 5.0f);
        quality = ((int)quality + 0.5f);
        GUI.SetNextControlName("Accept");
        if (GUI.Button(new Rect(60, 5 * 70, 730, 64), "Accept"))
            Menu_Options_Back();
        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
        GUI.Label(new Rect(450, 3 * 70, 340, 64), mres.width + "x" + mres.height);
        GUI.Label(new Rect(450, 4 * 70, 340, 64), QualitySettings.names[(int)quality]);

        GUI.Box(new Rect(250, 0 * 70, 512, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(250, 1 * 70, 512, 64), new GUIContent("", "1"));
        GUI.Box(new Rect(350, 2 * 70, 64, 64), new GUIContent("", "2"));
        GUI.Box(new Rect(250, 3 * 70, 512, 64), new GUIContent("", "3"));
        GUI.Box(new Rect(250, 4 * 70, 512, 64), new GUIContent("", "4"));
        GUI.Box(new Rect(60, 5 * 70, 730, 64), new GUIContent("", "5"));
        guin.mouseover = GUI.tooltip;

		GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.horizontalSlider.hover.background = backgroundSlider;
        GUI.skin.toggle.hover.background = backgroundToggle;
        GUI.skin.toggle.onHover.background = backgroundToggleOn;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!guin.usingMouse)
            switch (guin.keySelect)
            {
                case 0: GUI.FocusControl("Music"); break;
                case 1: GUI.FocusControl("Effect"); break;
                case 2: GUI.FocusControl("Fullscreen"); break;
                case 3: GUI.FocusControl("Resolution"); break;
                case 4: GUI.FocusControl("Quality"); break;
                case 5: GUI.FocusControl("Accept"); break;
                default: GUI.FocusControl("title"); break;
            }
        else
            GUI.FocusControl("title");
	}
	
	void OnGUI() {
		GUI.skin = gSkin;
        if (firstGUI)
        {
            background = GUI.skin.button.hover.background;
            backgroundSlider = GUI.skin.horizontalSlider.hover.background;
            backgroundToggle = GUI.skin.toggle.hover.background;
            backgroundToggleOn = GUI.skin.toggle.onHover.background;
            backgroundSliderOff = GUI.skin.horizontalSlider.normal.background;
            backgroundToggleOff = GUI.skin.toggle.normal.background;
            backgroundToggleOnOff = GUI.skin.toggle.onNormal.background;

            activeColor = GUI.skin.button.active.textColor;
            inactiveColor = GUI.skin.button.focused.textColor;
            firstGUI = false;
        }
        if (guin.QuitPressed())
            Menu_Options_Back();
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

    public void Menu_Options_Back() {
        gSkin.button.hover.background = background;
        gSkin.horizontalSlider.hover.background = backgroundSlider;
        gSkin.toggle.hover.background = backgroundToggle;
        gSkin.toggle.onHover.background = backgroundToggleOn;
        gSkin.button.focused.textColor = inactiveColor;
        if (mres.width != Screen.width || mres.height != Screen.height || fullscreen != Screen.fullScreen)
            Screen.SetResolution(mres.width, mres.height, fullscreen);
        OptionsValues.musicVolume = musicvol;
        PlayerPrefs.SetFloat("MusicVolume", OptionsValues.musicVolume);
        OptionsValues.sfxVolume = effectvol;
        PlayerPrefs.SetFloat("SFXVolume", OptionsValues.sfxVolume);
        PlayerPrefs.Save();
        QualitySettings.SetQualityLevel((int)quality);
        this.enabled = false;
        cancelSound.Play();
        if (!pauseMenu)
        {
            MainMenuScript mms = GetComponent<MainMenuScript>();
            mms.enabled = true;
            guin.ClearElements();
            guin.maxKeys = 7;
            guin.AddElement(0, mms.Menu_Main_Start_Campaign);
            guin.AddElement(1, mms.Menu_Main_Start_Endless);
            guin.AddElement(2, mms.Menu_Main_Controls);
            guin.AddElement(3, mms.Menu_Main_Options);
            guin.AddElement(4, mms.Menu_Main_Highscores);
            guin.AddElement(5, mms.Menu_Main_Credits);
            guin.AddElement(6, mms.Menu_Main_Quit);
        }
        else
        {
            PauseMenuScript pms = GetComponent<PauseMenuScript>();
            pms.enabled = true;
            guin.ClearElements();
            guin.maxKeys = 5;
            guin.menuKey = 4;
            guin.AddElement(0, pms.Pause_Options);
            guin.AddElement(1, pms.Pause_Controls);
            guin.AddElement(2, pms.Pause_GiveUp);
            guin.AddElement(3, pms.Pause_Quit);
            guin.AddElement(4, pms.Pause_Back);
        }
    }

    void Start()
    {
        firstGUI = true;
        selectrelease = false;
        buttonactive = false;
        movedLeft = false;
        movedRight = false;
        musicvol = OptionsValues.musicVolume;
        effectvol = OptionsValues.sfxVolume;
        fullscreen = Screen.fullScreen;
        guin = GetComponent<GUINavigation>();
        
        res = Screen.resolutions;
        f = 0.0f;
        if (res.Length > 1)
        {
            for (int i = 0; i < res.Length; i++)
                if (res[i].width < 800 || res[i].height < 600)
                    f += 1.0f;
            for (resolution = f; resolution < res.Length && (res[(int)resolution].width != Screen.width || res[(int)resolution].height != Screen.height); resolution += 1) ;
        }
        else
            resolution = 0;
    }

    void Update()
    {
        res = Screen.resolutions;
        float startquality = quality;
        f = 0.0f;
        if (res.Length > 1)
            for (int i = 0; i < res.Length; i++)
                if (res[i].width < 800 || res[i].height < 600)
                    f += 1.0f;

        float kh = Input.GetAxisRaw("Horizontal");
        float kh2 = Input.GetAxisRaw("DPadHorizontal");

        if (!guin.usingMouse)
            switch (guin.keySelect)
            {
                case 0:
                    {
                        if (Mathf.Abs(kh) > Mathf.Abs(kh2))
                            musicvol += Input.GetAxis("Horizontal") / 50;
                        else
                            musicvol += Input.GetAxis("DPadHorizontal") / 50;
                        if (musicvol < 0) musicvol = 0;
                        else if (musicvol > 1) musicvol = 1;
                        break;
                    }
                case 1: 
                    {
                        if (Mathf.Abs(kh) > Mathf.Abs(kh2))
                            effectvol += Input.GetAxis("Horizontal") / 50;
                        else
                            effectvol += Input.GetAxis("DPadHorizontal") / 50;
                        if (effectvol < 0) effectvol = 0;
                        else if (effectvol > 1) effectvol = 1;
                        break; 
                    }
                case 2:
                    {
                        if (buttonactive && !guin.activated)
                        {
                            fullscreen = !fullscreen;
                        }
                        break;
                    }
                case 3:
                    {
                        if ((kh < -0.2 || kh2 < -0.2) && !movedLeft)
                            resolution--;
                        else if ((kh > 0.2 || kh2 > 0.2) && !movedRight)
                            resolution++;
                        if (resolution < f) resolution = (int)f;
                        else if (resolution > res.Length - 1) resolution = res.Length - 1;
                            
                        mres = res[(int)(resolution)];
                        break;
                    }
                case 4:
                    {
                        if ((kh < -0.2 || kh2 < -0.2) && !movedLeft)
                            quality--;
                        else if ((kh > 0.2 || kh2 > 0.2) && !movedRight)
                            quality++;
                        if (quality < 0) quality = 0;
                        else if (quality > 5) quality = 5;
                        break;
                    }
            }
        if (kh > 0.2 || kh2 > 0.2)
        {
            movedRight = true;
            movedLeft = false;
        }
        else if (kh < -0.2 || kh2 < -0.2)
        {
            movedRight = false;
            movedLeft = true;
        }
        else
        {
            movedRight = false;
            movedLeft = false;
        }
        buttonactive = guin.activated;
        OptionsValues.musicVolume = musicvol;
        OptionsValues.sfxVolume = effectvol;
        previouseffectvol = effectvol;
        if (startquality != quality)
            QualitySettings.SetQualityLevel((int)quality);
    }
}
