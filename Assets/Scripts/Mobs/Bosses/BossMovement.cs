using TMPro;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header ("Life")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    public UnityEngine.UI.Image barraVida;


    [SerializeField] private GameObject bossHealthPanel;


    [Header("Portal")]
    [SerializeField] private GameObject portalNextZone;


    //Atauqe
    /*[SerializeField] GameObject fire;
    [SerializeField] GameObject cabezaDisparo;
    [SerializeField] float timeBoss;*/



    [Header("Damage")]
    [SerializeField] private float damage = 3;
                        public float Damge => damage;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] private float detectionRange;

    public GameObject target;



    [Header("Drop")]
    [SerializeField] private GameObject Reliquia;
    [SerializeField] int drop;
    [SerializeField] float expBoss;
    public float ExpBoss => expBoss;




    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;

        if (portalNextZone != null)
            portalNextZone.SetActive(false);
        barraVida.fillAmount = (float)currentHealth / maxHealth;

    }


    void Update()
    {
        /*  if (target == null)
            return; */

        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        if (distance < detectionRange)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0f;


            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);


            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            /*timeBoss += Time.deltaTime;
            if (timeBoss > 2)
            {
                Quaternion fireRotation = Quaternion.LookRotation(direction.normalized);
                Instantiate(fire, cabezaDisparo.transform.position, transform.rotation);
                timeBoss = 0;
            }*/



        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletMovement>().Damage);
            

        }
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(other.gameObject.GetComponent<PlayerStats>().PlayerDamage);
            

        }
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        barraVida.fillAmount = (float)currentHealth / maxHealth;
        CheckDeath();
    }
    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            if (target != null)
            {
                PlayerStats playerStats = target.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.MasExp(expBoss);
                }
            }
            if (bossHealthPanel != null)
                bossHealthPanel.SetActive(false);
            Die();
            DropReliquia();
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        if (portalNextZone != null)
        {
            portalNextZone.SetActive(true);
            currentHealth = maxHealth;
        }

    }

    public void DropReliquia()
    {
        if (currentHealth <= 0)
        {

            Instantiate(Reliquia, transform.position, transform.rotation);
        }

    }
}
