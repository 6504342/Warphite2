using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{
    public AudioMixer AudioMixered;

    void Start()
    {
        // ดึงค่าจาก PlayerPrefs และตั้งค่า AudioMixer
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetVolume("MusicVolume", musicVolume);
        SetVolume("MasterVolume", masterVolume);
        SetVolume("SFXVolume", sfxVolume);
    }

    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Level" + level); // เปลี่ยนเป็นชื่อ Scene ของคุณ
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }

    // ฟังก์ชันสำหรับตั้งค่า Volume
    void SetVolume(string parameter, float volume)
    {
        AudioMixered.SetFloat(parameter, Mathf.Log10(volume) * 20); // ตั้งค่า AudioMixer โดยใช้ค่าที่ดึงจาก PlayerPrefs
    }
}
