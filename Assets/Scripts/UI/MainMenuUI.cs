using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Stats stats;
    
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    public void OnPlayGameButtonClick()
    {
        AudioManager.Instance.Play("ButtonClick");
        AudioManager.Instance.Stop("MainMenuMusic");
        SceneManager.LoadScene("GameScene");
        AudioManager.Instance.Play("BuildStage");
    }

    public void Start()
    {
        AudioManager.Instance.Play("MainMenuMusic");
        sfxVolumeSlider.value = stats.sfxVolume;
        musicVolumeSlider.value = stats.musicVolume;
    }

    public void Update()
    {
        UpdateVolume();
    }

    public void OnQuitGameButtonClick()
    {
        stats.doneTutorial = false;
        AudioManager.Instance.Play("ButtonClick");
        Application.Quit();
    }

    public void OnSettingsButtonClick()
    {
        AudioManager.Instance.Play("ButtonClick");
        sfxVolumeSlider.value = stats.sfxVolume;
        musicVolumeSlider.value = stats.musicVolume;
        
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void OnMainMenuButtonClick()
    {
        AudioManager.Instance.Play("ButtonClick");
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void UpdateVolume()
    {
        stats.sfxVolume = sfxVolumeSlider.value;
        stats.musicVolume = musicVolumeSlider.value;
    }
}
