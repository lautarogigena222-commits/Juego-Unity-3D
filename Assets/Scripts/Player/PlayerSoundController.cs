using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoAtacar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSaltar);
    }
    public void playAtacar() 
    {
        audioSource.PlayOneShot(sonidoAtacar);
        
    }
}

