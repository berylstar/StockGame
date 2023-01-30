using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public SoundManager inst = null;
    public AudioClip sTime;
    public AudioClip sBuy;
    public AudioClip sSell;

    private void Awake()
    {
        if (inst == null)
            inst = this;
    }
}
