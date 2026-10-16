using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        if (playerInventory == null || coinText == null)
            return;

        coinText.text = playerInventory.Coins.ToString();
    }
}