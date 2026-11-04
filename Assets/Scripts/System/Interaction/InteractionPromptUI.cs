using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        Instance = this;

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    public void Show(string text)
    {
        if (promptPanel == null) return;

        if (promptText != null && promptText.text != text)
            promptText.text = text;

        if (!promptPanel.activeSelf)
            promptPanel.SetActive(true);
    }

    public void Hide()
    {
        if (promptPanel != null && promptPanel.activeSelf)
            promptPanel.SetActive(false);
    }
}
