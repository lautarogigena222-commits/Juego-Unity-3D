using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance { get; private set; }

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Transform spawnBase;
    [SerializeField] private GameObject playerHUD;

    private void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowDelayed(float delay)
    {
        StartCoroutine(ShowRoutine(delay));
    }

    private IEnumerator ShowRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Show();
    }

    public void Show()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (playerHUD != null)
            playerHUD.SetActive(false);


        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Botón "Volver a la base"
    public void ContinuarEnBase()
    {
        ThirdPersonController player = FindFirstObjectByType<ThirdPersonController>();

        if (player != null && spawnBase != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = spawnBase.position;
            cc.enabled = true;
        }

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (playerHUD != null)
            playerHUD.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Reiniciar()
    {
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();

        if (stats != null)
            stats.RestaurarVidaCompleta();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (playerHUD != null)
            playerHUD.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
