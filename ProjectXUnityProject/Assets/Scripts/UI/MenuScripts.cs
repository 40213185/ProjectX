using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MenuScripts : MonoBehaviour
{
    private Vector2Int[] resolutions = new Vector2Int[] { new Vector2Int(1280, 720), new Vector2Int(1366, 768), new Vector2Int(1600, 900), new Vector2Int(1920, 1080) };
    private int currentResLoc = 0;

    public GameObject settingsTab;
    public GameObject main;
    public Slider soundSlider;
    public Slider musicSlider;
    public Text resSizeButton;

    //Main
    public void BeginGame() 
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void ToggleSettings() 
    {
        main.SetActive(settingsTab.activeSelf);
        settingsTab.SetActive(!settingsTab.activeSelf);
        resSizeButton.text = Screen.currentResolution.ToString();
        Vector2Int res = new Vector2Int(Screen.width, Screen.height);
        for (int i = 0; i < 4; i++) 
        {
            if(res == resolutions[i]) 
            {
                currentResLoc = i;
                Debug.Log("Current Res Loc = " + currentResLoc);
                break;
            }
        }
    }

    public void Quit() 
    {
        Application.Quit();
    }

    //Settings

    public void ChangeResolution() 
    {
        currentResLoc++;
        if (currentResLoc > 3) currentResLoc = 0;
        Debug.Log("Res at pos" + resolutions[currentResLoc]);
        resSizeButton.text = resolutions[currentResLoc].ToString();
        Screen.SetResolution(resolutions[currentResLoc].x, resolutions[currentResLoc].y, true);
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
}
