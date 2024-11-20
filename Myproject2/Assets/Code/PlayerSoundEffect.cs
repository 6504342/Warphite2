using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundEffects;

    public void PlaySoundHighpitch(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.pitch = Random.Range(1.3f, 2.3f); 
            audioSource.clip = soundEffects[index];
            audioSource.Play();
        }
    }
    public void PlaySoundLowpitch(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.pitch = Random.Range(1f, 0.8f);
            audioSource.clip = soundEffects[index];
            audioSource.Play();
        }
    }
}
