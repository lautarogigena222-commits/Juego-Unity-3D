using ithappy.Animals_FREE;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private PlayerStats playerStats;
    private bool isAttacking = false;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    public void EnableHitbox()
    {
        isAttacking = true;
    }

    public void DisableHitbox()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(playerStats.PlayerDamage);
        }
        if (other.CompareTag("Animal"))
        {
            Animal animal = other.GetComponentInParent<Animal>();
            if (animal != null)
                animal.TakeDamage(playerStats.PlayerDamage);
        }
        if (other.CompareTag("Boss"))
        {
            BossMovement boss = other.GetComponentInParent<BossMovement>();
            if (boss != null)
                boss.TakeDamage(playerStats.PlayerDamage);
        }
    }
}
