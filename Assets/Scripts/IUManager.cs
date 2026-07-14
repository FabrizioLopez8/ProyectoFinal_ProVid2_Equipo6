using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Sprites de los Bolsimon")]
    public SpriteRenderer spriteJ1;
    public SpriteRenderer spriteJ2;
    public SpriteRenderer spriteJ3;

    [Header("Sistemas de partículas")]
    public ParticleSystem particulasJ1;
    public ParticleSystem particulasJ2;
    public ParticleSystem particulasJ3;

    [Header("UI")]
    public TextMeshProUGUI textoDialogo;

    void Start()
    {
        // Se suscribe a los eventos de cada jugador
        SuscribirEventos(GameManager.Instance.jugadores[0], spriteJ1, particulasJ1);
        SuscribirEventos(GameManager.Instance.jugadores[1], spriteJ2, particulasJ2);
        SuscribirEventos(GameManager.Instance.jugadores[2], spriteJ3, particulasJ3);
    }

    // Suscribe los efectos visuales a los eventos de un jugador
    private void SuscribirEventos(Jugador jugador, SpriteRenderer sprite, ParticleSystem particulas)
    {
        jugador.OnQuemado += () => StartCoroutine(EfectoQuemado(sprite));
        jugador.OnCurado += () => particulas.Play(); // lanza particulas verdes
        jugador.OnDanioRecibido += () => StartCoroutine(EfectoTitileo(sprite));
        jugador.OnEliminado += () => sprite.color = Color.gray; // se apaga al morir
    }

    // Oscurece el sprite del jugador quemado
    private IEnumerator EfectoQuemado(SpriteRenderer sprite)
    {
        sprite.color = new Color(0.3f, 0.1f, 0.1f); // color oscuro rojizo
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white; // vuelve al color normal
    }

    // Hace titilar el sprite del jugador afectado por agua
    private IEnumerator EfectoTitileo(SpriteRenderer sprite)
    {
        for (int i = 0; i < 3; i++) // titila 3 veces
        {
            sprite.color = new Color(0.3f, 0.6f, 1f); // color azulado
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnDestroy()
    {
        foreach (Jugador j in GameManager.Instance.jugadores)
        {
            j.LimpiarEventos();
        }
    }
}
