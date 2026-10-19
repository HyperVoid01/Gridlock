using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] commonEnemyPrefabs;
    [SerializeField] private GameObject[] intermediateEnemyPrefabs;
    [SerializeField] private GameObject[] hardEnemyPrefabs;
    public List<GameObject> currentEnemies;
    
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Vector2 spawnRate;
    [SerializeField] private float waitTime;

    public static EnemySpawner Instance;

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
        currentEnemies = new List<GameObject>();
    }
    
    public void SpawnEnemies(int common, int intermediate, int hard)
    {
        if (common > 0)
            StartCoroutine(SpawnCommon(common));
        if (intermediate > 0)
            StartCoroutine(SpawnIntermediate(intermediate));
        if (hard > 0)
            StartCoroutine(SpawnHard(hard));
    }

    private IEnumerator SpawnCommon(int enemies)
    {
        yield return new WaitForSeconds(waitTime);
        
        for (int i = 0; i < enemies; i++)
        {
            currentEnemies.Add(Instantiate(commonEnemyPrefabs[Random.Range(0, commonEnemyPrefabs.Length - 1)], spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, Quaternion.identity));
            yield return new WaitForSeconds(Random.Range(spawnRate.x, spawnRate.y));
        }
    }

    private IEnumerator SpawnIntermediate(int enemies)
    {
        yield return new WaitForSeconds(waitTime);
        
        for (int i = 0; i < enemies; i++)
        {
            currentEnemies.Add(Instantiate(intermediateEnemyPrefabs[Random.Range(0, intermediateEnemyPrefabs.Length - 1)], spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, Quaternion.identity));
            yield return new WaitForSeconds(Random.Range(spawnRate.x, spawnRate.y));
        }
    }

    private IEnumerator SpawnHard(int enemies)
    {
        yield return new WaitForSeconds(waitTime);
        
        for (int i = 0; i < enemies; i++)
        {
            currentEnemies.Add(Instantiate(hardEnemyPrefabs[Random.Range(0, hardEnemyPrefabs.Length - 1)], spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, Quaternion.identity));
            yield return new WaitForSeconds(Random.Range(spawnRate.x, spawnRate.y));
        }
    }
}
