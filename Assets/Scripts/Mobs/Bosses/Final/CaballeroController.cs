using UnityEngine;
using UnityEngine.AI;

public class JefeFinalCaballero : MonoBehaviour
{
    public int vida = 100;

    public Transform player;
    public NavMeshAgent agent;
    public Rigidbody rb;

    public GameObject bombaPrefab;
    public Transform puntoLanzamiento;
    public float fuerzaSalto = 15f;
    public float tiempoEntreAtaques = 3f;

    private float distanciaAlJugador;
    private bool estaSaltando = false;
    private float temporizadorAtaque = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    void Update()
    {
        if (player == null) return;

        distanciaAlJugador = Vector3.Distance(transform.position, player.position);

        if (temporizadorAtaque > 0)
        {
            temporizadorAtaque -= Time.deltaTime;
        }


        if (!estaSaltando)
        {
            if (distanciaAlJugador > 5f)
            {
                if (temporizadorAtaque <= 0)
                {
                    Salto();
                    temporizadorAtaque = tiempoEntreAtaques; 
                }
                else
                {
                    Buscar();
                }
            }
            else if (distanciaAlJugador >= 2f && distanciaAlJugador <= 5f)
            {
                if (temporizadorAtaque <= 0)
                {
                    Lanzar();
                    temporizadorAtaque = tiempoEntreAtaques; 
                }
                else
                {
                    Buscar();
                }
            }
            else
            {
                Buscar();
            }
        }
    }

    void Buscar()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.position);
        }
    }

    void Salto()
    {
        estaSaltando = true;

        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 direccionSalto = (player.position - transform.position).normalized;
        direccionSalto.y = 1f;

        rb.AddForce(direccionSalto * fuerzaSalto, ForceMode.Impulse);
    }

    void Lanzar()
    {
        if (bombaPrefab != null && puntoLanzamiento != null)
        {
           
            Instantiate(bombaPrefab, puntoLanzamiento.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (estaSaltando)
            {
                Debug.Log("te aplasto caballero");
                RestablecerDespuesDeSalto();
            }
            else
            {
                if (distanciaAlJugador <= 1.5f)
                {
                    Debug.Log("te pega caballero");
                }
            }
        }
        else if (estaSaltando)
        {
            RestablecerDespuesDeSalto();
        }
    }

    void RestablecerDespuesDeSalto()
    {
        estaSaltando = false;
        rb.isKinematic = true;
        agent.enabled = true;
    }
}