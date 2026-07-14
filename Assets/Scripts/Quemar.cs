public class Quemar : IAccion
{public const int DANIO_QUEMADURA = 20;

    public void Ejecutar(Jugador atacante, Jugador objetivo)
    {
        foreach (Jugador j in GameManager.Instance.GetVivos())
        {
            if (j != atacante)
            {
                j.criatura.quemado = true;
                j.DispararQuemado(); // avisa al UIManager
            }
        }
    }
}
