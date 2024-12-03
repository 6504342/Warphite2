using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySoundEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float delay = 0f;

    public void EnemySoundHighpitch(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            double startTime = AudioSettings.dspTime + delay;
            audioSource.pitch = Random.Range(1.3f, 2.3f);
            audioSource.clip = soundEffects[index];
            audioSource.PlayScheduled(startTime);
        }
    }
    public void EnemySoundLowpitch(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.pitch = Random.Range(1f, 0.8f);
            audioSource.clip = soundEffects[index];
            audioSource.Play();
        }
    }
}  

