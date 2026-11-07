using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;

    public GameObject target;
    private Spawn spawner;
    private Animator animator;

    [Header("Daño")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip spawnSound;

    [Header("Daño")]
    [SerializeField] private float damage = 3f;
                     public float Damge => damage;
    [SerializeField] private float attackCooldown = 1.2f;        
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;
    private float nextAttackTime = 0f;

    [Header("Daño por contacto")]
    [SerializeField] private float contactDamage = 2f;
    [SerializeField] private float contactCooldown = 1f;
    [SerializeField] private float contactRange = 1.5f;
    [SerializeField] private float contactHeight = 2f;
    private float nextContactTime;


    [SerializeField] private float enemyLife = 10f;
    [SerializeField] private GameObject moneda;
    [SerializeField] private float expEnemy = 10f;
    public float ExpEnemy => expEnemy;
    private bool isDead = false;
    private PlayerStats playerStats;

    void Start()
    {
        PlayerStats playerStatsInstance = FindFirstObjectByType<PlayerStats>();
        if (playerStatsInstance != null)
        {
            playerStats = playerStatsInstance;
            target = playerStatsInstance.gameObject;
        }
        animator = GetComponent<Animator>();

    }
    public void PlaySpawnSound()
    {
        if (audioSource != null && spawnSound != null)
            audioSource.PlayOneShot(spawnSound);
    }

    void Update()
    {
        if (isDead || target == null) return;
        if (playerStats != null && playerStats.IsDead)
        {
            if (animator != null) animator.SetBool("IsWalking", false);
            return;
        }

        CheckContactDamage();

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

    private void CheckContactDamage()
    {
        if (Time.time < nextContactTime || playerStats == null) return;

        Vector3 a = transform.position;
        Vector3 b = playerStats.transform.position;

        float horizontal = Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
        float vertical = Mathf.Abs(a.y - b.y);

        if (horizontal > contactRange || vertical > contactHeight) return;

        nextContactTime = Time.time + contactCooldown;
        playerStats.TakeDamage(contactDamage);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        enemyLife -= amount;

        if (enemyLife <= 0)
        {
            Die();
        }
    }
    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private void Die()
    {
        isDead = true;

        BossTrigger.SumarEnemigoDerrotado();

        /*if (target != null)
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.MasExp(expEnemy);
            }
        }*/

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
        if (isDead || target == null || playerStats == null) return;
        if (playerStats.IsDead) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRange + 1f) return;

        playerStats.TakeDamage(damage);
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerDied += CancelarAtaque;
        PlayerStats.OnPlayerRespawn += CancelarAtaque;
        PlayerStats.OnPlayerRespawn += DespawnPorRespawn;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDied -= CancelarAtaque;
        PlayerStats.OnPlayerRespawn -= CancelarAtaque;
        PlayerStats.OnPlayerRespawn -= DespawnPorRespawn;
    }

    private void CancelarAtaque()
    {
        if (animator == null) return;

        animator.ResetTrigger("Attack");
        animator.SetBool("IsWalking", false);
        nextAttackTime = Time.time + attackCooldown;
    }

    private void DespawnPorRespawn()
    {
        if (isDead) return;
        isDead = true;

        if (spawner != null)
            spawner.OnEnemyDied();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, contactRange);
    }
}