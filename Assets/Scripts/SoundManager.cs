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

    private AudioSource soundman;

    private void Awake()
    {
        if (inst == null)
            inst = this;
    }

    public void PlaySound(string _case)
    {
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
            
        soundman.Play();
    }
}
