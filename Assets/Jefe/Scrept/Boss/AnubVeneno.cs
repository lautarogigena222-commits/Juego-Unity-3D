using UnityEngine;

public class AnubVeneno : MonoBehaviour
{
 
    public int bulletBoss;  

    void Start()
    {
        Destroy(gameObject, 3);
    }


    void Update()
    {
        transform.Translate(Vector3.forward * 6 * Time.deltaTime);
        transform.localScale += new Vector3(3, 3, 3) * Time.deltaTime;


    }
}
