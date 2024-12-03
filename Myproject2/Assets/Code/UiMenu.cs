using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{
    public AudioMixer AudioMixered;

    void Start()
    {
        // �֧��Ҩҡ PlayerPrefs ��е�駤�� AudioMixer
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetVolume("MusicVolume", musicVolume);
        SetVolume("MasterVolume", masterVolume);
        SetVolume("SFXVolume", sfxVolume);
    }

    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Level" + level); // ����¹�繪��� Scene �ͧ�س
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }

    // �ѧ��ѹ����Ѻ��駤�� Volume
    void SetVolume(string parameter, float volume)
    {
        AudioMixered.SetFloat(parameter, Mathf.Log10(volume) * 20); // ��駤�� AudioMixer �����ҷ��֧�ҡ PlayerPrefs
    }
}
