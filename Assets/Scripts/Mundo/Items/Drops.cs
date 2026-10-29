using UnityEngine;

public class Drops : MonoBehaviour
{
    [Header("Monedas")]
    [SerializeField] GameObject coins;

    [Header("Comida y Curación")]
    [SerializeField] GameObject food;
    [SerializeField] private float healFood = 3f;
    public float HealFood => healFood;

    [SerializeField] int cantidadDrop;
    private Animation animacion;

    void Start()
    {
        animacion = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Curar(healFood);
            }

            Destroy(gameObject);
        }
    }
}