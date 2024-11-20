using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioOption : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        Invoke("CloseWindow", 0.01f);
    }

    public void SetMusicVolume(float volume)
    {
        // ลดเสียงให้เงียบเมื่อ slider อยู่ที่ต่ำสุด
        audioMixer.SetFloat("MusicVolume", volume <= 0.01f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // ลดเสียงให้เงียบเมื่อ slider อยู่ที่ต่ำสุด
        audioMixer.SetFloat("SFXVolume", volume <= 0.01f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetMasterVolume(float volume)
    {
        // ลดเสียงให้เงียบเมื่อ slider อยู่ที่ต่ำสุด
        audioMixer.SetFloat("MasterVolume", volume <= 0.01f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void CloseWindow() 
    {
        gameObject.SetActive(false);
    }
}
