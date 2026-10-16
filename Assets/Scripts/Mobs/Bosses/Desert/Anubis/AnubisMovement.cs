using UnityEngine;

public class AnubisMovement : MonoBehaviour
{
    //vida
    [SerializeField] float BossLife;
    [SerializeField] float lifeMax;

    public UnityEngine.UI.Image barraVida;


    //Atauqe
    /*[SerializeField] GameObject fire;
    [SerializeField] GameObject cabezaDisparo;
    [SerializeField] float timeBoss;*/
    


    //Daño
    [SerializeField] private float damage = 3;
                        public float Damge => damage;

    //Movimiento
    [SerializeField] float speed;
    [SerializeField] private float detectionRange;

    public GameObject target;



    //Drop
    [SerializeField] private GameObject Reliquia;
    [SerializeField] int drop;
    [SerializeField] float expBoss;
    public float ExpBoss => expBoss;

    [SerializeField] private Escena sceneController;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
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


            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(0, 180, 0);
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
        BossLife -= amount;

        barraVida.fillAmount = (float)BossLife / lifeMax;
        CheckDeath();
    }
    private void CheckDeath()
    {
        if (BossLife <= 0)
        {
            if (target != null)
            {
                PlayerStats playerStats = target.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.MasExp(expBoss);
                }
            }

            DropReliquia();  //Runa invocación jefe final
            Destroy(gameObject);
        }
    }

    public void DropReliquia()
    {
        if (BossLife <= 0)
        {

            Instantiate(Reliquia, transform.position, transform.rotation);
        }

    }

    /*public void WinScene()
    {
        if (BossLife <= 0)
        {
            sceneController.ChangeScene("Win");
        }
    }*/
}
