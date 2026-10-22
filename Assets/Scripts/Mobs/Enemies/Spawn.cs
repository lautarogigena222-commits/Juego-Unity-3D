using UnityEngine;

public class Spawn: MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float timeBetweenSpawn = 3f;
    private float spawnTimeEnemy;
    [SerializeField]  private float spawnTimeAnimal;
    private bool playerInZone = false;

    
    [Header("Zona de detección")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private LayerMask playerLayer;

    
    [Header("Enemigos")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private int enemyNumber;
    [SerializeField] private int maxEnemies;
    [SerializeField] private bool spawnEnemies = false;

    
    [Header("Animales")]
    [SerializeField] private GameObject animal;
    [SerializeField] private int animalNumber;
    [SerializeField] private int maxAnimals;
    [SerializeField] private bool spawnAnimals = false;

    void Update()
    {
        DetectPlayer();

        if (!playerInZone) return;

        if (spawnEnemies && enemyNumber < maxEnemies)
            SpawnEnemy();

        if (spawnAnimals && animalNumber < maxAnimals)
            SpawnAnimal();
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            playerLayer
        );

        playerInZone = hits.Length > 0;
    }

    private void SpawnEnemy()
    {
        spawnTimeEnemy += Time.deltaTime;
        if (spawnTimeEnemy < timeBetweenSpawn) return;

        GameObject spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

        // Le pasa referencia al spawner para que avise al morir
        Enemy enemyScript = spawnedEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
            enemyScript.SetSpawner(this);

        spawnTimeEnemy = 0;
        enemyNumber++;
    }

    private void SpawnAnimal()
    {
        spawnTimeAnimal += Time.deltaTime;
        if (spawnTimeAnimal < timeBetweenSpawn) return;

        Instantiate(animal, transform.position, Quaternion.identity);
        spawnTimeAnimal = 0;
        animalNumber++;
    }

    public void OnEnemyDied()
    {
        enemyNumber--;
        if (enemyNumber < 0) enemyNumber = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}