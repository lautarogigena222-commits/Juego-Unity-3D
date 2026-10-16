using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Cofre/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public enum UpgradeType
    {
        IncreaseHealth,
        IncreaseDamage,
        IncreaseSpeed,
        AutoShoot,
        IncreaseProjectileDamage,
        IncreaseFireRate,
        InstantHeal,
    }

    //info panel
    public string upgradeName;

    [TextArea] 
    public string description;

    public Sprite icon;

   
    
    [Header("Efecto")]
    public UpgradeType upgradeType;
    public float value;
}
