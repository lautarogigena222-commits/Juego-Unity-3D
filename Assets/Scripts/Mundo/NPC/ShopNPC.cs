using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable, IPromptable
{
    public string PromptText => "Tienda [F]";
    public bool CanShowPrompt => true;

    public void Interact()
    {
        if (ShopUI.Instance != null)
            ShopUI.Instance.OpenShop();
    }
}