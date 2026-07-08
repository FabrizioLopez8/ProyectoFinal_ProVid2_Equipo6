using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControladorVictoria : MonoBehaviour
{
    [Header("Pantalla de Victoria")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoGanador;

    public void MostrarVictoria(string nombreDelGanador)
    {
 
        textoGanador.text = $"¡{nombreDelGanador} GANASTE LA BATALLA!";

        
        panelVictoria.SetActive(true);

        Time.timeScale = 0f;
    }
    public void MostrarEmpate()
    {
        textoGanador.text = "¡LA BATALLA TERMINÓ EN EMPATE!";

        
        panelVictoria.SetActive(true);

        
        Time.timeScale = 0f;
    }

}
