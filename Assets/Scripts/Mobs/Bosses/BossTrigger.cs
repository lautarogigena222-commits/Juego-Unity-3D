using System.Collections;
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

    private bool bossInvocado = false;
    private GameObject bossInstance;

    private void OnTriggerEnter(Collider other)
    {
        if (bossInvocado) return;

        if (!other.gameObject.CompareTag("Player")) return;

        if (enemigosDerrotados < enemigosRequeridos)
        {
            return;
        }

        bossInvocado = true;
        SetSpawners(false);
          //PausarSpawners();
        DestruirEnemigos();

        bossInstance = Instantiate(boss, transform.position + Vector3.back * 5f + Vector3.up * 2f, Quaternion.identity);

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);

        OcultarTrigger();
        StartCoroutine(EsperarMuerteBoss());
    }

    private IEnumerator EsperarMuerteBoss()
    {
        // Espera hasta que el boss sea destruido
        yield return new WaitUntil(() => bossInstance == null);

        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);

        SetSpawners(true);
    }

    public static void SumarEnemigoDerrotado()
    {
        enemigosDerrotados++;
    }


    private void SetSpawners(bool activo)
    {
        ActualizarSpawner(spawner1, activo);
        ActualizarSpawner(spawner2, activo);
        ActualizarSpawner(spawner3, activo);
        ActualizarSpawner(spawner4, activo);
    }

    private void ActualizarSpawner(Spawn spawner, bool activo)
    {
        if (spawner == null) return;

        spawner.enabled = activo;

        // Los enemigos se destruyeron sin avisar, así que se resetea el contador
        if (activo)
            spawner.ResetEnemyCount();
    }

    private void DestruirEnemigos()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
    }

    private void OcultarTrigger()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }
}