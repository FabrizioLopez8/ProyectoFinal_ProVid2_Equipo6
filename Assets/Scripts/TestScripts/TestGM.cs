using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    private bool finishedActions = false;

    private bool returnToMenuTimerStart = false;
    private float rtmt = 0.0f;

    public List<ITestAbilities> testHabilidades = new List<ITestAbilities>();

    private bool UIWaitForUpdate;
    private bool UIWaitTimeStart;
    public float UIWaitTimerDuration;
    private float UIWaitTimer;
    private bool WaitForRoundStart = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GenerateAbilityList();
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

        if (UIWaitTimeStart)
        {
            UIWaitTimer += Time.deltaTime;
            if (UIWaitTimer >= UIWaitTimerDuration)
            {
                UIWaitTimer = 0.0f;
                UIWaitForUpdate = true;
                UIWaitTimeStart = false;
            }
        }

        if (!WaitForRoundStart)
        {
            StartRound();
            WaitForRoundStart = true;
        }
    }

    void StartRound()
    {
        players[players.FindIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>().SetTurn(true);
        turnPlayer = players[players.FindIndex(GameObject => GameObject.activeSelf == true)];
        textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";
    }

    public void AddAction(TestBolsimon origen, int indexHabilidad, TestBolsimon target, float speed, TargetType targetType)
    {
        accionesTurno.Add(new Action(origen, indexHabilidad, target, speed, targetType));
        NextPlayer();
    }

    void NextPlayer()
    {
        //print("paso al turno del siguiente jugador");
        TestBolsimon lastPlayer = players[players.FindLastIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>();
        if (lastPlayer.currentTurn)
        {
            lastPlayer.SetTurn(false);
            StartCoroutine(EndTurn());
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
                ActualizarUI($"Es el turno de {pTurn.Name}");
            }
        }

    }

    private IEnumerator EndTurn()
    {
        finishedActions = false;

        StartCoroutine(ExecuteActions());

        yield return new WaitUntil(() => finishedActions);

        accionesTurno.Clear();
        foreach(GameObject p in players)
        {
            p.GetComponent<TestBolsimon>().OnTurnEnd();
        }
        WaitForRoundStart = false;
    }

    private IEnumerator ExecuteActions()
    {
        accionesTurno.Sort((a, b) => b.speed.CompareTo(a.speed));
        foreach (Action a in accionesTurno)
        {
            if (a.origen.isActiveAndEnabled != true || a.target.isActiveAndEnabled != true)
            {
                continue;
            }

            if (a.targetType == TargetType.SingleTarget)
            {
                ActualizarUI($"{a.origen.Name} ha usado {a.origen.habilidades[a.indexHabilidad].Nombre} en {a.target.Name}");

                yield return new WaitUntil(() => UIWaitForUpdate);

                a.origen.habilidades[a.indexHabilidad].Ejecutar(a.origen, a.target);

                yield return new WaitUntil(() => UIWaitForUpdate);
            }
            else if (a.targetType == TargetType.AllExceptSelf)
            {
                ActualizarUI($"{a.origen.Name} ha usado {a.origen.habilidades[a.indexHabilidad].Nombre}");

                yield return new WaitUntil(() => UIWaitForUpdate);

                List<TestBolsimon> allOtherPlayers = new List<TestBolsimon>();
                foreach (GameObject p in players)
                {
                    if (!p.activeSelf) continue;
                    TestBolsimon pBol = p.GetComponent<TestBolsimon>();
                    if (pBol.Name != a.origen.Name) allOtherPlayers.Add(pBol);
                }
                foreach(TestBolsimon p in allOtherPlayers)
                {
                    a.origen.habilidades[a.indexHabilidad].Ejecutar(a.origen, p);

                    yield return new WaitUntil(() => UIWaitForUpdate);
                }
            }
        }
        foreach(GameObject p in players)
        {
            TestBolsimon pBol = p.GetComponent<TestBolsimon>();
            pBol.AplicarEfectos();

            yield return new WaitUntil(() => UIWaitForUpdate); 
        }
        finishedActions = true;
    }

    public void ActualizarVida(TestBolsimon jugador, int vida, int vidaPreDano)
    {
        // TODO: Actualizar este terrible choclo a algo que no se repita tanto.
        if (jugador.gameObject == players[0])
        {
            bolsimon1Vida.text = vida.ToString();

            if (vida > vidaPreDano)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {vida - vidaPreDano} de vida!");
                return;
            }

            if (vida <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetFuego").SetActive(false);
                bolsimon1Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño");
            }
        }
        else if (jugador.gameObject == players[1])
        {
            bolsimon2Vida.text = vida.ToString();
            if (vida > vidaPreDano)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {vida - vidaPreDano} de vida!");
                return;
            }

            if (vida <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetAgua").SetActive(false);
                bolsimon2Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño");
            }
        }
        else
        {
            bolsimon3Vida.text = vida.ToString();
            if (vida > vidaPreDano)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {vida - vidaPreDano} de vida!");
                return;
            }
            if (vida <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetPlanta").SetActive(false);
                bolsimon3Vida.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño");
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

    public void IsShielded(TestBolsimon origen)
    {
        ActualizarUI($"¡{origen.Name} fue protegido por su Escudo!");
    }

    public void ShieldUsageMessage(TestBolsimon origen, string status)
    {
        if (status == "error") ActualizarUI($"{origen.Name} se ha quedado sin escudos.");
        else if (status == "succes") ActualizarUI($"{origen.Name} ha usado un escudo. quedan {origen.shieldCount - 1} disponibles.");
    }

    void ActualizarUI(string text)
    {
        if (UIWaitForUpdate) {UIWaitForUpdate = false;}
        textUI.text = text;
        UIWaitTimeStart = true;
    }

    void GenerateAbilityList()
    {
        testHabilidades.Add(new MuroLLamas());
        testHabilidades.Add(new Lluvia());
        testHabilidades.Add(new Fotosintesis());
    }

}

public enum TargetType
{
    SingleTarget,
    AllExceptSelf
}
public class Action
{
    public TestBolsimon origen;
    public int indexHabilidad;
    public TestBolsimon target;
    public float speed;
    public TargetType targetType;

    public Action(TestBolsimon origen, int indexHabilidad, TestBolsimon target, float speed, TargetType targetType)
    {
        this.origen = origen;
        this.indexHabilidad = indexHabilidad;
        this.target = target;
        this.speed = speed;
        this.targetType = targetType;
    }
}