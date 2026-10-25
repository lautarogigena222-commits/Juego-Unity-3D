using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    [SerializeField] private Image icono;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI precioText;
    [SerializeField] private Button comprarButton;

    private ShopItemData item;
    private ShopUI shopUI;

    public void Setup(ShopItemData data, ShopUI ui)
    {
        item = data;
        shopUI = ui;

        if (data.icon != null)
            icono.sprite = data.icon;

        nombreText.text = data.itemName;
        precioText.text = data.price + " monedas";

        comprarButton.onClick.RemoveAllListeners();
        comprarButton.onClick.AddListener(() => shopUI.BuyItem(item));
    }
}