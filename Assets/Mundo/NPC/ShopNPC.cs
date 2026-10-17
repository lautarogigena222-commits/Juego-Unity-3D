using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("No existe GameProgress en la escena.");
            return;
        }

        // Todavía no tiene las 4 reliquias
        if (!GameProgress.Instance.HasAllRelics())
        {
            Debug.Log("Necesitas reunir las cuatro reliquias.");
            return;
        }

        // Tiene las 4 → las entrega y recibe la llave
        GameProgress.Instance.ExchangeRelicsForKey();

        Debug.Log("Has entregado las cuatro reliquias. Has recibido la llave final.");
    }
}