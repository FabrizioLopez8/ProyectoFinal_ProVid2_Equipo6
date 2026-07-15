using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControladorVictoria : MonoBehaviour
{
    public static ControladorVictoria Instance { get ; private set ;}
    [Header("Pantalla de Victoria")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoGanador;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //Se suscribe al evento de TestGM apenas el objeto se activa
    void OnEnable()
    {
        if (TestGM.Instance != null)
            TestGM.Instance.OnJuegoTerminado += ManejarFinDeJuego;
    }

    void OnDisable()
    {
        if (TestGM.Instance != null)
            TestGM.Instance.OnJuegoTerminado -= ManejarFinDeJuego;
    }

    // Recibe el aviso de TestGM: si hay un ganador, muestra victoria; si no, empate
    public void ManejarFinDeJuego(BolsimonController ganador)
    {
        if (ganador != null)
            MostrarVictoria(ganador.Name);
        else
            MostrarEmpate();
    }

    public void MostrarVictoria(string nombreDelGanador)
    {
        textoGanador.text = $"�{nombreDelGanador} GANASTE LA BATALLA!";
        panelVictoria.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MostrarEmpate()
    {
        textoGanador.text = "�LA BATALLA TERMIN� EN EMPATE!";
        panelVictoria.SetActive(true);
        Time.timeScale = 0f;
    }
}