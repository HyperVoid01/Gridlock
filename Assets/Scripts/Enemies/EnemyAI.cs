using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private SphereCollider _sphereCollider;
    public GameObject powerSupply;
    public GameObject target;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectionRange;

    [SerializeField] private int enemyRanking = 1;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        powerSupply = GameObject.FindGameObjectWithTag("PowerSupply");
    }

    private void Start()
    {
        _sphereCollider.radius = detectionRange;

        if (powerSupply)
        {
            target = powerSupply;
            agent.SetDestination(target.transform.position);
        }

        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (!target)
        {
            target = powerSupply;
            agent.SetDestination(target.transform.position);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Generator")) return;

        if (Vector3.Distance(transform.position, other.transform.position) <
            Vector3.Distance(transform.position, powerSupply.transform.position) && target == powerSupply)
        {
            target = other.gameObject;
            agent.SetDestination(target.transform.position);
        }
        else if (Vector3.Distance(transform.position, other.transform.position) <
                 Vector3.Distance(transform.position, target.transform.position) && target != powerSupply)
        {
            target = other.gameObject;
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Generator")) return;

        if (Vector3.Distance(transform.position, other.transform.position) <
            Vector3.Distance(transform.position, powerSupply.transform.position) && target == powerSupply)
        {
            target = other.gameObject;
            agent.SetDestination(target.transform.position);
        }
        else if (Vector3.Distance(transform.position, other.transform.position) <
                 Vector3.Distance(transform.position, target.transform.position) && target != powerSupply)
        {
            target = other.gameObject;
            agent.SetDestination(target.transform.position);
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= attackRange && target == powerSupply)
            {
                agent.SetDestination(transform.position);
                //target = gameObject;
                CityPowerSupply.Instance.TakeDamage(damage);
            }
            else if (Vector3.Distance(transform.position, target.transform.position) <= attackRange &&
                     target != powerSupply && target)
            {
                //target = gameObject;
                agent.SetDestination(transform.position);
                target.GetComponent<Generator>().TakeDamage(damage);
            }
            yield return new WaitForSeconds(attackRate);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            switch (enemyRanking)
            {
                case 1:
                {
                    AudioManager.Instance.Play("Robot1");
                    break;
                }
                
                case 2:
                {
                    AudioManager.Instance.Play("Robot2");
                    break;
                }
                
                case 3:
                {
                    AudioManager.Instance.Play("Robot3");
                    break;
                }
            }
            
            EnemySpawner.Instance.currentEnemies.Remove(gameObject);
            RoundManager.Instance.enemiesDefeated++;
            Destroy(gameObject);
        }
    }
}
