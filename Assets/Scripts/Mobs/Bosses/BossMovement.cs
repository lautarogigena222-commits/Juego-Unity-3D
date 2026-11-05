using TMPro;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    public UnityEngine.UI.Image barraVida;
    [SerializeField] private GameObject bossHealthPanel;


    [Header("Portal")]
    [SerializeField] private GameObject portalNextZone;



    [Header("Damage")]
    [SerializeField] private float damage = 3;
    public float Damge => damage;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.2f;
    public GameObject target;

    [Header("Animator")]
    private Animator animator;
    [SerializeField] private float deathDelay = 3f;

    [Header("Música")]
    [SerializeField] private MusicManager musicManager;


    [Header("Drop")]
    [SerializeField] private GameObject Reliquia;
    [SerializeField] int drop;
    [SerializeField] float expBoss;
    public float ExpBoss => expBoss;

    [Header("Jefe final")]
    [SerializeField] private bool isFinalBoss = false;
    [SerializeField] private float victoryDelay = 2f;

    [SerializeField] private int relicNumber = 1;

    private float nextAttackTime = 1f;
    private bool musicaCombateActivada = false;
    private bool isDead;




    void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)  animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player");

        if (musicManager == null)
            musicManager = FindFirstObjectByType<MusicManager>();

        if (portalNextZone != null)
            portalNextZone.SetActive(false);

        ActualizarBarra();
        //barraVida.fillAmount = (float)currentHealth / maxHealth;



    }

    public void SetUI(UnityEngine.UI.Image barra, GameObject panel)
    {
        barraVida = barra;
        bossHealthPanel = panel;
        ActualizarBarra();
    }

    void Update()
    {
        if (isDead || target == null) return;
        Debug.Log("Boss intenta atacar");

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance >= detectionRange)
        {
            SetWalking(false);
            return;
        }

        if (!musicaCombateActivada && musicManager != null)
        {
            musicManager.BossAparecio();
            musicaCombateActivada = true;
        }

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }

        if (distance > attackRange)
        {
            // Persigue al jugador
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            SetWalking(true);
        }
        else
        {
            // Está en rango: se detiene y ataca
            SetWalking(false);

            if (Time.time >= nextAttackTime)
            {
                if (animator != null)
                    animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void SetWalking(bool walking)
    {
        if (animator != null)
            animator.SetBool("IsWalking", walking);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletMovement>().Damage);


        }*/
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponentInParent<PlayerStats>();
            if (stats != null)
                TakeDamage(stats.PlayerDamage);
        }

    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        ActualizarBarra();
        //barraVida.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth <= 0)
            Morir();
    }

    private void Morir()
    {
        isDead = true;
        currentHealth = 0;
        ActualizarBarra();

        if (musicaCombateActivada && musicManager != null)
        {
            musicManager.BossMurio();
            musicaCombateActivada = false;
        }

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);

        if (portalNextZone != null)
            portalNextZone.SetActive(true);

        DropReliquia();

        // Evita que siga recibiendo golpes mientras se reproduce la muerte
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        SetWalking(false);
        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, deathDelay);
    }

    private void DropReliquia()
    {
        if (Reliquia == null) return;

        GameObject relic = Instantiate(Reliquia, transform.position + Vector3.up, Quaternion.identity);

        RelicReward reward = relic.GetComponent<RelicReward>();
        if (reward != null)
            reward.SetRelicNumber(relicNumber);

        if (isFinalBoss && VictoryUI.Instance != null)
            VictoryUI.Instance.ShowDelayed(victoryDelay);
    }



    private void ActualizarBarra()
    {
        if (barraVida != null && maxHealth > 0f)
            barraVida.fillAmount = currentHealth / maxHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void HitPlayer()
    {
        Debug.Log("HitPlayer llamado");
        if (isDead || target == null) return;

        // Solo daña si el jugador sigue cerca cuando cae el golpe de la animación
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRange + 1f) return;

        PlayerStats stats = target.GetComponentInParent<PlayerStats>();
        if (stats != null)
            stats.TakeDamage(damage);
    }

}