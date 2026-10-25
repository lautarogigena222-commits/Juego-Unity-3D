using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (ShopUI.Instance != null)
            ShopUI.Instance.OpenShop();
    }
}