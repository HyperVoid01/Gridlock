using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public bool isPaused = false;
    public int energy = 10;
    public int towersPerGenerator = 2;

    // Turret Indexes
    [Header("Towers")]
    public List<Tower> windTowers;
    public List<Tower> hydroTowers;
    public List<Tower> solarTowers;
    public List<Tower> badTowers;
    
    // Generator Indexes
    [Header("Generators")]
    public List<Generator> windGenerators;
    public List<Generator> hydroGenerators;
    public List<Generator> solarGenerators;
    public List<Generator> badGenerators;

    public static GameManager Instance;

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
        PauseGame();
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            PauseGame();
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            ResumeGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        UIManager.Instance.ShowPauseScreen();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        UIManager.Instance.HidePauseScreen();
    }

    public void GameOver()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("Lose");
        isPaused = false;
        gameOver = true;
        Time.timeScale = 0;
        UIManager.Instance.ShowGameOverMenu();
    }

    public int GetTowerNumber(EnergyTypes energyType)
    {
        switch (energyType)
        {
            case EnergyTypes.Wind:
            {
                return windTowers.Count;
            }

            case EnergyTypes.Hydro:
            {
                return hydroTowers.Count;
            }

            case EnergyTypes.Solar:
            {
                return solarTowers.Count;
            }

            case EnergyTypes.Bad:
            {
                return badTowers.Count;
            }
        }
        
        return 0;
    }
    
    public int GetGeneratorNumber(EnergyTypes energyType)
    {
        switch (energyType)
        {
            case EnergyTypes.Wind:
            {
                return windGenerators.Count;
            }

            case EnergyTypes.Hydro:
            {
                return hydroGenerators.Count;
            }

            case EnergyTypes.Solar:
            {
                return solarGenerators.Count;
            }

            case EnergyTypes.Bad:
            {
                return badGenerators.Count;
            }
        }
        
        return 0;
    }

    public bool CheckAllTowersAvailability()
    {
        int totalTowers = windGenerators.Count + hydroGenerators.Count + solarTowers.Count + badTowers.Count;
        int totalGenerator = windGenerators.Count + hydroGenerators.Count + solarGenerators.Count +  badGenerators.Count;

        int slots = totalGenerator * towersPerGenerator;
        
        if (slots > totalTowers) return true; // Available slots for more turrets
        return false; // No space for turrets
    }

    public bool CheckTowerAvailability(EnergyTypes energyType)
    {
        switch (energyType)
        {
            case EnergyTypes.Wind:
            {
                if (windTowers.Count < windGenerators.Count * towersPerGenerator) return true;
                break;
            }

            case EnergyTypes.Hydro:
            {
                if (hydroTowers.Count < hydroGenerators.Count * towersPerGenerator) return true;
                break;
            }

            case EnergyTypes.Solar:
            {
                if (solarTowers.Count < solarGenerators.Count * towersPerGenerator) return true;
                break;
            }

            case EnergyTypes.Bad:
            {
                if (badTowers.Count < badGenerators.Count * towersPerGenerator) return true;
                break;
            }
        }
        
        return false;
    }
    
    public int CheckTowerAvailabilityCount(EnergyTypes energyType)
    {
        int availableTowers = 0;
        switch (energyType)
        {
            case EnergyTypes.Wind:
            {
                availableTowers = windGenerators.Count * towersPerGenerator - windTowers.Count;
                break;
            }

            case EnergyTypes.Hydro:
            {
                availableTowers = hydroGenerators.Count * towersPerGenerator - hydroTowers.Count;
                break;
            }

            case EnergyTypes.Solar:
            {
                availableTowers = solarGenerators.Count * towersPerGenerator - solarTowers.Count;
                break;
            }

            case EnergyTypes.Bad:
            {
                availableTowers = badGenerators.Count * towersPerGenerator - badTowers.Count;
                break;
            }
        }
        
        return availableTowers;
    }
    
    public int CheckTowerActiveCount(EnergyTypes energyType)
    {
        int activeTowers = 0;
        switch (energyType)
        {
            case EnergyTypes.Wind:
            {
                foreach (Tower tower in windTowers)
                {
                    if (tower.isActive) activeTowers++;
                }
                break;
            }

            case EnergyTypes.Hydro:
            {
                foreach (Tower tower in hydroTowers)
                {
                    if (tower.isActive) activeTowers++;
                }
                break;
            }

            case EnergyTypes.Solar:
            {
                foreach (Tower tower in solarTowers)
                {
                    if (tower.isActive) activeTowers++;
                }
                break;
            }

            case EnergyTypes.Bad:
            {
                foreach (Tower tower in badTowers)
                {
                    if (tower.isActive) activeTowers++;
                }
                break;
            }
        }
        
        return activeTowers;
    }
    
    public void AddToTowerCount(Tower tower)
    {
        switch (tower.energyType)
        {
            case EnergyTypes.Wind:
            {
                windTowers.Add(tower);
                break;
            }

            case EnergyTypes.Hydro:
            {
                hydroTowers.Add(tower);
                break;
            }

            case EnergyTypes.Solar:
            {
                solarTowers.Add(tower);
                break;
            }

            case EnergyTypes.Bad:
            {
                badTowers.Add(tower);
                break;
            }
        }
    }
    
    public void AddToGeneratorCount(Generator generator)
    {
        switch (generator.energyType)
        {
            case EnergyTypes.Wind:
            {
                windGenerators.Add(generator);
                break;
            }

            case EnergyTypes.Hydro:
            {
                hydroGenerators.Add(generator);
                break;
            }

            case EnergyTypes.Solar:
            {
                solarGenerators.Add(generator);
                break;
            }

            case EnergyTypes.Bad:
            {
                badGenerators.Add(generator);
                break;
            }
        }
    }
    
    public void RemoveFromTowerCount(Tower tower)
    {
        switch (tower.energyType)
        {
            case EnergyTypes.Wind:
            {
                windTowers.Remove(tower);
                break;
            }

            case EnergyTypes.Hydro:
            {
                hydroTowers.Remove(tower);
                break;
            }

            case EnergyTypes.Solar:
            {
                solarTowers.Remove(tower);
                break;
            }

            case EnergyTypes.Bad:
            {
                badTowers.Remove(tower);
                break;
            }
        }
    }
    
    public void RemoveFromGeneratorCount(Generator generator)
    {
        switch (generator.energyType)
        {
            case EnergyTypes.Wind:
            {
                windGenerators.Remove(generator);
                break;
            }

            case EnergyTypes.Hydro:
            {
                hydroGenerators.Remove(generator);
                break;
            }

            case EnergyTypes.Solar:
            {
                solarGenerators.Remove(generator);
                break;
            }

            case EnergyTypes.Bad:
            {
                badGenerators.Remove(generator);
                break;
            }
        }
    }

    public void RecalculateTowerAvailabilities(EnergyTypes energyType)
    {
        if (CheckTowerAvailabilityCount(energyType) >= 0) // Enabling towers if slots are available
        {
            switch (energyType)
            {
                case EnergyTypes.Wind:
                {
                    int i = windTowers.Count - CheckTowerActiveCount(energyType);
                    foreach (Tower tower in windTowers)
                    {
                        if (!tower.isActive && i > 0)
                        {
                            tower.EnableTower();
                            i--;
                        }

                        if (i == 0)
                        {
                            return;
                        }
                    }
                    break;
                }

                case EnergyTypes.Hydro:
                {
                    int i = hydroTowers.Count - CheckTowerActiveCount(energyType);
                    foreach (Tower tower in hydroTowers)
                    {
                        if (!tower.isActive && i > 0)
                        {
                            tower.EnableTower();
                            i--;
                        }

                        if (i == 0)
                        {
                            return;
                        }
                    }
                    break;
                }

                case EnergyTypes.Solar:
                {
                    int i = solarTowers.Count - CheckTowerActiveCount(energyType);
                    foreach (Tower tower in solarTowers)
                    {
                        if (!tower.isActive && i > 0)
                        {
                            tower.EnableTower();
                            i--;
                        }

                        if (i == 0)
                        {
                            return;
                        }
                    }
                    break;
                }

                case EnergyTypes.Bad:
                {
                    int i = badTowers.Count - CheckTowerActiveCount(energyType);
                    foreach (Tower tower in badTowers)
                    {
                        if (!tower.isActive && i > 0)
                        {
                            tower.EnableTower();
                            i--;
                        }

                        if (i == 0)
                        {
                            return;
                        }
                    }
                    break;
                }
            }
        }
        else // Disabling towers if no generators are available
        {
            switch (energyType)
            {
                case EnergyTypes.Wind:
                {
                    for (int i = CheckTowerAvailabilityCount(energyType); i < 0; i++)
                    {
                        windTowers[windTowers.Count + i].DisableTower();
                    }
                    break;
                }

                case EnergyTypes.Hydro:
                {
                    for (int i = CheckTowerAvailabilityCount(energyType); i < 0; i++)
                    {
                        hydroTowers[hydroTowers.Count + i].DisableTower();
                    }
                    break;
                }

                case EnergyTypes.Solar:
                {
                    for (int i = CheckTowerAvailabilityCount(energyType); i < 0; i++)
                    {
                        solarTowers[solarTowers.Count + i].DisableTower();
                    }
                    break;
                }

                case EnergyTypes.Bad:
                {
                    for (int i = CheckTowerAvailabilityCount(energyType); i < 0; i++)
                    {
                        badTowers[badTowers.Count + i].DisableTower();
                    }
                    break;
                }
            }
        }
    }
}
