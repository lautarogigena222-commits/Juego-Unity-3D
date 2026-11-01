using UnityEngine;

public class DuendeController : MonoBehaviour
{
    public Transform jugador;

    public float velocidad = 3f;

    void Update()
    {
        if (jugador != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("te esta atacando el duende");
        }
    }
}