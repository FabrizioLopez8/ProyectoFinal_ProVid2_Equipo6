
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Jugador[] jugadores = new Jugador[3];
    public int turnoActual = 0; // Indice del 0 al 2 (3 posiciones)

    void Awake(){
        if (Instance == null) 
        {
            Instance = this; DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AvanzarTurno(){
        do {
            turnoActual= (turnoActual + 1) % 3;
        }
        while (jugadores[turnoActual].estado == EstadoJugador.Eliminado);
    }

    public Jugador[] GetVivos() {
        return System.Array.FindAll(jugadores, j => j.estado == EstadoJugador.Vivo);
    }
}
