using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask interactLayer;

    private PlayerControls controls;
    private IInteractable currentInteractable;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += OnInteract;
        controls.Player.Enable();
    }

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        currentInteractable = null;
        string texto = "Interactuar [F]";

        bool tiendaAbierta = ShopUI.Instance != null && ShopUI.Instance.IsOpen;

        if (!tiendaAbierta)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, interactLayer);
            float masCercano = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                IInteractable interactable = hit.GetComponentInParent<IInteractable>();
                if (interactable == null) continue;

                IPromptable promptable = interactable as IPromptable;
                if (promptable != null && !promptable.CanShowPrompt) continue;

                float distancia = Vector3.Distance(transform.position, hit.transform.position);
                if (distancia >= masCercano) continue;

                masCercano = distancia;
                currentInteractable = interactable;

                if (promptable != null)
                    texto = promptable.PromptText;
            }
        }

        if (InteractionPromptUI.Instance == null) return;

        if (currentInteractable != null)
            InteractionPromptUI.Instance.Show(texto);
        else
            InteractionPromptUI.Instance.Hide();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (currentInteractable != null)
            currentInteractable.Interact();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Player.Disable();
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Player.Interact.performed -= OnInteract;
            controls.Dispose();
            controls = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}