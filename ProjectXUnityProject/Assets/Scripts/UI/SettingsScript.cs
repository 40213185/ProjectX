using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    private Vector2Int[] resolutions = new Vector2Int[] { new Vector2Int(1280, 720), new Vector2Int(1366, 768), new Vector2Int(1600, 900), new Vector2Int(1920, 1080) };
    private int currentResLoc = 0;

    public GameObject settingsTab;
    public Slider soundSlider;
    public Slider musicSlider;
    public Text resSizeButton;

    public bool fullScreen;

    public void Start()
    {
        fullScreen = true;
    }

    public void closeSettings()
    {
        settingsTab.SetActive(false);
    }

    public void ChangeResolution()
    {
        Debug.Log("res changed");
        currentResLoc++;
        if (currentResLoc > 3) currentResLoc = 0;
        Debug.Log("Res at pos" + resolutions[currentResLoc]);
        resSizeButton.text = resolutions[currentResLoc].ToString();
        Screen.SetResolution(resolutions[currentResLoc].x, resolutions[currentResLoc].y, fullScreen);
        Debug.Log("New Resolution is " + Screen.currentResolution);
    }

    public void SoundEffectChange()
    {
        Debug.Log("Sound Effect Vol now" + soundSlider.value);
    }

    public void MusicVolChange()
    {
        Debug.Log("Music Vol now" + musicSlider.value);
    }

    public void ToggleWindowMode() 
    {
        fullScreen = !fullScreen;
        Screen.SetResolution(resolutions[currentResLoc].x, resolutions[currentResLoc].y, fullScreen);
    }
}
