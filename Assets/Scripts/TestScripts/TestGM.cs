using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.SceneManagement;

public class TestGM : MonoBehaviour
{
    public static TestGM Instance { get ; private set ;}
    // Start is called before the first frame update

    public GameObject turnPlayer;
    public List<GameObject> players;
    public TMP_Text textUI;

    public TMP_Text bolsimon1Vida;
    public TMP_Text bolsimon2Vida;
    public TMP_Text bolsimon3Vida;

    public List<Action> accionesTurno = new List<Action>();

    private bool returnToMenuTimerStart = false;
    private float rtmt = 0.0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        GameObject allPlayers = GameObject.Find("Jugadores");
        int childrenCount = allPlayers.transform.childCount;
        for (int i =0; i < childrenCount; i++)
        {
            players.Add(allPlayers.transform.GetChild(i).gameObject);
        }
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (returnToMenuTimerStart)
        {
            rtmt += Time.deltaTime;
            if (rtmt >= 5.0f)
            {
                ReturnToMainMenu();
            }
        }
    }

    void StartRound()
    {
        players[players.FindIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>().SetTurn(true);
        turnPlayer = players[players.FindIndex(GameObject => GameObject.activeSelf == true)];
        textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";
    }

    public void AddAction(TestBolsimon origen, int indexHabilidad, TestBolsimon target)
    {
        accionesTurno.Add(new Action(origen, indexHabilidad, target));
        NextPlayer();
    }

    void NextPlayer()
    {
        print("paso al turno del siguiente jugador");
        if (players[players.FindLastIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>().currentTurn)
        {
            players[players.FindLastIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>().SetTurn(false);
            EndTurn();
            StartRound();
            return;
        }

        bool nextPlayerAwaiting = false; print(turnPlayer);
        foreach (GameObject p in players)
        {
            TestBolsimon pTurn = p.GetComponent<TestBolsimon>();
            if (p.activeSelf != true)
            {
                continue;
            }
            if (turnPlayer == p)
            {
                nextPlayerAwaiting = true;
                pTurn.SetTurn(false);
            }
            else if (nextPlayerAwaiting)
            {
                nextPlayerAwaiting = false;
                pTurn.SetTurn(true);
                turnPlayer = p;
                textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";
            }
        }

    }

    void EndTurn()
    {
        ExecuteActions();
        accionesTurno.Clear();
        print("turn ended");
    }

    void ExecuteActions()
    {
        foreach (Action a in accionesTurno)
        {
            if (a.origen.isActiveAndEnabled != true || a.target.isActiveAndEnabled != true)
            {
                continue;
            }
            a.origen.habilidades[a.indexHabilidad].Ejecutar(a.origen, a.target);
            textUI.text = $"{a.origen.Name} ha usado {a.origen.habilidades[a.indexHabilidad].Nombre} en {a.target.Name}";
        }
    }

    public void ActualizarVida(TestBolsimon jugador, int vida)
    {
        if (jugador.gameObject == players[0])
        {
            bolsimon1Vida.text = vida.ToString();
            if (vida <= 0)
            {
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetFuego").SetActive(false);
                bolsimon1Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
        }
        else if (jugador.gameObject == players[1])
        {
            bolsimon2Vida.text = vida.ToString();
            if (vida <= 0)
            {
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetAgua").SetActive(false);
                bolsimon2Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
        }
        else
        {
            bolsimon3Vida.text = vida.ToString();
            if (vida <= 0)
            {
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetPlanta").SetActive(false);
                bolsimon3Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
        }
    }

    void CheckWinCondition()
    {
        //print("checking win conditions");
        //print(players.Count(GameObject => GameObject.activeSelf == true));
        if (players.Count(GameObject => GameObject.activeSelf == true) == 1)
        {
            textUI.text = $"El bolsimon {players[players.FindIndex(GameObject => GameObject.activeSelf)].GetComponent<TestBolsimon>().Name} ha ganado. Volviendo al menu.";
            returnToMenuTimerStart = true;
        }
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("EscenaMenu");
    }

    // void RemoveAction(TestBolsimon origen)
    // {
    //     accionesTurno.RemoveAll(Action => Action.origen == origen);
    //     accionesTurno.RemoveAll(Action => Action.target == origen);
    // }

    // public void Hola()
    // {
    //     print("hola");
    // }

}

public class Action
{
    public TestBolsimon origen;
    public int indexHabilidad;
    public TestBolsimon target;

    public Action(TestBolsimon origen, int indexHabilidad, TestBolsimon target)
    {
        this.origen = origen;
        this.indexHabilidad = indexHabilidad;
        this.target = target;
    }
}