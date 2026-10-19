using System.Collections;
using UnityEngine;

public class CityPowerSupply : MonoBehaviour
{
    [SerializeField] private int powerSupplyHealth = 100;
    public int energyRegen = 2; // Energy that regens every round

    [SerializeField] private float endGameDelay = 1f;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem destroyParticles;
    
    public static CityPowerSupply Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TakeDamage(int amount)
    {
        if (powerSupplyHealth > 0 && powerSupplyHealth - amount > 0)
        {
            powerSupplyHealth -= amount;
            damageParticles.Play();
            AudioManager.Instance.Play("PowerSupplyDamage");
        }

        if (powerSupplyHealth - amount <= 0 && !GameManager.Instance.gameOver)
        {
            powerSupplyHealth = 0;
            destroyParticles.Play();
            AudioManager.Instance.Play("PowerSupplyDestroyed");
            StartCoroutine(EndGame());
        }
        
        UIManager.Instance.UpdateHP(powerSupplyHealth);
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(endGameDelay);
        GameManager.Instance.GameOver();
    }
}
