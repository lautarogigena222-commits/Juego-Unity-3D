using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class Animal : MonoBehaviour
    {
        [Header("Movimiento")]
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float tiempoMovimiento = 3f;
        [SerializeField] private float tiempoDescansoMin = 1f;
        [SerializeField] private float tiempoDescansoMax = 3f;

        [Header("Animator")]
        [SerializeField] private string m_VerticalID = "Vert";
        [SerializeField] private string m_StateID = "State";

        [Header ("Drops")]
        [SerializeField] GameObject comida;
        [SerializeField] int drop;
        [SerializeField] float animalLife = 10f; 

        private CharacterController controller;
        private Animator animator;

        private float cronometro;
        private float tiempoDescanso;
        private bool descansando;
        private Vector3 direccion;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            ElegirNuevaDireccion();
        }

        private void Update()
        {
            Comportamiento();
            AplicarGravedad();
            DropComida();
        }

       /* private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animalLife -= other.GetComponent<PlayerStats>().PlayerDamage;
            }
            if (animalLife <= 0)
            {
                Destroy(gameObject);
                DropComida();
            }
        }*/

        public void TakeDamage(float amount)
        {
            animalLife -= amount;
            if(animalLife <= 0)
            {
                Destroy(gameObject);
                DropComida();
            }
        }

        public void DropComida()
        {
            if (animalLife <= 0)
            {
                drop = UnityEngine.Random.Range(0, 2);
                switch (drop)
                {
                    case 0:
                        break;
                    case 1:
                        Instantiate(comida, transform.position, transform.rotation);
                        break;
                    case 2:
                        break;
                }

            }
        }

        private void Comportamiento()
        {
            cronometro += Time.deltaTime;

            if (descansando)
            {
                animator.SetFloat(m_VerticalID, 0f);
                animator.SetFloat(m_StateID, 0f);

                if (cronometro >= tiempoDescanso)
                {
                    descansando = false;
                    cronometro = 0f;
                    ElegirNuevaDireccion();
                }
            }
            else
            {
                controller.Move(direccion * walkSpeed * Time.deltaTime);

                if (direccion != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(direccion),
                        10f * Time.deltaTime
                    );
                }

                animator.SetFloat(m_VerticalID, 1f);
                animator.SetFloat(m_StateID, 0f);

                if (cronometro >= tiempoMovimiento)
                {
                    descansando = true;
                    cronometro = 0f;
                    tiempoDescanso = Random.Range(tiempoDescansoMin, tiempoDescansoMax);
                }
            }
        }

        private void AplicarGravedad()
        {
            if (!controller.isGrounded)
                controller.Move(Physics.gravity * Time.deltaTime);
        }

        private void ElegirNuevaDireccion()
        {
            float angulo = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            direccion = new Vector3(Mathf.Sin(angulo), 0f, Mathf.Cos(angulo)).normalized;
        }
    }
}
