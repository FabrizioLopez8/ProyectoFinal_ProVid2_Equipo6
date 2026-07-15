using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
// using UnityEditor.Animations;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class TestGM : MonoBehaviour
{
    public static TestGM Instance { get; private set; }

    public GameObject turnPlayer;
    public List<GameObject> players;
    public TMP_Text textUI;
    public ControladorVictoria controladorVictoria;
    public TMP_Text bolsimon1Vida;
    public TMP_Text bolsimon2Vida;
    public TMP_Text bolsimon3Vida;

    [Header("Timer del Turno")]
    public TMP_Text textoTimer;
    private float tiempoTurno = 15f;
    private float timerActual;
    private bool timerActivo = false;

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
        for (int i = 0; i < childrenCount; i++)
        {
            players.Add(allPlayers.transform.GetChild(i).gameObject);
        }
        StartRound();
    }

    void Update()
    {
       
        if (timerActivo)
        {
            timerActual -= Time.deltaTime;
            textoTimer.text = Mathf.CeilToInt(timerActual).ToString();

            if (timerActual <= 0)
            {
                timerActivo = false;
                textoTimer.text = "0";
                ActualizarUI($"¡A {turnPlayer.GetComponent<TestBolsimon>().Name} se le acabó el tiempo!");
                NextPlayer();
            }
        }

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
        ActualizarCartelTurno();
        textUI.text = $"Es el turno de {turnPlayer.GetComponent<TestBolsimon>().Name}";

        
        timerActual = tiempoTurno;
        timerActivo = true;
    }

    public void AddAction(TestBolsimon origen, int indexHabilidad, TestBolsimon target, float speed, TargetType targetType)
    {
        accionesTurno.Add(new Action(origen, indexHabilidad, target, speed, targetType));
        NextPlayer();
    }

    void NextPlayer()
    {
        
        timerActivo = false;

        TestBolsimon lastPlayer = players[players.FindLastIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>();
        if (lastPlayer.currentTurn)
        {
            players[players.FindLastIndex(GameObject => GameObject.activeSelf == true)].GetComponent<TestBolsimon>().SetTurn(false);
            EndTurn();

            int jugadoresVivos = players.Count(p => p.activeSelf == true);

            if (jugadoresVivos > 1)
            {
                StartRound();
            }

            lastPlayer.SetTurn(false);
            StartCoroutine(EndTurn());
            return;
        }

        bool nextPlayerAwaiting = false;
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
                ActualizarCartelTurno();
                ActualizarUI($"Es el turno de {pTurn.Name}");

               
                timerActual = tiempoTurno;
                timerActivo = true;
            }
        }
    }

    private IEnumerator EndTurn()
    {
        
        timerActivo = false;
        if (textoTimer != null) textoTimer.text = "-";

        finishedActions = false;

        StartCoroutine(ExecuteActions());

        yield return new WaitUntil(() => finishedActions);

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
        foreach (GameObject p in players)
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
                foreach (TestBolsimon p in allOtherPlayers)
                {
                    a.origen.habilidades[a.indexHabilidad].Ejecutar(a.origen, p);

                    yield return new WaitUntil(() => UIWaitForUpdate);
                }
            }
        }
        foreach (GameObject p in players)
        {
            TestBolsimon pBol = p.GetComponent<TestBolsimon>();
            pBol.AplicarEfectos();

            yield return new WaitUntil(() => UIWaitForUpdate);
        }
        finishedActions = true;
    }

    public void ActualizarVida(TestBolsimon jugador, int vida, int vidaPreDano)
    {
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
                GameObject.Find("BotonTargetFuego")?.SetActive(false);
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                bolsimon1Vida.gameObject.SetActive(false);
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
                GameObject.Find("BotonTargetAgua")?.SetActive(false);
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                bolsimon2Vida.gameObject.SetActive(false);
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
                GameObject.Find("BotonTargetPlanta")?.SetActive(false);
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño y ha sido derrotado!");
                bolsimon3Vida.gameObject.SetActive(false);
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {vidaPreDano - vida} de daño");
            }
        }
    }

    void CheckWinCondition()
    {
        print("checking win conditions");
        int jugadoresVivos = players.Count(p => p.activeSelf == true);

        if (jugadoresVivos == 1)
        {
            GameObject jugadorGanadorObj = players.First(p => p.activeSelf == true);
            TestBolsimon scriptGanador = jugadorGanadorObj.GetComponent<TestBolsimon>();
            string nombreGanador = scriptGanador.Name;

            jugadorGanadorObj.transform.position = new Vector3(0, 0, 0);

            controladorVictoria.MostrarVictoria(nombreGanador);
        }
        else if (jugadoresVivos == 0)
        {
            controladorVictoria.MostrarEmpate();
        }
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("EscenaMenu");
    }

    void ActualizarCartelTurno()
    {
        if (turnPlayer != null)
        {
            string nombreBolsimon = turnPlayer.GetComponent<TestBolsimon>().Name;
            textUI.text = $"Turno de: {nombreBolsimon}";
        }
    }

    public void ShieldUsageMessage(TestBolsimon origen, string status)
    {
        if (status == "error") ActualizarUI($"{origen.Name} se ha quedado sin escudos.");
        else if (status == "success") ActualizarUI($"{origen.Name} ha usado un escudo. quedan {origen.shieldCount - 1} disponibles.");
    }

    void ActualizarUI(string text)
    {
        if (UIWaitForUpdate) { UIWaitForUpdate = false; }
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