using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoJugador {Vivo, Eliminado}

[System.Serializable]
public class jugador {
    public string nombre; // "jugador 1 ", "jugador 2" "jugador 3"
    public Bolsimon criatura ;
    public EstadoJugador estado = EstadoJugador.Vivo;
}
