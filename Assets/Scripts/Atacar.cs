// Accion de ataque: calcula el daño con multiplicador de tipo y lo aplica al objetivo
using UnityEngine;

public class Atacar : IAccion
{
    public void Ejecutar(Jugador atacante, Jugador objetivo)
    {
        // Si el objetivo tiene escudo activo, lo consume y no recibe daño
        if (objetivo.criatura.escudo)
        {
            objetivo.criatura.escudo = false; // el escudo se consume al recibir el ataque
            return;
        }

        // Obtiene el multiplicador segun los tipos enfrentados
        float multi = BattleManager.GetMultiplicador(atacante.criatura.tipo, objetivo.criatura.tipo);

        // Calcula el daño base redondeado al entero mas cercano
        int danio = Mathf.RoundToInt(atacante.criatura.ataque * multi);

        // Si el atacante fue debilitado por la habilidad de Agua, su ataque hace menos daño
        if (atacante.criatura.danioReducido)
        {
            danio = Mathf.RoundToInt(danio * ReducirAtaque.MULTIPLICADOR_REDUCCION);
            atacante.criatura.danioReducido = false; // se consume en este ataque
        }

        // Aplica el daño sin que el HP baje de 0
        objetivo.criatura.hp = Mathf.Max(0, objetivo.criatura.hp - danio);

        // Avisa al UIManager que el objetivo recibio daño (efecto de titileo)
        objetivo.DispararDanio();

        // Si el HP llega a 0, el jugador queda eliminado
        if (objetivo.criatura.hp <= 0)
        {
            objetivo.estado = EstadoJugador.Eliminado;
            objetivo.DispararEliminado(); // avisa al UIManager para apagar el sprite
        }
    }
}