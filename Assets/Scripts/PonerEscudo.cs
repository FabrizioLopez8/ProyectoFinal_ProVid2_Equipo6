// Accion defensiva: activa el escudo del atacante para absorber el proximo ataque
public class PonerEscudo : IAccion
{
    public void Ejecutar(Jugador atacante, Jugador objetivo)
    {
        // Activa el escudo del jugador que eligio esta accion
        // El objetivo no se usa en esta accion
        atacante.criatura.escudo = true;
    }
}