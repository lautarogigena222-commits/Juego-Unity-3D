using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [Header("Panel")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopPrompt;
    [SerializeField] private TextMeshProUGUI monedasText;

    [Header("Items")]
    [SerializeField] private Transform gridItems;
    [SerializeField] private GameObject shopItemSlotPrefab;
    [SerializeField] private List<ShopItemData> items;

    [Header("Referencias")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerStats playerStats;

    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
        shopPanel.SetActive(false);
        shopPrompt.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        shopPrompt.SetActive(show);
    }

    public void OpenShop()
    {
        if (isOpen) return;
        isOpen = true;

        shopPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        RefreshShop();
    }

    public void CloseShop()
    {
        isOpen = false;
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RefreshShop()
    {
        // Limpiar slots anteriores
        foreach (Transform child in gridItems)
            Destroy(child.gameObject);

        monedasText.text = "Monedas: " + playerInventory.Coins;

        foreach (ShopItemData item in items)
        {
            GameObject slot = Instantiate(shopItemSlotPrefab, gridItems);
            ShopItemSlot slotScript = slot.GetComponent<ShopItemSlot>();
            slotScript.Setup(item, this);
        }
    }

    public void BuyItem(ShopItemData item)
    {
        if (playerInventory.Coins < item.price)
        {
            Debug.Log("No tenés suficientes monedas.");
            return;
        }

        playerInventory.SpendCoins(item.price);
        playerStats.ApplyUpgrade(item.upgradeData);
        monedasText.text = "Monedas: " + playerInventory.Coins;

        Debug.Log("Compraste: " + item.itemName);
    }

    public bool IsOpen => isOpen;
}
