using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossHealthPanel;

    [Header("Spawners")]
    [SerializeField] private Spawn spawner1;
    [SerializeField] private Spawn spawner2;
    [SerializeField] private Spawn spawner3;
    [SerializeField] private Spawn spawner4;

    [Header("Condición de Invocación")]
    [SerializeField] private int enemigosRequeridos = 10;
    private static int enemigosDerrotados = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (enemigosDerrotados < enemigosRequeridos)
        {
            return;
        }

        PausarSpawners();
        DestruirEnemigos();

        Instantiate(boss, transform.position + Vector3.back * 5f + Vector3.up * 2f, Quaternion.identity);

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);

        Destroy(gameObject);
    }

    public static void SumarEnemigoDerrotado()
    {
        enemigosDerrotados++;
    }

    private void PausarSpawners()
    {
        if (spawner1 != null) spawner1.enabled = false;
        if (spawner2 != null) spawner2.enabled = false;
        if (spawner3 != null) spawner3.enabled = false;
        if (spawner4 != null) spawner4.enabled = false;
    }

    private void DestruirEnemigos()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
    }
}