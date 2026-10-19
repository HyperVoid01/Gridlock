using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Stats stats;
    
    [SerializeField] private TextMeshProUGUI currentRound;
    [SerializeField] private Slider roundMeter;
    [SerializeField] private Button startRoundButton;
    [SerializeField] private TextMeshProUGUI powerSupplyHP;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI energyRegenText;

    [SerializeField] private GameObject shopHUD;
    [SerializeField] private GameObject shopHidePosition;
    [SerializeField] private GameObject shopShowPosition;
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject settingsMenu;
    
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        sfxVolumeSlider.value = stats.sfxVolume;
        musicVolumeSlider.value = stats.musicVolume;
        
        UpdateRoundCounter(1, true);
        
        UpdateEnergy(GameManager.Instance.energy);
    }

    private void Update()
    {
        UpdateEnergyRegen(CityPowerSupply.Instance.energyRegen);
        UpdateVolume();
    }
    
    public void UpdateVolume()
    {
        stats.sfxVolume = sfxVolumeSlider.value;
        stats.musicVolume = musicVolumeSlider.value;
    }

    public void UpdateRoundCounter(int round, bool onBuildStage)
    {
        if (onBuildStage)
        {
            currentRound.text = "BUILD STAGE";
        }
        else
        {
            currentRound.text = "Round : " + round;
        }
    }

    public void UpdateRoundMeter(int currentEnemies = 0, int totalEnemies = 0)
    {
        roundMeter.maxValue = totalEnemies;
        roundMeter.value = currentEnemies;
    }

    public void OnStartRoundButtonClicked()
    {
        AudioManager.Instance.Play("ButtonClick");
        RoundManager.Instance.NextWave();
    }

    public void ShowShopHUD()
    {
        StartCoroutine(SlideInShop());
    }

    public void HideShopHUD()
    {
        StartCoroutine(SlideOutShop());
    }

    private IEnumerator SlideInShop() // Show
    {
        RectTransform rectTransform = shopHUD.GetComponent<RectTransform>();
        RectTransform rectTransform2 = shopShowPosition.GetComponent<RectTransform>();

        while (Vector3.Distance(rectTransform.position, rectTransform2.position) > 0.1f)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, rectTransform2.position, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        // Snap to final position to be safe
        rectTransform.position = rectTransform2.position;
    }
    
    private IEnumerator SlideOutShop() // Remove
    {
        RectTransform rectTransform = shopHUD.GetComponent<RectTransform>();
        RectTransform rectTransform2 = shopHidePosition.GetComponent<RectTransform>();

        while (Vector3.Distance(rectTransform.position, rectTransform2.position) > 0.1f)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, rectTransform2.position, 0.1f);
            yield return new WaitForFixedUpdate();
        }

        // Snap to target
        rectTransform.position = rectTransform2.position;
    }

    public void UpdateHP(int hp)
    {
        powerSupplyHP.text = $"{hp} / 100";
    }

    public void UpdateEnergyRegen(int energyRegen)
    {
        energyRegenText.text = energyRegen.ToString();
    }

    public void PlaceTower(GameObject tower)
    {
        AudioManager.Instance.Play("ButtonClick");
        BuildMode.Instance.PlaceTower(tower);
    }

    public void UpdateEnergy(int energy)
    {
        energyText.text = energy.ToString();
    }

    public void ShowPauseScreen()
    {
        if (!GameManager.Instance.gameOver)
        {
            AudioManager.Instance.Play("ButtonClick");
            pauseMenu.SetActive(true);
            gameHUD.SetActive(false);
            settingsMenu.SetActive(false);
        }
    }

    public void HidePauseScreen()
    {
        AudioManager.Instance.Play("ButtonClick");
        pauseMenu.SetActive(false);
        gameHUD.SetActive(true);
    }

    public void OnSettingsButtonClicked()
    {
        AudioManager.Instance.Play("ButtonClick");
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    public void OnMainMenuButtonClicked()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("ButtonClick");
        SceneManager.LoadScene("MainMenu");
    }
    
    public void ShowGameOverMenu()
    {
        pauseMenu.SetActive(false);
        gameHUD.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        AudioManager.Instance.Play("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
