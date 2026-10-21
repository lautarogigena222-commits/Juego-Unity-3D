using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    //Vida
    [SerializeField] private float life;
                    public float Life => life;
    [SerializeField] private float lifeMax;
    [SerializeField] private UnityEngine.UI.Image vida;

    
    //Daño
    [SerializeField] private int playerDamage;
    public int PlayerDamage => playerDamage;
    //[SerializeField] private LayerMask enemys;
    [SerializeField] private float attackRange;

 

    //Exp
    [SerializeField] float exp;
    [SerializeField] float expMax;
    [SerializeField] private UnityEngine.UI.Image experiencia;
    [SerializeField] float nivel;
    [SerializeField] float nivelMaximo;
    [SerializeField] private TextMeshProUGUI nivelTexto;

    //Mejora
    [SerializeField] private AutoShooter autoShooter;

    //Escenas
    [SerializeField] private Escena sceneController;





    void Start()
    {
        nivelTexto.text = "NIVEL: " + nivel.ToString();
    }

    
    void Update()
    {
        if (exp >= expMax)
        {
            nivel++;
            exp = 0;
            expMax += 10;
        }
        nivelTexto.text = "NIVEL: " + nivel.ToString();
       /* if(life <= 0)
        {
            DefeatScene();

        }*/

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            life -= collision.gameObject.GetComponent<Enemy>().Damge;

            vida.fillAmount = (float)life / lifeMax;
        }

        if (collision.gameObject.CompareTag("Animal"))
        {
            Destroy(collision.gameObject);
        }
        /* if (life <= 0)
         {
             DefeatScene();

         }*/

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
                life -= other.gameObject.GetComponent<AnubisMovement>().Damge;
                exp += other.gameObject.GetComponent<AnubisMovement>().ExpBoss;
            
            vida.fillAmount = (float)life / lifeMax;
            experiencia.fillAmount = (float)exp / expMax;
        }
       /* if (life <= 0)
        {
            DefeatScene();

        }*/
    }

    public void MasExp(float amount)
    {
        exp += amount;
        experiencia.fillAmount = (float)exp / expMax;
    }


    public void TakeDamage(float amount)
    {
        life -= amount;
        vida.fillAmount = (float)life / lifeMax;
    }



    public void ApplyUpgrade(UpgradeData data)
    {
        switch (data.upgradeType)
        {
            case UpgradeData.UpgradeType.IncreaseHealth:
                lifeMax += data.value;
                life += data.value;
                vida.fillAmount = (float)life / lifeMax;
                break;

            case UpgradeData.UpgradeType.IncreaseDamage:
                playerDamage += (int)data.value;
                break;

            case UpgradeData.UpgradeType.InstantHeal:
                life = Mathf.Min(life + data.value, lifeMax);
                vida.fillAmount = (float)life / lifeMax;
                break;
            case UpgradeData.UpgradeType.AutoShoot:
                if (autoShooter != null)
                {
                    autoShooter.EnableAutoShoot();
                }
                break;
                /*
                case UpgradeData.UpgradeType.IncreaseProjectileDamage:
                    if (autoShooter != null)
                    {
                        autoShooter.IncreaseDamage(data.value);
                    }
                    break;

                case UpgradeData.UpgradeType.IncreaseFireRate:
                    if (autoShooter != null)
                    {
                        autoShooter.IncreaseFireRate(data.value);
                    }
                    break;

                case UpgradeData.UpgradeType.IncreaseSpeed:
                    if (playerController != null)
                    {
                        playerController.IncreaseSpeed(data.value);
                    }
                    break;
                */

        }

        
    }

    /*public void DefeatScene()
    {
        if (life <= 0)
        {
            sceneController.ChangeScene("Lose");
        }
    }*/

}