using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public GameObject menu;
    public GameObject gamePanel;
    public GameObject mainMenu;
    public GameObject settingMenu;
    public Button resumeGameButton;
    public SoundsController soundsController;
    public Slider music;
    public Slider sounds;

    float musicVol;
    float soundsVol;

    private void Start()
    {

        if (music != null) 
        {
            musicVol = soundsController.GetMusicVol();
            music.value = musicVol; 
        }
        if (sounds != null)
        {
            soundsVol = soundsController.GetSoundsVol();
            sounds.value = soundsVol;
        }
    }
    public void StartGameButton()
    {
       SceneManager.LoadScene("Game");
       Time.timeScale = 1;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ResumeGameButton()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void SettingOpenButton()
    {
        CloseAndOpenSetting();
    }

    public void AcceptSettingButton()
    {
        soundsController.SaveCuttentSettingVol(musicVol, soundsVol);
        soundsController.SetMusicVol(musicVol);
        soundsController.SetSoundsVol(soundsVol);
        CloseAndOpenSetting();
    }
    public void CancelSettingButton()
    {
        CloseAndOpenSetting();
    }

    public void MusicSlider (float musicVol)
    {
        this.musicVol = musicVol;
    }
    public void SoundsSlider(float soundsVol)
    {
        this.soundsVol = soundsVol;
    }

    void CloseAndOpenSetting()
    {
        settingMenu.SetActive(!settingMenu.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
    public void PauseGame()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Death()
    {
        menu.SetActive(true);
        resumeGameButton.GetComponent<Button>().interactable = false;
    }
}
