using UnityEngine;
using TMPro;

public class TipoJugador : MonoBehaviour
{
    public TextMeshProUGUI textoDelBoton; 
    private string[] opciones = { "JUGADOR", "IA", "NADIE" };
    private int indice = 0;

    public void CambiarTipo()
    {
        indice++;
        if (indice >= opciones.Length)
        {
            indice = 0;
        }
        textoDelBoton.text = opciones[indice];
    }
}