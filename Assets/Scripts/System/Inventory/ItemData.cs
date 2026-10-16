using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Game/Inventory/Item"
)]
public class ItemData : ScriptableObject
{
    [Header("Información")]
    public string itemName;

    [TextArea]
    public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Prefab")]
    public GameObject itemPrefab;

    [Header("Tipo")]
    public ItemType itemType;

    [Header("Stats")]
    public int damage;
}