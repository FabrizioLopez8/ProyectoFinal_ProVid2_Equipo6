using UnityEngine;
using UnityEngine.UI; 

public class SoundManager : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioSource musicaFondo; 

    void Start()
    {
  
        if (musicaFondo != null)
        {
            musicaFondo.loop = true;
            if (!musicaFondo.isPlaying)
            {
                musicaFondo.Play();
            }
        }
    }

   
    public void CambiarVolumen(float valorVolumen)
    {
        if (musicaFondo != null)
        {
            musicaFondo.volume = valorVolumen;
        }
    }
}