using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private Transform target;
    [SerializeField] float damage = 5f;
    public float Damage => damage;

    public void SetTarget(Transform newTarget, float projectileDamage)
    {
        target = newTarget;
        damage = projectileDamage;
        Destroy(gameObject, lifeTime);
    }



    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
       
        

        if (other.CompareTag("Boss"))
        {
            AnubisMovement boss = other.GetComponent<AnubisMovement>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }
}