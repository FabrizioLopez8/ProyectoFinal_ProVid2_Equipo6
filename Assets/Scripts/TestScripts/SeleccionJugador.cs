using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TipoJugador : MonoBehaviour
{
    public TextMeshProUGUI textoDelBoton;

    private PlayerType currentType = PlayerType.Player; 

    private Dictionary<PlayerType, string> opciones = new Dictionary<PlayerType, string>
    {
        {PlayerType.Player, "JUGADOR"},
        {PlayerType.IA, "IA"},
        {PlayerType.None, "NADIE"}
    };

    void Start()
    {
        AssignPlayerType(currentType);
    }

    public void CambiarTipo()
    {
        currentType++;
        // codigo generado por IA, basicamente se fija que currenType sea un valor definido de PlayerType
        if (!System.Enum.IsDefined(typeof(PlayerType), currentType)) {
            currentType = PlayerType.Player;
        }
        textoDelBoton.text = opciones[currentType];

        AssignPlayerType(currentType);
    }

    void AssignPlayerType(PlayerType playerType)
    {
        char player = gameObject.name[gameObject.name.Length -1];
        switch (player)
        {
            case '1': 
                PlayersData.player1 = playerType;
                print(PlayersData.player1);
                break;
            case '2': 
                PlayersData.player2 = playerType; 
                print(PlayersData.player2);
                break;
            case '3': 
                PlayersData.player3 = playerType; 
                print(PlayersData.player3);
                break;

        }
    }
}