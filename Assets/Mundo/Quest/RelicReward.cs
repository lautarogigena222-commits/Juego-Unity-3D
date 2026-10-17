using UnityEngine;

public class RelicReward : MonoBehaviour
{
    [SerializeField] private int relicNumber = 1;

    public void GiveRelic()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogWarning("No existe GameProgress.");
            return;
        }

        GameProgress.Instance.GiveRelic(relicNumber);

        Destroy(gameObject);
    }
}