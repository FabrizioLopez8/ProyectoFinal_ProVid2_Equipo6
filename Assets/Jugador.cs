using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoJugador { Vivo, Eliminado }

[System.Serializable]
public class Jugador 
{
    public string nombre;
    public Bolsimon criatura;
    public EstadoJugador estado = EstadoJugador.Vivo;
}