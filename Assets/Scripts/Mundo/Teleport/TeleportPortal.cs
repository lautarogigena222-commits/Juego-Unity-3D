using UnityEngine;
using StarterAssets;

public class TeleportPortal : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    [SerializeField] private Transform destination;
    [SerializeField] private Transform playerSpawnDirection;

    private bool teleporting = false;

    public void Interact()
    {
        Teleport();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ThirdPersonController>() != null)
            Teleport();
    }

    private void Teleport()
    {
        if (teleporting || destination == null) return;
        teleporting = true;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) { teleporting = false; return; }

        ThirdPersonController cc = player.GetComponent<ThirdPersonController>();

        cc.enabled = false;
        player.transform.position = destination.position;

        if (playerSpawnDirection != null)
            player.transform.rotation = playerSpawnDirection.rotation;

        cc.enabled = true;
        teleporting = false;
    }
}