using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public PlayerType pType;
    public BolsimonController bolsimon;
    // Start is called before the first frame update
    void Awake()
    {
        GetPlayerType();
        GetPlayerBolsimon();
        
    }
    void Start()
    {
        //print($"{PlayersData.player3} + {PlayersData.bolsimonP3}");
    }
    public void GetPlayerType()
    {
        char player = gameObject.name[gameObject.name.Length -1];
        switch (player)
        {
            case '1':
                pType = PlayersData.player1;
                break;
            case '2':
                pType = PlayersData.player2;
                break;
            case '3':
                pType = PlayersData.player3;
                break;
        }
    }

    public void GetPlayerBolsimon()
    {
        BolsimonController playerBolsimon = gameObject.GetComponentInChildren<BolsimonController>();
        if (pType == PlayerType.None) playerBolsimon.gameObject.SetActive(false);
        else
        {

            bolsimon = playerBolsimon;
            bolsimon.playerType = pType;
            char player = gameObject.name[gameObject.name.Length -1];
            switch (player)
            {
                case '1':
                    bolsimon.type = PlayersData.bolsimonP1;
                    break;
                case '2':
                    bolsimon.type = PlayersData.bolsimonP2;
                    break;
                case '3':
                    bolsimon.type = PlayersData.bolsimonP3;
                    break;     
            }


        }

    }
}

public enum PlayerType
{
    Player,
    IA,
    None
}