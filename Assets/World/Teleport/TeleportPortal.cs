using UnityEngine;

public class TeleportPortal : MonoBehaviour, IInteractable
{
    public enum TeleportMode
    {
        OnTouch,
        OnInteract
    }

    [Header("Configuración")]
    [SerializeField] private TeleportMode teleportMode = TeleportMode.OnTouch;
    [SerializeField] private string targetScene = "FinalBoss";

    private bool teleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (teleportMode != TeleportMode.OnTouch)
            return;

        if (other.GetComponentInParent<PlayerController>() != null)
        {
            Debug.Log("Jugador entró al portal.");
            Teleport();
        }
    }

    public void Interact()
    {
        if (teleportMode != TeleportMode.OnInteract)
            return;

        Teleport();
    }

    private void Teleport()
    {
        if (teleporting)
            return;

        teleporting = true;

        Debug.Log("Teletransportando a: " + targetScene);

        LoadingManager.LoadScene(targetScene);
    }
}