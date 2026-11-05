using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class FinalBossTotem : MonoBehaviour, IInteractable, IPromptable
{
    [Header("Zona final")]
    [SerializeField] private Transform spawnPlayer;
    [SerializeField] private Transform spawnBoss;

    [Header("Boss final")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject bossHealthPanel;
    [SerializeField] private Image bossHealthBar;

    [Header("Cartel")]
    [SerializeField] private string textoCartel = "Invocar al jefe final [F]";

    /*[Header("Interacción")]
    [SerializeField] private float interactRange = 3f;

    public int Priority => 10;
    public float InteractRange => interactRange;*/

    private bool invocado = false;

    // Aparece el cartel solo si tiene las 4 reliquias (o ya canjeó la llave)
    public bool CanShowPrompt => !invocado && CumpleRequisito();
    public string PromptText => textoCartel;

    private bool CumpleRequisito()
    {
        return GameProgress.Instance != null && GameProgress.Instance.HasAllRelics();
    }

    public void Interact()
    {
        if (invocado || !CumpleRequisito()) return;

        if (spawnPlayer == null || spawnBoss == null || bossPrefab == null)
        {
            Debug.LogError("FinalBossTotem: falta asignar Spawn Player, Spawn Boss o Boss Prefab.", this);
            return;
        }

        invocado = true;

        TeletransportarJugador();

        GameObject bossObj = Instantiate(bossPrefab, spawnBoss.position, spawnBoss.rotation);

        BossMovement boss = bossObj.GetComponent<BossMovement>();
        if (boss != null)
            boss.SetUI(bossHealthBar, bossHealthPanel);

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);
    }

    private void TeletransportarJugador()
    {
        ThirdPersonController player = FindFirstObjectByType<ThirdPersonController>();
        if (player == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.transform.position = spawnPlayer.position;
        player.transform.rotation = spawnPlayer.rotation;
        cc.enabled = true;
    }
}