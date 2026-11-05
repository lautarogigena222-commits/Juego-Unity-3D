using TMPro;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    public UnityEngine.UI.Image barraVida;
    [SerializeField] private GameObject bossHealthPanel;

    [Header("Portal")]
    [SerializeField] private GameObject portalNextZone;

    [Header("Damage")]
    [SerializeField] private float damage = 3f;
    public float Damge => damage;
    [SerializeField] private bool damageByAnimationEvent = true;
    [SerializeField] private float hitDelay = 0.5f;

    [Header("Daño por contacto")]
    [SerializeField] private float contactDamage = 3f;
    [SerializeField] private float contactCooldown = 1f;
    [SerializeField] private float contactRange = 2.5f;
    [SerializeField] private float contactHeight = 3f;
    private float nextContactTime;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.2f;
    public GameObject target;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = 3f;

    [Header("Música")]
    [SerializeField] private MusicManager musicManager;

    [Header("Drop")]
    [SerializeField] private GameObject Reliquia;
    [SerializeField] private int relicNumber = 1;
    [SerializeField] private float expBoss;
    public float ExpBoss => expBoss;

    [Header("Jefe final")]
    [SerializeField] private bool isFinalBoss = false;
    [SerializeField] private float victoryDelay = 2f;

    private float nextAttackTime = 1f;
    private bool musicaCombateActivada = false;
    private bool isDead;
    private Vector3 startPosition;
    private PlayerStats playerStats;

    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
            playerStats = target.GetComponentInParent<PlayerStats>();

        if (playerStats == null)
            playerStats = FindFirstObjectByType<PlayerStats>();

        if (musicManager == null)
            musicManager = FindFirstObjectByType<MusicManager>();

        if (portalNextZone != null)
            portalNextZone.SetActive(false);

        ActualizarBarra();
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

        if (playerStats != null && playerStats.IsDead)
        {
            SetWalking(false);
            return;
        }

        CheckContactDamage();

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance >= detectionRange)
        {
            SetWalking(false);
            return;
        }

        if (!musicaCombateActivada)
        {
            musicaCombateActivada = true;

            if (musicManager != null)
                musicManager.BossAparecio();

            if (bossHealthPanel != null)
                bossHealthPanel.SetActive(true);
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
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            SetWalking(true);
        }
        else
        {
            SetWalking(false);

            if (Time.time >= nextAttackTime)
            {
                if (animator != null)
                    animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackCooldown;

                if (!damageByAnimationEvent)
                    Invoke(nameof(AplicarDano), hitDelay);
            }
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

    private void SetWalking(bool walking)
    {
        if (animator != null)
            animator.SetBool("IsWalking", walking);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        ActualizarBarra();

        if (currentHealth <= 0)
            Morir();
    }

    private void Morir()
    {
        isDead = true;
        currentHealth = 0;
        ActualizarBarra();
        CancelInvoke();

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

        if (isFinalBoss && VictoryUI.Instance != null)
            VictoryUI.Instance.ShowDelayed(victoryDelay);

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
    }

    // Lo llama el evento de la animación de ataque
    public void HitPlayer()
    {
        if (!damageByAnimationEvent) return;
        AplicarDano();
    }

    private void AplicarDano()
    {
        if (isDead || target == null || playerStats == null || playerStats.IsDead) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRange + 1f) return;

        playerStats.TakeDamage(damage);
    }

    private void ActualizarBarra()
    {
        if (barraVida != null && maxHealth > 0f)
            barraVida.fillAmount = currentHealth / maxHealth;
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerRespawn += ResetBoss;
        PlayerStats.OnPlayerDied += OcultarPanelVida;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerRespawn -= ResetBoss;
        PlayerStats.OnPlayerDied -= OcultarPanelVida;
    }

    private void OcultarPanelVida()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }

    private void ResetBoss()
    {
        if (isDead) return;

        CancelInvoke();

        if (musicaCombateActivada && musicManager != null)
        {
            musicManager.BossMurio();
            musicaCombateActivada = false;
        }

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);

        // El jefe final se destruye: se vuelve a invocar desde el portal
        if (isFinalBoss)
        {
            Destroy(gameObject);
            return;
        }

        // Los demás bosses se curan y vuelven a su lugar
        currentHealth = maxHealth;
        transform.position = startPosition;
        SetWalking(false);
        ActualizarBarra();
    }

    private void OnDestroy()
    {
        if (musicaCombateActivada && musicManager != null)
            musicManager.BossMurio();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, contactRange);
    }
}