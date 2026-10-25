using StarterAssets;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource audioSource; // para salto y ataque
    [SerializeField] private AudioSource audioSourcePasos; // solo para pasos

    [Header("Sonidos")]
    public AudioClip sonidoSaltar;
    public AudioClip sonidoAtacar;

    [Header("Pasos")]
    [SerializeField] private AudioClip[] sonidosPasos;
    [SerializeField] private float volumenPasos = 0.2f;

    [Header("Volúmenes")]
    [SerializeField] private float volumenSaltar = 1f;
    [SerializeField] private float volumenAtacar = 0.5f;

    private ThirdPersonController thirdPersonController;

    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        if (thirdPersonController != null)
            sonidosPasos = thirdPersonController.FootstepAudioClips;
    }

    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSaltar, volumenSaltar);
    }

    public void playAtacar()
    {
        audioSource.PlayOneShot(sonidoAtacar, volumenAtacar);
    }

    void FootL() => PlayPaso();
    void FootR() => PlayPaso();
    void Hit() { }
    void Land() { }

    private void PlayPaso()
    {
        if (sonidosPasos == null || sonidosPasos.Length == 0) return;
        AudioClip clip = sonidosPasos[Random.Range(0, sonidosPasos.Length)];
        audioSourcePasos.PlayOneShot(clip, volumenPasos);
    }
}

