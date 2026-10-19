using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int currentWave = 0;
    public int maxWaveCount = 10;

    public bool onBuildStage = true;
    public int EnemiesInWave => EnemySpawner.Instance.currentEnemies.Count;

    public Vector2 commonEnemies;
    public Vector2 intermediateEnemies;
    public Vector2 hardEnemies;

    public float difficultyScaler = 1.5f;
    public int energyReductionModifier = 1;
    
    public int totalEnemies;
    public int enemiesDefeated = 0;
    
    public NavMeshSurface surface;
    
    public static RoundManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Update()
    {
        if (enemiesDefeated == totalEnemies && !onBuildStage)
        {
            AudioManager.Instance.Stop("BattleStage");
            AudioManager.Instance.Play("BuildStage");
            UIManager.Instance.ShowShopHUD();
            onBuildStage = true;
            totalEnemies = 0;
            enemiesDefeated = 0;
            GameManager.Instance.energy += CityPowerSupply.Instance.energyRegen;
            UIManager.Instance.UpdateEnergy(GameManager.Instance.energy);
        }
        
        if (EnemiesInWave == 0 && onBuildStage)
        {
            UIManager.Instance.UpdateRoundCounter(currentWave, true);
            UIManager.Instance.UpdateRoundMeter(0, 1);
        }
        else
        {
            UIManager.Instance.UpdateRoundMeter(enemiesDefeated, totalEnemies);
        }
    }

    public void NextWave()
    {
        if (currentWave < maxWaveCount && onBuildStage)
        {
            AudioManager.Instance.Stop("BuildStage");
            AudioManager.Instance.Play("BattleStage");
            //surface.BuildNavMesh();
            
            onBuildStage = false;
            currentWave++;
            UIManager.Instance.UpdateRoundCounter(currentWave, false);
            BuildMode.Instance.ExitBuildMode();

            ScaleDifficulty();
            
            int numCommon = (int)Random.Range(commonEnemies.x, commonEnemies.y);
            int numIntermediate = (int)Random.Range(intermediateEnemies.x, intermediateEnemies.y);
            int numHard = (int)Random.Range(hardEnemies.x, hardEnemies.y);
            
            totalEnemies = numCommon + numIntermediate + numHard;

            EnemySpawner.Instance.SpawnEnemies(numCommon, numIntermediate, numHard);
            
            EnergyReduction();
            TowerExplosions();
        }
    }
    
    private void ScaleDifficulty() // Increases number of enemies
    {
        if (currentWave == 1) return;
        
        commonEnemies *= difficultyScaler;
        intermediateEnemies *= difficultyScaler;
        hardEnemies *= difficultyScaler;
    }

    private void EnergyReduction() // Reduces energy regen every round if player owns any bad generators
    {
        if (GameManager.Instance.badGenerators.Count == 0) return;
        
        CityPowerSupply.Instance.energyRegen -= GameManager.Instance.badGenerators.Count * energyReductionModifier;
    }

    private void TowerExplosions() // Randomly destroys bad towers
    {
        Tower[] badTowers = GameManager.Instance.badTowers.ToArray();
        foreach (Tower badTower in badTowers)
        {
            badTower.ExplosionChance();
        }
    }
}
