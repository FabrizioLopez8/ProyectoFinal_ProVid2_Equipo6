using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPausa;

    private bool juegoPausado = false;

    void Update()
    {
        // Esto detecta si apretás la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f; 
        juegoPausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; 
        juegoPausado = false;
    }

    public void IrMenuPrincipal()
    {
        // ¡CRÍTICO! Siempre hay que descongelar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaMenu"); // Cambiá esto si tu escena de inicio se llama diferente
    }

    public void AbrirSonido()
    {
        // Acá podés apagar el panelPausa y prender tu panelSonido
        // panelPausa.SetActive(false);
        // panelSonido.SetActive(true);
        Debug.Log("Abriendo menú de sonido...");
    }
}