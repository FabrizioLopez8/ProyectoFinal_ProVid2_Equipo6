// Habilidad de Planta: se cura una cantidad fija de HP pero pierde su proximo turno.
using UnityEngine;

public class Curar : IAccion
{
    public const int CURACION = 50; // HP fijo que recupera (ajustable)

    public void Ejecutar(Jugador atacante, Jugador objetivo)
    {
        // La curacion es sobre uno mismo, el "objetivo" no se usa en esta accion
        atacante.criatura.hp = Mathf.Min(atacante.criatura.hpMax, atacante.criatura.hp + CURACION);
        atacante.criatura.saltaTurno = true;
        atacante.DispararCurado(); // avisa al UIManager (particulas verdes)
    }
}
