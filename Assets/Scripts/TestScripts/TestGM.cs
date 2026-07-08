using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
<<<<<<< HEAD
=======
// using UnityEditor.Animations;
>>>>>>> desarrollo-gaston
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
    public ControladorVictoria controladorVictoria;
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
<<<<<<< HEAD
        textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";
=======
        ActualizarCartelTurno();
>>>>>>> desarrollo-gaston
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

            // Chequeamos cuántos sobrevivieron
            int jugadoresVivos = players.Count(p => p.activeSelf == true);

            // SOLO iniciamos una nueva ronda si el juego no terminó
            if (jugadoresVivos > 1)
            {
                StartRound();
            }

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
<<<<<<< HEAD
                textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";
=======
                ActualizarCartelTurno();
>>>>>>> desarrollo-gaston
            }
        }

    }

    void EndTurn()
    {
        ExecuteActions();
        accionesTurno.Clear();
        print("turn ended");

        foreach (GameObject p in players)
        {
            if (p.GetComponent<TestBolsimon>().vida <= 0)
            {
                p.SetActive(false);
            }
        }

        CheckWinCondition();
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
<<<<<<< HEAD
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetFuego").SetActive(false);
=======
                GameObject.Find("BotonTargetFuego")?.SetActive(false);
>>>>>>> desarrollo-gaston
                bolsimon1Vida.gameObject.SetActive(false);
            }
        }
        else if (jugador.gameObject == players[1])
        {
            bolsimon2Vida.text = vida.ToString();
            if (vida <= 0)
            {
<<<<<<< HEAD
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetAgua").SetActive(false);
=======
                GameObject.Find("BotonTargetAgua")?.SetActive(false);
>>>>>>> desarrollo-gaston
                bolsimon2Vida.gameObject.SetActive(false);
            }
        }
        else
        {
            bolsimon3Vida.text = vida.ToString();
            if (vida <= 0)
            {
<<<<<<< HEAD
                textUI.text = $"{jugador.Name} ha sido derrotado!";
                GameObject.Find("BotonTargetPlanta").SetActive(false);
=======
                GameObject.Find("BotonTargetPlanta")?.SetActive(false);
>>>>>>> desarrollo-gaston
                bolsimon3Vida.gameObject.SetActive(false);
            }
        }
    }

    void CheckWinCondition()
    {
<<<<<<< HEAD
        //print("checking win conditions");
        //print(players.Count(GameObject => GameObject.activeSelf == true));
        if (players.Count(GameObject => GameObject.activeSelf == true) == 1)
        {
            textUI.text = $"El bolsimon {players[players.FindIndex(GameObject => GameObject.activeSelf)].GetComponent<TestBolsimon>().Name} ha ganado. Volviendo al menu.";
            returnToMenuTimerStart = true;
=======
        print("checking win conditions");

     
        int jugadoresVivos = players.Count(p => p.activeSelf == true);

        if (jugadoresVivos == 1)
        {
            
            GameObject jugadorGanadorObj = players.First(p => p.activeSelf == true);

            
            TestBolsimon scriptGanador = jugadorGanadorObj.GetComponent<TestBolsimon>();
            string nombreGanador = scriptGanador.Name;

            
            jugadorGanadorObj.transform.position = new Vector3(0, 0, 0);
            // jugadorGanadorObj.transform.localScale = new Vector3(2f, 2f, 2f);

            
            controladorVictoria.MostrarVictoria(nombreGanador);
        }
        else if (jugadoresVivos == 0) 
        {
            // Si nadie quedó vivo, llamamos a la nueva función de empate
            controladorVictoria.MostrarEmpate();
>>>>>>> desarrollo-gaston
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
    void ActualizarCartelTurno()
    {
        if (turnPlayer != null)
        {
            // Saca el nombre del componente TestBolsimon del jugador actual
            string nombreBolsimon = turnPlayer.GetComponent<TestBolsimon>().name;
            textUI.text = $"Turno de: {nombreBolsimon}";
        }
    }

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