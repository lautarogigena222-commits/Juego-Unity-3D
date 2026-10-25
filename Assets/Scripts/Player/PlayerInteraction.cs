using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask interactLayer;
    //private bool playerInZone = false;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, interactLayer);

        

        if (hits.Length > 0)
        {
         

            IInteractable interactable = hits[0].GetComponentInParent<IInteractable>();

        


            if (interactable != null)
            {
                currentInteractable = interactable;
                ShopUI.Instance?.ShowPrompt(true);
                return;
            }
        }

        currentInteractable = null;
        ShopUI.Instance?.ShowPrompt(false);
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

    /*public interface IInteractable
    {
        void Interact();
    }*/
}