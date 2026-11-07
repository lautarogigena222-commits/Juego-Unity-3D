using UnityEngine;

public class EnemigoSaltarin : MonoBehaviour
{
    public float fuerzaHaciaAdelante = 5f;
    public float fuerzaHaciaArriba = 4f; 
    public float tiempoEntreSaltos = 2f;

    private Transform player;
    private Rigidbody rb;
    private float tiempoParaProximoSalto;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject objetoPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objetoPlayer != null)
        {
            player = objetoPlayer.transform;
        }
    }

    void Update()
    {
        if (player == null) return; 

        if (Time.time >= tiempoParaProximoSalto)
        {
            SaltarHaciaJugador();

            tiempoParaProximoSalto = Time.time + tiempoEntreSaltos;
        }
    }

    void SaltarHaciaJugador()
    {
        Vector3 direccion = player.position - transform.position;

        direccion.y = 0;
        direccion = direccion.normalized; 

        Vector3 fuerzaDeSalto = (direccion * fuerzaHaciaAdelante) + (Vector3.up * fuerzaHaciaArriba);

        rb.AddForce(fuerzaDeSalto, ForceMode.Impulse);
    }

   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("te choco el enemigo");
        }
    }
}