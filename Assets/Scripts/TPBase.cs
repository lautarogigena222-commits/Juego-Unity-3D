using UnityEngine;

public class TPBase : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enterDesert;
    //[SerializeField] GameObject enterForest;
    //[SerializeField] GameObject enterCave;
    //[SerializeField] GameObject enterSnnnow;
    //[SerializeField] GameObject enterFinalBoss;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enterDesert = GameObject.FindGameObjectWithTag("EnterDesert");
        //enterForest = GameObject.FindGameObjectWithTag("EnterForest");
        //enterCave = GameObject.FindGameObjectWithTag("EnterCave");
        //enterSnnnow = GameObject.FindGameObjectWithTag("EnterSnnnow");
        //enterFinalBoss = GameObject.FindGameObjectWithTag("EnterFinalBoss");
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = new Vector3(enterDesert.transform.position.x, enterDesert.transform.position.y + 2f, enterDesert.transform.position.z);
        }
    }
}
