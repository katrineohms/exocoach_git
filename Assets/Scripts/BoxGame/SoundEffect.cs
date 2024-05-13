using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip AudClip;

    public void playSound()
    {
        sound.clip = AudClip;
        sound.Play();
    }

}
