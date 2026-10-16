using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Inventario")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerEquipment playerEquipment;

    [Header("Slots")]
    [SerializeField] private InventorySlot weaponSlot;

    [Header("Información")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private GameObject equipButton;

    [Header("Monedas")]
    [SerializeField] private TMP_Text coinText;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Inventory.performed += OnInventoryPressed;
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        controls.Player.Enable();

        UpdateInventoryUI();
    }

    private void OnInventoryPressed(
        UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        bool opening = !inventoryPanel.activeSelf;

        inventoryPanel.SetActive(opening);

        if (opening)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            UpdateInventoryUI();
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdateInventoryUI()
    {
        if (playerInventory == null)
            return;

        // =========================
        // ARMA
        // =========================

        ItemData weapon = playerInventory.Weapon;

        if (weaponSlot != null)
        {
            weaponSlot.SetItem(weapon);
        }

        // =========================
        // INFORMACIÓN
        // =========================

        if (weapon != null)
        {
            if (weapon.icon != null)
            {
                itemIcon.sprite = weapon.icon;
                itemIcon.color = Color.white;
            }
            else
            {
                itemIcon.sprite = null;
                itemIcon.color = Color.clear;
            }

            itemName.text = weapon.itemName;
            itemDescription.text = weapon.description;

            equipButton.SetActive(true);
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;

            itemName.text = "";
            itemDescription.text = "";

            equipButton.SetActive(false);
        }

        // =========================
        // MONEDAS
        // =========================

        if (coinText != null)
        {
            coinText.text = playerInventory.Coins.ToString();
        }
    }

    public void EquipCurrentWeapon()
    {
        if (playerInventory == null)
            return;

        if (playerEquipment == null)
            return;

        if (playerInventory.Weapon == null)
            return;

        playerEquipment.EquipWeapon(
            playerInventory.Weapon
        );
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Disable();
        }
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Player.Inventory.performed -= OnInventoryPressed;

            controls.Dispose();
            controls = null;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}