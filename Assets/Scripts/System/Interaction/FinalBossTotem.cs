using UnityEngine;

public class FinalBossTotem : MonoBehaviour, IInteractable
{
    [SerializeField] private string finalBossScene = "FinalBoss";

    public void Interact()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("No existe GameProgress.");
            return;
        }

        if (!GameProgress.Instance.HasFinalBossKey)
        {
            Debug.Log("El tótem está sellado. Necesitas la llave final.");
            return;
        }

        Debug.Log("La llave es aceptada. Accediendo al jefe final...");

        LoadingManager.LoadScene(finalBossScene);
    }
}