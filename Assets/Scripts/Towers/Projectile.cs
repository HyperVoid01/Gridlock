using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyAI>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
