using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst = null;

    [Header("Time Sound")]
    public GameObject speekerTime;
    public AudioClip sBeforeTime;
    public AudioClip sOnTime;

    [Header("Button Sound")]
    public GameObject speekerButton;
    public AudioClip sBuy;
    public AudioClip sSell;
    public AudioClip sError;
    public AudioClip sButton;
    public AudioClip sLongClick;

    private AudioSource soundman;
    private bool flag_Mute = false;

    private void Awake()
    {
        if (inst == null)
            inst = this;
    }

    public void PlaySound(string _case)
    {
        if (flag_Mute)
            return;

        if (_case == "BeforeTime")
        {
            soundman = speekerTime.GetComponent<AudioSource>();
            soundman.clip = sBeforeTime;
        }
            
        else if (_case == "OnTime")
        {
            soundman = speekerTime.GetComponent<AudioSource>();
            soundman.clip = sOnTime;
        }
            
        else if (_case == "Buy")
        {
            soundman = speekerButton.GetComponent<AudioSource>();
            soundman.clip = sBuy;
        }
            
        else if (_case == "Sell")
        {
            soundman = speekerButton.GetComponent<AudioSource>();
            soundman.clip = sSell;
        }
            
        else if (_case == "Error")
        {
            soundman = speekerButton.GetComponent<AudioSource>();
            soundman.clip = sError;
        }

        else if (_case == "Button")
        {
            soundman = speekerButton.GetComponent<AudioSource>();
            soundman.clip = sButton;
        }

        else if (_case == "LongClick")
        {
            soundman = speekerButton.GetComponent<AudioSource>();
            soundman.clip = sLongClick;
        }

        soundman.Play();
    }

    public void ButtonMute()
    {
        flag_Mute = !flag_Mute;
    }
}