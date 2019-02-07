using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//////////////////////////
//Main Menu Scene Script//
//////////////////////////

public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Loading Game Scene...");
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
