using UnityEngine;

public class ComportamientoBomba : MonoBehaviour
{
    public GameObject explosionVFX;
    public float fuerzaLanzamiento = 12f;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (player != null && rb != null)
        {
            Vector3 direccion = (player.position - transform.position).normalized;

            direccion.y += 0.5f;

            rb.AddForce(direccion.normalized * fuerzaLanzamiento, ForceMode.Impulse);
        }

        Invoke("Explotar", 2f);
    }

    void Explotar()
    {
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        if (player != null)
        {
            float distancia = Vector3.Distance(transform.position, player.position);

            if (distancia < 1.5f)
            {
                Debug.Log("Explosion alcanzada");
            }
        }

        Destroy(gameObject);
    }
}