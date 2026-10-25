using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItem")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public int price;
    public UpgradeData upgradeData;
}
