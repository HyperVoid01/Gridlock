using System.Collections;
using UnityEngine;

[RequireComponent (typeof (SphereCollider))]
public class Tower : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float projectileSpeed;

    [Header("Targeting & Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    public GameObject target;
    public bool isActive;

    [Header("Energy Settings")]
    public EnergyTypes energyType;
    public ParticleSystem energyParticle;
    [SerializeField] private ParticleSystem explosionParticles;
    [Range(0, 100)] public float explosionChance;

    [Header("Internal References")]
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = range;
        EnableTower();
    }

    private void Update()
    {
        if (RoundManager.Instance.onBuildStage)
            _sphereCollider.enabled = false;
        else if (!RoundManager.Instance.onBuildStage)
            _sphereCollider.enabled = true;
    }

    public void EnableTower()
    {
        AudioManager.Instance.Play("TowerPlaced");
        isActive = true;
        StartCoroutine(FollowTarget());
        StartCoroutine(Shoot());
        energyParticle.Play();
    }

    public void DisableTower()
    {
        isActive = false;
        StopAllCoroutines();
        energyParticle.Play();
    }

    public IEnumerator FollowTarget()
    {
        while (true)
        {
            // Rotates Turret
            if (target)
            {
                // Get direction to target
                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0f;
                
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public IEnumerator Shoot()
    {
        while (true)
        {
            if (target)
            {
                // Shoots Projectile
                Rigidbody projectileRb = Instantiate(projectile, projectileSpawnPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                projectileRb.gameObject.GetComponent<Projectile>().damage = damage;
                Vector3 direction = (target.transform.position - transform.position).normalized;
                direction.y = 0f;
                projectileRb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
            }
            
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void ExplosionChance() // Has a chance to explode if tower is bad
    {
        if (energyType != EnergyTypes.Bad) return;
        
        float chance = Random.Range(0f, 100f);

        if (chance <= explosionChance)
        {
            
            GameManager.Instance.RemoveFromTowerCount(this);
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
        
        explosionParticles.Play();
        AudioManager.Instance.Play("TowerDestroyed");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!target && other.gameObject.CompareTag("Enemy"))
            target = other.gameObject;
        
        if (target && Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position) && other.gameObject.CompareTag("Enemy"))
            target = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!target && other.gameObject.CompareTag("Enemy"))
            target = other.gameObject;
        
        if (target && Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position) && other.gameObject.CompareTag("Enemy"))
            target = other.gameObject;
    }
}

public enum EnergyTypes
{
    Wind,
    Hydro,
    Solar,
    Bad
}
