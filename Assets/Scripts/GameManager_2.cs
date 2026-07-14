using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Jugador[] jugadores = new Jugador[3];
    public int turnoActual = 0; // Indice del 0 al 2 (3 posiciones)

    public event System.Action<Jugador> OnJuegoTerminado; // avisa quien gano

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AvanzarTurno()
    {
        do
        {
            turnoActual = (turnoActual + 1) % 3;

            if (jugadores[turnoActual].estado == EstadoJugador.Eliminado)
                continue; // eliminado, no juega, sigue buscando

            if (jugadores[turnoActual].criatura.saltaTurno)
            {
                jugadores[turnoActual].criatura.saltaTurno = false; // consume el turno salteado (habilidad de Planta)
                continue; // no le toca jugar esta vuelta, sigue buscando
            }

            break; // encontramos un jugador que si puede jugar
        }
        while (true);

        // Al empezar el turno, si el jugador esta quemado, recibe daño y se cura del estado
        AplicarEstadoInicioTurno(jugadores[turnoActual]);
    }

    // Aplica el daño de quemadura al inicio del turno y luego cura el estado
    private void AplicarEstadoInicioTurno(Jugador jugador)
    {
        if (!jugador.criatura.quemado)
            return;

        int danio = Quemar.DANIO_QUEMADURA;
        if (jugador.criatura.tipo == TipoElemento.Planta)
            danio *= 2; // la quemadura hace el doble de daño a Planta

        jugador.criatura.hp = Mathf.Max(0, jugador.criatura.hp - danio);
        jugador.DispararDanio(); // efecto de titileo por el daño

        if (jugador.criatura.hp <= 0)
        {
            jugador.estado = EstadoJugador.Eliminado;
            jugador.DispararEliminado();
        }

        jugador.criatura.quemado = false; // se cura del estado tras el daño
        jugador.DispararCurado();

        // La quemadura tambien puede ser el golpe final: revisamos si el juego termino aca
        VerificarFinDeJuego();
    }

    // Si queda un solo jugador vivo (o ninguno), avisa que el juego termino
    public void VerificarFinDeJuego()
    {
        Jugador[] vivos = GetVivos();
        if (vivos.Length <= 1)
        {
            OnJuegoTerminado?.Invoke(vivos.Length == 1 ? vivos[0] : null);
        }
    }

    public Jugador[] GetVivos()
    {
        return System.Array.FindAll(jugadores, j => j.estado == EstadoJugador.Vivo);
    }
}