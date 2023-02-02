using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void ButtonGameStart()
    {
        SceneManager.LoadScene("MainScene");
        SoundManager.inst.PlaySound("Button");
    }

    public void ButtonIntro()
    {
        SceneManager.LoadScene("IntroScene");
        SoundManager.inst.PlaySound("Button");
    }

    public void ButtonHelp()
    {
        SceneManager.LoadScene("HelpScene");
        SoundManager.inst.PlaySound("Button");
    }
}
