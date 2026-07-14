// Habilidad de Agua: el objetivo queda "debilitado" y su proximo ataque hace menos daño.
public class ReducirAtaque : IAccion
{
    public const float MULTIPLICADOR_REDUCCION = 0.5f; // el objetivo hace 50% menos daño en su proximo ataque

    public void Ejecutar(Jugador atacante, Jugador objetivo)
    {
        objetivo.criatura.danioReducido = true;
    }
}
