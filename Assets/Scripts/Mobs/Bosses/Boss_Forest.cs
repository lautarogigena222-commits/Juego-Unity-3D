using UnityEngine;

public class Boss_Forest : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 500f;
    private float currentHealth;

    [Header("Portal")]
    [SerializeField] private GameObject portalNextZone;
    [Header("Music")]
    [SerializeField] private MusicManager musicManager;
    private bool musicaCombateActivada = false;

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
        if (musicaCombateActivada && musicManager != null)
        {
            musicManager.BossMurio();
            musicaCombateActivada = false;
        }

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
