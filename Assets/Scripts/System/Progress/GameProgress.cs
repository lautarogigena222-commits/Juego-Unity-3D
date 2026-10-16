using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [Header("Reliquias")]
    [SerializeField] private bool relic1;
    [SerializeField] private bool relic2;
    [SerializeField] private bool relic3;
    [SerializeField] private bool relic4;

    [Header("Llave final")]
    [SerializeField] private bool finalBossKey;

    public bool HasRelic1 => relic1;
    public bool HasRelic2 => relic2;
    public bool HasRelic3 => relic3;
    public bool HasRelic4 => relic4;
    public bool HasFinalBossKey => finalBossKey;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GiveRelic(int relicNumber)
    {
        switch (relicNumber)
        {
            case 1:
                relic1 = true;
                break;

            case 2:
                relic2 = true;
                break;

            case 3:
                relic3 = true;
                break;

            case 4:
                relic4 = true;
                break;

            default:
                Debug.LogWarning("Número de reliquia inválido: " + relicNumber);
                return;
        }

        Debug.Log("Reliquia " + relicNumber + " obtenida.");
    }

    public bool HasAllRelics()
    {
        return relic1 && relic2 && relic3 && relic4;
    }

    public void ExchangeRelicsForKey()
    {
        if (!HasAllRelics())
            return;

        relic1 = false;
        relic2 = false;
        relic3 = false;
        relic4 = false;

        finalBossKey = true;

        Debug.Log("Las 4 reliquias fueron entregadas. Llave final obtenida.");
    }
}