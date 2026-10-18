using UnityEngine;
using StarterAssets;

public class TeleportPortal : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    [SerializeField] private Transform destination;
    [SerializeField] private Transform playerSpawnDirection;

    private bool teleporting = false;


    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonController tpc = other.GetComponentInParent<ThirdPersonController>();

        if (tpc != null)
            Teleport(tpc.gameObject); // usamos directamente el GO del ThirdPersonController
    }

    public void Interact()
    {
        ThirdPersonController tpc = FindFirstObjectByType<ThirdPersonController>();
        if (tpc != null)
            Teleport(tpc.gameObject);
    }

    private void Teleport(GameObject player)
    {
        if (teleporting || destination == null) return;
        teleporting = true;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc == null) { teleporting = false; return; }

        cc.enabled = false;
        player.transform.position = destination.position;

        if (playerSpawnDirection != null)
            player.transform.rotation = playerSpawnDirection.rotation;

        cc.enabled = true;
        teleporting = false;
    }
}