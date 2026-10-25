using UnityEngine;

namespace StarterAssets
{
    public class PlayerSoundController : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip sonidoSaltar;
        public AudioClip sonidoAtacar;

        public void playSaltar()
        {
            if (audioSource != null && sonidoSaltar != null)
            {
                audioSource.PlayOneShot(sonidoSaltar);
            }
        }

        public void playAtacar()
        {
            if (audioSource != null && sonidoAtacar != null)
            {
                audioSource.PlayOneShot(sonidoAtacar);
            }
        }
    }
}