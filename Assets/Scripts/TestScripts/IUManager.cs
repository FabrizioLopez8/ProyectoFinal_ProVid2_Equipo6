using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get ; private set ;}
    [Header("Sprites de los Bolsimon")]
    public SpriteRenderer spriteJ1;
    public SpriteRenderer spriteJ2;
    public SpriteRenderer spriteJ3;

    [Header("Sistemas de part�culas")]
    public ParticleSystem particulasJ1;
    public ParticleSystem particulasJ2;
    public ParticleSystem particulasJ3;

    //[Header("UI")]
    //public TextMeshProUGUI textoDialogo;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        
    }

    public void Preparations()
    {
        // Se suscribe a los eventos de cada jugador
        SuscribirEventos(TestGM.Instance.playersBolsimon[0], spriteJ1, particulasJ1);
        SuscribirEventos(TestGM.Instance.playersBolsimon[1], spriteJ2, particulasJ2);
        SuscribirEventos(TestGM.Instance.playersBolsimon[2], spriteJ3, particulasJ3);
    }

    // Suscribe los efectos visuales a los eventos de un jugador
    private void SuscribirEventos(BolsimonController jugador, SpriteRenderer sprite, ParticleSystem particulas)
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
        sprite.color = Bolsimon.Colors(sprite.gameObject.GetComponent<BolsimonController>().type); // vuelve al color normal
    }

    // Hace titilar el sprite del jugador afectado por agua
    private IEnumerator EfectoTitileo(SpriteRenderer sprite)
    {
        for (int i = 0; i < 3; i++) // titila 3 veces
        {
            sprite.color = new Color(0.3f, 0.6f, 1f); // color azulado
            yield return new WaitForSeconds(0.1f);
            sprite.color = Bolsimon.Colors(sprite.gameObject.GetComponent<BolsimonController>().type);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // void OnDestroy()
    // {
    //     foreach (BolsimonController j in TestGM.Instance.playersBolsimon)
    //     {
    //         j.LimpiarEventos();
    //     }
    // }
}
