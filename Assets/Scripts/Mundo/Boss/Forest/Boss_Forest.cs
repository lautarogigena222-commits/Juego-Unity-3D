using UnityEngine;

public class Boss_Forest : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 500f;
    private float currentHealth;

    [Header("Portal")]
    [SerializeField] private GameObject portalNextZone;

    private void Start()
    {
        currentHealth = maxHealth;

        if (portalNextZone != null)
            portalNextZone.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Boss HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Boss muerto");

        if (portalNextZone != null)
            portalNextZone.SetActive(true);

        Destroy(gameObject);
    }
}
