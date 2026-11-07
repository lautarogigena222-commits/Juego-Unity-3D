using UnityEngine;
using UnityEngine.Audio;

public class ZonaMusical : MonoBehaviour
{
    public MusicManager musicManager;
    public AudioMixerSnapshot musicaDeEstaZona;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicManager.CambiarZona(musicaDeEstaZona);
        }
    }
}
