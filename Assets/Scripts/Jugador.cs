// Jugador.cs
using System;

public enum EstadoJugador { Vivo, Eliminado }

[Serializable]
public class Jugador
{
    public string nombre;
    public Bolsimon criatura;
    public EstadoJugador estado = EstadoJugador.Vivo;

    // Eventos que dispara el jugador cuando algo cambia
    public event System.Action OnQuemado;
    public event System.Action OnCurado;
    public event System.Action OnDanioRecibido;
    public event System.Action OnEliminado;

    // Metodos para disparar los eventos desde las acciones
    public void DispararQuemado() => OnQuemado?.Invoke();
    public void DispararCurado() => OnCurado?.Invoke();
    public void DispararDanio() => OnDanioRecibido?.Invoke();
    public void DispararEliminado() => OnEliminado?.Invoke();

    // Limpia todas las suscripciones. Solo se puede asignar "= null" a un evento
    // desde ADENTRO de la clase que lo declara, por eso este metodo vive aca
    // y no se hace jugador.OnQuemado = null directamente desde UIManager.
    public void LimpiarEventos()
    {
        OnQuemado = null;
        OnCurado = null;
        OnDanioRecibido = null;
        OnEliminado = null;
    }
}