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

    // Recibe la accion elegida por el jugador activo y la ejecuta
    // La accion puede ser cualquier clase que implemente IAccion (Atacar, PonerEscudo, etc.)
    public void EjecutarAccion(Jugador atacante, Jugador objetivo, IAccion accion)
    {
        // Delega la logica a la clase de la accion — el BattleManager no sabe como funciona cada una
        accion.Ejecutar(atacante, objetivo);

        // Muestra el mensaje correspondiente segun el resultado de la accion
        if (objetivo.estado == EstadoJugador.Eliminado)
            MostrarDialogo($"¡{objetivo.nombre} fue eliminado!");
        else if (!objetivo.criatura.escudo && objetivo.criatura.hp < objetivo.criatura.hpMax)
            MostrarDialogo($"{atacante.nombre} atacó a {objetivo.nombre}");
        else
            MostrarDialogo($"{atacante.nombre} se protege.");

        // Verifica si el juego terminó
        if (GameManager.Instance.GetVivos().Length > 1)
            GameManager.Instance.AvanzarTurno(); // si quedan jugadores vivos, avanza el turno
        else
            MostrarDialogo($"¡Fin del juego!\nGanador: {GameManager.Instance.GetVivos()[0].nombre}");
    }

    // Actualiza el texto del cuadro de dialogo en la UI
    private void MostrarDialogo(string mensaje)
    {
        textoDialogo.text = mensaje;
    }
}