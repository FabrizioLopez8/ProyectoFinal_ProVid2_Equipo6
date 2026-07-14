using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI textoDialogo; // referencia al cuadro de dialogo en la UI

    // Calcula el multiplicador de daño segun el tipo del atacante y el defensor
    public static float GetMultiplicador(TipoElemento ataque, TipoElemento defensa)
    {
        // Ventajas (daño doble)
        if (ataque == TipoElemento.Fuego && defensa == TipoElemento.Planta) return 2f;
        if (ataque == TipoElemento.Agua && defensa == TipoElemento.Fuego) return 2f;
        if (ataque == TipoElemento.Planta && defensa == TipoElemento.Agua) return 2f;

        // Desventajas (daño reducido a la mitad)
        if (ataque == TipoElemento.Fuego && defensa == TipoElemento.Agua) return 0.5f;
        if (ataque == TipoElemento.Agua && defensa == TipoElemento.Planta) return 0.5f;
        if (ataque == TipoElemento.Planta && defensa == TipoElemento.Fuego) return 0.5f;

        return 1f; // neutro: mismo tipo o combinacion sin ventaja
    }

    // Devuelve la habilidad especial que le corresponde a cada tipo de Bolsimon.
    public static IAccion GetHabilidad(TipoElemento tipo)
    {
        switch (tipo)
        {
            case TipoElemento.Fuego: return new Quemar();
            case TipoElemento.Agua: return new ReducirAtaque();
            case TipoElemento.Planta: return new Curar();
            default: return null;
        }
    }

    // Recibe la accion elegida por el jugador activo y la ejecuta
    public void EjecutarAccion(Jugador atacante, Jugador objetivo, IAccion accion)
    {
        accion.Ejecutar(atacante, objetivo);

        // Muestra el mensaje correspondiente segun el resultado de la accion
        if (objetivo.estado == EstadoJugador.Eliminado)
            MostrarDialogo($"{objetivo.nombre} fue eliminado!");
        else if (!objetivo.criatura.escudo && objetivo.criatura.hp < objetivo.criatura.hpMax)
            MostrarDialogo($"{atacante.nombre} atacó a {objetivo.nombre}");
        else
            MostrarDialogo($"{atacante.nombre} se protege.");

        // Chequeo unico de fin de juego: dispara OnJuegoTerminado si corresponde
        // (esto es lo que escucha ControladorVictoria)
        GameManager.Instance.VerificarFinDeJuego();

        // Si el juego no termino, avanza el turno
        if (GameManager.Instance.GetVivos().Length > 1)
            GameManager.Instance.AvanzarTurno();
    }

    private void MostrarDialogo(string mensaje)
    {
        textoDialogo.text = mensaje;
    }
}