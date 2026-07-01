using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Paneles del menú
    public GameObject panelMenuPrincipal;
    public GameObject panelOpciones;
    public GameObject panelCreditos;
    public GameObject panelSeleccion; 

    // --- FUNCIONES DEL MENÚ PRINCIPAL ---

    public void AbrirSeleccion()
    {
        panelMenuPrincipal.SetActive(false);
        panelSeleccion.SetActive(true);
    }

    public void VolverDeSeleccion()
    {
        panelSeleccion.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    public void IniciarBatalla()
    {
        SceneManager.LoadScene("EscenaJuego"); 
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // --- FUNCIONES DE OPCIONES Y CRÉDITOS ---
    public void AbrirOpciones()
    {
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void AbrirCreditos()
    {
        panelMenuPrincipal.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void VolverDeOpciones()
    {
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    public void CerrarCreditos()
    {
        panelCreditos.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }
    public void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        Debug.Log("Pantalla completa: " + pantallaCompleta);
    }
}