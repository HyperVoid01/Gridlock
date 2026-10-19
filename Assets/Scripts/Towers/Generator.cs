using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    [Header("Energy")]
    public EnergyTypes energyType;
    public int energyCost;
    [SerializeField] private int energyProduction;
    
    [Header("Feedback Particles")]
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem destroyParticles;

    private void Start()
    {
        AudioManager.Instance.Play("TowerPlaced");
        CityPowerSupply.Instance.energyRegen += energyProduction;
        GameManager.Instance.RecalculateTowerAvailabilities(energyType);
    }

    public void TakeDamage(int damage)
    {
        if (health > 0 && health - damage > 0)
        {
            health -= damage;
            damageParticles.Play();
            return;
        }
        
        if (health - damage <= 0)
        {
            CityPowerSupply.Instance.energyRegen -= energyProduction;
            GameManager.Instance.RemoveFromGeneratorCount(this);
            GameManager.Instance.RecalculateTowerAvailabilities(energyType);
            
            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        CameraController.Instance.CameraShake();
        
        Vector3 origin = transform.position;
        
        for (int i = 0; i < 20; i++)
        {
            Vector3 destination = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y, transform.position.z + Random.Range(-1f, 1f));
            transform.position = destination;
            yield return new WaitForSeconds(0.05f);
            transform.position = origin;
        }
        
        destroyParticles.Play();
        AudioManager.Instance.Play("TowerDestroyed");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
