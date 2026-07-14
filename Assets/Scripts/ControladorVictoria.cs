using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControladorVictoria : MonoBehaviour
{
    [Header("Pantalla de Victoria")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoGanador;

    // Se suscribe al evento de GameManager apenas el objeto se activa
    void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnJuegoTerminado += ManejarFinDeJuego;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnJuegoTerminado -= ManejarFinDeJuego;
    }

    // Recibe el aviso de GameManager: si hay un ganador, muestra victoria; si no, empate
    private void ManejarFinDeJuego(Jugador ganador)
    {
        if (ganador != null)
            MostrarVictoria(ganador.nombre);
        else
            MostrarEmpate();
    }

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