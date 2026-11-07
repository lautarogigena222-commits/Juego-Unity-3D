
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [Header("Snapshots de exploración")]
    public AudioMixerSnapshot exploracionBosque;
    public AudioMixerSnapshot exploracionDesierto;
    public AudioMixerSnapshot exploracionCueva;
    public AudioMixerSnapshot exploracionNieve;
    public AudioMixerSnapshot silencio;
    [Header("Snapshot de combate")]
    public AudioMixerSnapshot combateBoss;
    [Header("Configuración")]
    public float tiempoTransicion = 2f;
    private AudioMixerSnapshot musicaActual;
    private int bossesActivos = 0;

    private void Start()
    {
        CambiarZona(exploracionBosque);
    }
    public void CambiarZona(AudioMixerSnapshot nuevaMusica)
    {
        musicaActual = nuevaMusica;

        if (bossesActivos == 0)
        {
            musicaActual.TransitionTo(tiempoTransicion);
        }
    }
    public void BossAparecio()
    {
        bossesActivos++;

        if (bossesActivos == 1)
        {
            combateBoss.TransitionTo(tiempoTransicion);
        }
    }
    public void BossMurio()
    {
        bossesActivos = Mathf.Max(0, bossesActivos - 1);

        if (bossesActivos == 0 && musicaActual != null)
        {
            musicaActual.TransitionTo(tiempoTransicion);
        }
    }
}