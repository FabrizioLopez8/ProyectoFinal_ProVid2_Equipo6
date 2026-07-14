
// Interface que deben implementar todas las acciones del juego
// Siguiendo el patron Strategy: cada accion es una clase separada
public interface IAccion
{
    // Recibe quien ejecuta la accion y sobre quien se aplica
    void Ejecutar(Jugador atacante, Jugador objetivo);
}