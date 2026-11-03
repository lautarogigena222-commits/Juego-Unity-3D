using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;

    public GameObject target;
    private Spawn spawner;
    private Animator animator;

    [SerializeField] private float damage = 3f;
    public float Damge => damage;

    [SerializeField] private float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    [SerializeField] private float enemyLife = 10f;
    [SerializeField] private GameObject moneda;
    [SerializeField] private float expEnemy = 10f;
    public float ExpEnemy => expEnemy;

    private bool isDead = false;

    void Start()
    {
        PlayerStats playerStatsInstance = FindFirstObjectByType<PlayerStats>();
        if (playerStatsInstance != null)
        {
            target = playerStatsInstance.gameObject;
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= detectionRange && distance > attackRange)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero && direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
            }

            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (animator != null) animator.SetBool("IsWalking", true);
        }
        else if (distance <= attackRange)
        {
            if (animator != null) animator.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                if (animator != null) animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            if (animator != null) animator.SetBool("IsWalking", false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        enemyLife -= amount;
        if (enemyLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        BossTrigger.SumarEnemigoDerrotado();

        if (target != null)
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.MasExp(expEnemy);
            }
        }

        if (spawner != null)
        {
            spawner.OnEnemyDied();
        }

        DropMoneda();

        Destroy(gameObject);
    }

    public void DropMoneda()
    {
        int drop = UnityEngine.Random.Range(0, 2);
        if (drop == 1 && moneda != null)
        {
            Instantiate(moneda, transform.position + Vector3.up * 1f, transform.rotation);
        }
    }

    public void SetSpawner(Spawn s)
    {
        spawner = s;
    }

    public void HitPlayer()
    {
        if (target != null)
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}