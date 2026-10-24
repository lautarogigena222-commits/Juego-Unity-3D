using UnityEngine;

public class Spawn: MonoBehaviour
{
    [Header("Enemy prefab")]
    /*[SerializeField] private GameObject enemyBosque;
    [SerializeField] private GameObject enemyNieve;
    [SerializeField] private GameObject enemyDesierto;
    [SerializeField] private GameObject enemyPlaya;
    [SerializeField] private GameObject enemyCueva;*/

    private GameObject enemySelected;

    [Header("Spawn")]
    [SerializeField] private float timeBetweenSpawn = 3f;
    private float spawnTimeEnemy;
    [SerializeField]  private float spawnTimeAnimal;
    private bool playerInZone = false;

    
    [Header("Zona de detección")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private LayerMask playerLayer;

    
    [Header("Enemigos")]
    [SerializeField] private int enemyNumber;
    [SerializeField] private int maxEnemies;

    [SerializeField] private GameObject enemy;
    [SerializeField] private bool spawnEnemies = false;

    
    [Header("Animales")]
    [SerializeField] private GameObject animal;
    [SerializeField] private int animalNumber;
    [SerializeField] private int maxAnimals;
    [SerializeField] private bool spawnAnimals = false;

    private void Start()
    {
        // el spawner larga el enemigo mas cercano al player
        //AssignEnemy();
    }

    void Update()
    {
        DetectPlayer();

        if (!playerInZone) return;

        if (spawnEnemies && enemyNumber < maxEnemies)
            SpawnEnemy();

        if (spawnAnimals && animalNumber < maxAnimals)
            SpawnAnimal();
    }

    /*private void AssignEnemy()
    {
        Transform current = transform;
        bool assigned = false;

        while (current != null)
        {
            string nameLower = current.name.ToLower();

            if (nameLower.Contains("bosque") || nameLower.Contains("forest"))
            {
                enemySelected = enemy;
                assigned = true;
                break;
            }
            /*else if (nameLower.Contains("nieve") || nameLower.Contains("snow"))
            {
                enemySelected = enemyNieve;
                assigned = true;
                break;
            }
            else if (nameLower.Contains("desierto") || nameLower.Contains("desert"))
            {
                enemySelected = enemyDesierto;
                assigned = true;
                break;
            }
            else if (nameLower.Contains("playa") || nameLower.Contains("beach"))
            {
                enemySelected = enemyPlaya;
                assigned = true;
                break;
            }
            else if (nameLower.Contains("cueva") || nameLower.Contains("cave"))
            {
                enemySelected = enemyCueva;
                assigned = true;
                break;
            }

            current = current.parent;
        }

        if (!assigned)
        {
            enemySelected = enemyBosque;
        }
    }*/

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        playerInZone = hits.Length > 0;
    }

    private void SpawnEnemy()
    {
        /*if (enemySelected == null)
        {
            Debug.LogWarning("El spawner " + gameObject.name + " no tiene asignado el prefab del enemigo para este bioma.", gameObject);
            return;
        }*/

        spawnTimeEnemy += Time.deltaTime;
        if (spawnTimeEnemy < timeBetweenSpawn) return;



        GameObject spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);


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