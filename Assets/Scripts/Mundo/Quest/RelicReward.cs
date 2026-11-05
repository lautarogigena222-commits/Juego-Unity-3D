using UnityEngine;

public class RelicReward : MonoBehaviour
{
    [SerializeField] private int relicNumber = 1;

    // Permite que el boss le indique qué reliquia es al soltarla
    public void SetRelicNumber(int number)
    {
        relicNumber = number;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerStats>() == null) return;

        GiveRelic();
    }

    public void GiveRelic()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("No existe GameProgress en la escena.");
            return;
        }

        GameProgress.Instance.GiveRelic(relicNumber);
        Destroy(gameObject);
    }
}