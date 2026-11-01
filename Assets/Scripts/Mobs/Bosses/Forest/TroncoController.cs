using UnityEngine;

public class ArbolGigante : MonoBehaviour
{
    public Transform player;
    public float velocidad = 3f;

    private bool impacto = false;
    private bool estaAplastando = false;

    void Update()
    {
        if (!estaAplastando && player != null)
        {
            
            Vector3 objetivo = new Vector3(player.position.x, transform.position.y, player.position.z);

            transform.LookAt(objetivo);

            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

            if (Detectar())
            {
                Aplastar();
            }
        }
    }

    bool Detectar()
    {
        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia <= 1f)
        {
            return true;
        }

        return false;
    }

    void Aplastar()
    {
        estaAplastando = true;

        transform.Rotate(90f, 0f, 0f);

        Invoke("Levantarse", 2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (estaAplastando && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("te aplastaron");
            impacto = true;
        }
    }

    void Levantarse()
    {
        if (impacto)
        {
            transform.Rotate(-90f, 0f, 0f);
            impacto = false;
            estaAplastando = false;

            if (Detectar())
            {
                Invoke("Aplastar", 1f);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            estaAplastando = false;
        }
    }
}