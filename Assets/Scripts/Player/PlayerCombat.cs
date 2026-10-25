using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{
    public PlayerSoundController playerSoundController;
    private Animator animator;
    private PlayerControls controls;

    // Indica si el jugador está realizando un ataque
    public bool IsAttacking { get; private set; }

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 0.4f;
    private float lastAttackTime;

    private void Start()
    {
        animator = GetComponent<Animator>();

        controls = new PlayerControls();

        controls.Player.Attack.performed += OnAttack;

        controls.Player.Enable();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        // Si el inventario está abierto y el cursor está libre,
        // no permitime atacar con el clic izquierdo.
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Attack();
        
    }

    private void Attack()
    {
    
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        lastAttackTime = Time.time;
        animator.Play("1Hand_Up_Attack_A_3", 1, 0f);
        playerSoundController.playAtacar();



    }

    private System.Collections.IEnumerator ResetAndTriggerAttack()
    {
        animator.ResetTrigger("Attack");
        yield return null; // Espera un frame para que el Animator limpie el estado anterior
        animator.SetTrigger("Attack");
    }

    private void EndAttack()
    {
        IsAttacking = false;
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Disable();
        }
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Player.Attack.performed -= OnAttack;

            controls.Dispose();
            controls = null;
        }

        CancelInvoke();
    }

    public void EnableSwordHitbox()
    {
        SwordHitbox hitbox = GetComponentInChildren<SwordHitbox>();
        if (hitbox != null)
        {
            hitbox.EnableHitbox();
        }
    }

    public void DisableSwordHitbox()
    {
        SwordHitbox hitbox = GetComponentInChildren<SwordHitbox>();
        if (hitbox != null)
        {
            hitbox.DisableHitbox();
        }
    }
}