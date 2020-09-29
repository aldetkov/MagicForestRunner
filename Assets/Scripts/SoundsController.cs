using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundsController : MonoBehaviour
{
    public AudioMixer audioMixer;

    AudioSource[] audioSources;
    string[] audioNames;
    public const string SOMERSAULT = "Somersault";
    public const string JUMP = "Jump"; 
    public const string HIT = "Hit"; 
    public const string TAKECOIN = "TakeCoin";

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (GameObject.FindGameObjectWithTag("soundsController") != null) Destroy(gameObject);
        gameObject.tag = "soundsController";
    }
    // Start is called before the first frame update
    void Start()
    {
        SetMusicVol(GetMusicVol());
        SetSoundsVol(GetSoundsVol());
        audioSources = gameObject.GetComponents<AudioSource>();
        audioNames = new string[audioSources.Length];

        for (int i = 0; i < audioNames.Length; i++)
        {
            audioNames[i] = audioSources[i].clip.name;
        }

    }

    public void PlaySound(string nameSound)
    {
        audioSources[Array.IndexOf(audioNames, nameSound)].Play();
    }

    public void SetMusicVol (float volume)
    {
        audioMixer.SetFloat("MusicVol", volume);
    }
    public void SetSoundsVol(float volume)
    {
        audioMixer.SetFloat("SoundsVol", volume);
    }

    public float GetMusicVol()
    {
        return PlayerPrefs.GetFloat("MusicVol", -20f);
       
    }
    public float GetSoundsVol()
    {
        return PlayerPrefs.GetFloat("SoundsVol", 0);
    }

    public void SaveCuttentSettingVol(float musicVol, float soundsVol)
    {
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("SoundsVol", soundsVol);
    }

}
