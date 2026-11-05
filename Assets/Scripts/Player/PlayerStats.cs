using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] private float life;
                    public float Life => life;
    [SerializeField] private float lifeMax;
    [SerializeField] private UnityEngine.UI.Image vida;


    [Header("Daño")]
    [SerializeField] private int playerDamage;
    public int PlayerDamage => playerDamage;
    //[SerializeField] private LayerMask enemys;
    [SerializeField] private float attackRange;

    [Header("Muerte")]
    [SerializeField] private GameObject panelMuerte;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform spawnBase;
    [SerializeField] private float delayPanelMuerte = 2f;
    private bool isDead = false;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI MonedasTexto;
    private PlayerInventory inventory;
    private int ultimasMonedas = -1;



    //EXP y Nivel
    float exp;
     float expMax;
     private UnityEngine.UI.Image experiencia;
     float nivel;
     float nivelMaximo;
     private TextMeshProUGUI nivelTexto;





    //Mejora
    [SerializeField] private AutoShooter autoShooter;
    [SerializeField] private ThirdPersonController ThirdPersonController;

    //Escenas
    private Escena sceneController;




    void Awake()
    {
        if (autoShooter == null)
            autoShooter = GetComponent<AutoShooter>();

        if (ThirdPersonController == null)
            ThirdPersonController = GetComponent<ThirdPersonController>();

        inventory = GetComponent<PlayerInventory>();

    }

    void Start()
    {
        ActualizarMonedasUI();
    }

    
    void Update()
    {
        if (exp >= expMax)
        {
            nivel++;
            exp = 0;
            expMax += 10;
        }
        if (inventory != null && inventory.Coins != ultimasMonedas)
            ActualizarMonedasUI();
        //nivelTexto.text = "NIVEL: " + nivel.ToString();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    life -= collision.gameObject.GetComponent<Enemy>().Damge;

        //    vida.fillAmount = (float)life / lifeMax;
        //}

        if (collision.gameObject.CompareTag("Animal"))
        {
            Destroy(collision.gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health"))
        {
            life += other.gameObject.GetComponent<Drops>().HealFood;
            vida.fillAmount = (float)life / lifeMax;
            
            if (life > lifeMax)
            {
                life = lifeMax;
            }
        }
        if (other.gameObject.CompareTag("Boss"))
        {
                life -= other.gameObject.GetComponent<BossMovement>().Damge;
                exp += other.gameObject.GetComponent<BossMovement>().ExpBoss;
            
            vida.fillAmount = (float)life / lifeMax;
            experiencia.fillAmount = (float)exp / expMax;
        }

    }

    public void TakeCoins(int amount)
    {
        if (amount <= 0 || inventory == null) return;

        inventory.AddCoins(amount);
        ActualizarMonedasUI();
    }

    public void ActualizarMonedasUI()
    {
        if (MonedasTexto == null || inventory == null) return;

        ultimasMonedas = inventory.Coins;
        MonedasTexto.text = "Monedas: " + ultimasMonedas;
    }


    public void TakeDamage(float amount)
    {
        Debug.Log("Jugador recibe daño: " + amount);
        if (isDead) return;

        life -= amount;
        vida.fillAmount = (float)life / lifeMax;

        if (life <= 0)
            Die();
    }

    public void Curar(float amount)
    {
        if (isDead) return;

        life += amount;
        if (life > lifeMax)
        {
            life = lifeMax;
        }

        if (vida != null)
        {
            vida.fillAmount = (float)life / lifeMax;
        }
    }


    private void Die()
    {
        isDead = true;
        life = 0;
        vida.fillAmount = 0;

        if (playerAnimator != null)
            playerAnimator.SetBool("IsDead", true);

        // Frena el movimiento mientras se reproduce la animación
        if (ThirdPersonController != null)
            ThirdPersonController.enabled = false;

        StartCoroutine(MostrarPanelMuerte());
    }

    private IEnumerator MostrarPanelMuerte()
    {
        // Deja que se vea la animación antes de pausar
        yield return new WaitForSeconds(delayPanelMuerte);

        if (panelMuerte != null)
            panelMuerte.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Botón "Volver a la base"
    public void RespawnEnBase()
    {
        isDead = false;
        life = lifeMax;
        vida.fillAmount = 1f;

        if (playerAnimator != null)
            playerAnimator.SetBool("IsDead", false);

        CharacterController cc = GetComponent<CharacterController>();
        if (spawnBase != null && cc != null)
        {
            cc.enabled = false;
            transform.position = spawnBase.position;
            cc.enabled = true;
        }

        if (ThirdPersonController != null)
            ThirdPersonController.enabled = true;

        if (panelMuerte != null)
            panelMuerte.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ApplyUpgrade(UpgradeData data)
    {
        switch (data.upgradeType)
        {
            case UpgradeData.UpgradeType.IncreaseHealth:
                lifeMax += data.value;
                life += data.value;
                vida.fillAmount = life / lifeMax;
                break;

            case UpgradeData.UpgradeType.IncreaseDamage:
                playerDamage += (int)data.value;
                break;

            case UpgradeData.UpgradeType.InstantHeal:
                life = Mathf.Min(life + data.value, lifeMax);
                vida.fillAmount = life / lifeMax;
                break;

            case UpgradeData.UpgradeType.AutoShoot:
                if (autoShooter != null)
                    autoShooter.EnableAutoShoot();
                break;

            case UpgradeData.UpgradeType.IncreaseProjectileDamage:
                if (autoShooter != null)
                    autoShooter.IncreaseDamage(data.value);
                break;

            case UpgradeData.UpgradeType.IncreaseFireRate:
                if (autoShooter != null)
                    autoShooter.IncreaseFireRate(data.value);
                break;

            case UpgradeData.UpgradeType.IncreaseSpeed:
                if (ThirdPersonController != null)
                    ThirdPersonController.IncreaseSpeed(data.value);
                break;
        }
    }
}