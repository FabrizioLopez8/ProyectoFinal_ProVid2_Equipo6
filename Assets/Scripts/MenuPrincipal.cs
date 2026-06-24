using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    
    public GameObject panelMenuPrincipal; 
    public GameObject panelOpciones;      
    public GameObject panelCreditos;      

    public void Jugar()
    {
        SceneManager.LoadScene("EscenaJuego");
    }

    
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }


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
}
