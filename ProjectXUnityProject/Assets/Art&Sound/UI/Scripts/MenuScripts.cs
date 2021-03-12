using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public void BeginGame() 
    {
        SceneManager.LoadScene("Dungeon");
    }
}
