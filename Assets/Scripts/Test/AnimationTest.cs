using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private float nextAttackTime;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
                player = target.transform;
        }
    }

    void Update()
    {
        if (player == null || animator == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        // No detecta al jugador
        if (distance > detectionRange)
        {
            animator.SetBool("IsWalking", false);
            return;
        }

        // Mirar al jugador
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Caminar hacia el jugador
        if (distance > attackRange)
        {
            animator.SetBool("IsWalking", true);

            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(
                    player.position.x,
                    transform.position.y,
                    player.position.z
                ),
                speed * Time.deltaTime
            );
        }
        else
        {
            animator.SetBool("IsWalking", false);

            // Atacar
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void HitPlayer()
    {
        Debug.Log("El boss ejecutó el golpe");
    }
}
