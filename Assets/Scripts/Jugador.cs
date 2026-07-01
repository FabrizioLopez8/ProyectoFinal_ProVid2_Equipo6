using System;

public enum EstadoJugador { Vivo, Eliminado }

[Serializable]
public class Jugador
{
    public string nombre;
    public Bolsimon criatura;
    public EstadoJugador estado;
}