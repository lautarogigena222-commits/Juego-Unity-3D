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
        if (!other.CompareTag("Player")) return;

        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>();
        if (playerStats == null) return;

        if (healFood > 0f)
            playerStats.Curar(healFood);

        if (cantidadDrop > 0)
            playerStats.TakeCoins(cantidadDrop);
        Debug.Log("Recogido: " + name + " | monedas: " + cantidadDrop);

        Destroy(gameObject);
    }
}