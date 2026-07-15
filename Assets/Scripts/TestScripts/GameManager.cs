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

    public BolsimonController turnPlayer;
    public List<GameObject> players;
    public List<BolsimonController> playersBolsimon;
    public TMP_Text textUI;

    public TMP_Text bolsimonHealth1;
    public TMP_Text bolsimonHealth2;
    public TMP_Text bolsimonHealth3;

    public List<Action> turnActions = new List<Action>();
    private bool finishedActions = false;

    private bool returnToMenuTimerStart = false;
    private float rtmt = 0.0f;

    //public List<Abilities> testHabilidades = new List<Abilities>();

    private bool UIWaitForUpdate;
    private bool UIWaitTimeStart;
    public float UIWaitTimerDuration;
    private float UIWaitTimer;
    private bool WaitForRoundStart = true;

    public event System.Action<BolsimonController> OnJuegoTerminado;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //GenerateAbilityList();
    }
    void Start()
    {
        GameObject allPlayers = GameObject.Find("Jugadores");
        int childrenCount = allPlayers.transform.childCount;
        for (int i =0; i < childrenCount; i++)
        {
            players.Add(allPlayers.transform.GetChild(i).gameObject);
        }
        //foreach (GameObject p in players)
        foreach(GameObject p in players)
        {
            playersBolsimon.Add(p.transform.GetComponentInChildren<BolsimonController>());
        }
        StartRound();
        SetUIStartingHealth();
        UIManager.Instance.Preparations();
    }

    void SetUIStartingHealth()
    {
        bolsimonHealth1.text = playersBolsimon[0].Health.ToString();
        bolsimonHealth2.text = playersBolsimon[1].Health.ToString();
        bolsimonHealth3.text = playersBolsimon[2].Health.ToString();
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
        BolsimonController firstActiveBolsimon = playersBolsimon[playersBolsimon.FindIndex(gameObject => gameObject.isActiveAndEnabled == true)];
        firstActiveBolsimon.SetTurn(true);
        turnPlayer = firstActiveBolsimon;
        if (turnPlayer.playerType == PlayerType.IA) turnPlayer.MakeIAPlay();
        textUI.text = $"Es el turno de {turnPlayer.Name}";
    }

    public void AddAction(BolsimonController origin, int abilityIndex, BolsimonController target, float speed, TargetType targetType)
    {
        turnActions.Add(new Action(origin, abilityIndex, target, speed, targetType));
        NextPlayer();
    }

    void NextPlayer()
    {
        //print("paso al turno del siguiente jugador");
        BolsimonController lastPlayer = playersBolsimon[playersBolsimon.FindLastIndex(gameObject => gameObject.isActiveAndEnabled == true)];
        if (lastPlayer.currentTurn)
        {
            lastPlayer.SetTurn(false);
            StartCoroutine(EndTurn());
            return;
        }

        bool nextPlayerAwaiting = false; //print(turnPlayer);
        foreach (BolsimonController b in playersBolsimon)
        {
            if (b.isActiveAndEnabled != true)
            {
                continue;
            }
            if (turnPlayer == b)
            {
                nextPlayerAwaiting = true;
                b.SetTurn(false);
            }
            else if (nextPlayerAwaiting)
            {
                nextPlayerAwaiting = false;
                b.SetTurn(true);
                turnPlayer = b;
                if (turnPlayer.playerType == PlayerType.IA) turnPlayer.MakeIAPlay();
                ActualizarUI($"Es el turno de {b.Name}");
            }
        }

    }

    private IEnumerator EndTurn()
    {
        finishedActions = false;

        StartCoroutine(ExecuteActions());

        yield return new WaitUntil(() => finishedActions);

        turnActions.Clear();
        foreach(BolsimonController b in playersBolsimon)
        {
            b.OnTurnEnd();
        }
        WaitForRoundStart = false;
    }

    private IEnumerator ExecuteActions()
    {
        turnActions.Sort((a, b) => b.speed.CompareTo(a.speed));
        foreach (Action a in turnActions)
        {
            //primero checkeamos que tanto el target como el origen todavia esten vivos
            if (a.origin.isActiveAndEnabled != true || a.target.isActiveAndEnabled != true)
            {
                continue;
            }

            //revisamos si la habilidad es de hacia un solo target o a varios y aplicamos correctamente.
            if (a.targetType == TargetType.SingleTarget)
            {
                if (a.origin.abilities[a.abilityIndex].Name != "Escudo") 
                {
                    ActualizarUI($"{a.origin.Name} ha usado {a.origin.abilities[a.abilityIndex].Name} en {a.target.Name}");
                    yield return new WaitUntil(() => UIWaitForUpdate);
                }

                a.origin.abilities[a.abilityIndex].Execute(a.origin, a.target);

                yield return new WaitUntil(() => UIWaitForUpdate);
            }
            else if (a.targetType == TargetType.AllExceptSelf)
            {
                ActualizarUI($"{a.origin.Name} ha usado {a.origin.abilities[a.abilityIndex].Name}");

                yield return new WaitUntil(() => UIWaitForUpdate);

                List<BolsimonController> allOtherPlayers = new List<BolsimonController>();
                foreach (BolsimonController b in playersBolsimon)
                {
                    if (!b.isActiveAndEnabled) continue;
                    if (b.Name != a.origin.Name) allOtherPlayers.Add(b);
                }
                foreach(BolsimonController b in allOtherPlayers)
                {
                    a.origin.abilities[a.abilityIndex].Execute(a.origin, b);

                    yield return new WaitUntil(() => UIWaitForUpdate);
                }
            }
        }
        foreach(BolsimonController b in playersBolsimon)
        {
            if (b.isActiveAndEnabled == true)
            {
                b.ApplyDebuffEffectsOnTurnEnd();
                
                yield return new WaitUntil(() => UIWaitForUpdate); 
            }

        }
        finishedActions = true;
    }

    public void UpdateHealth(BolsimonController jugador, int health, int previousHealth)
    {
        // TODO: Actualizar este terrible choclo a algo que no se repita tanto.
        if (jugador == playersBolsimon[0])
        {
            bolsimonHealth1.text = health.ToString();

            if (health > previousHealth)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {health - previousHealth} de health!");
                return;
            }

            if (health <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetP1").SetActive(false);
                bolsimonHealth1.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño");
            }
        }
        else if (jugador == playersBolsimon[1])
        {
            bolsimonHealth2.text = health.ToString();
            if (health > previousHealth)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {health - previousHealth} de health!");
                return;
            }

            if (health <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetP2").SetActive(false);
                bolsimonHealth2.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño");
            }
        }
        else
        {
            bolsimonHealth3.text = health.ToString();
            if (health > previousHealth)
            {
                ActualizarUI($"¡{jugador.Name} ha recuperado {health - previousHealth} de health!");
                return;
            }
            if (health <= 0)
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño y ha sido derrotado!");
                GameObject.Find("BotonTargetP3").SetActive(false);
                bolsimonHealth3.gameObject.SetActive(false);
                // RemoveAction(jugador);
                CheckWinCondition();
            }
            else
            {
                ActualizarUI($"¡{jugador.Name} ha recibido {previousHealth - health} de daño");
            }
        }
    }

    void CheckWinCondition()
    {
        //print("checking win conditions");
        //print(players.Count(GameObject => GameObject.activeSelf == true));
        if (playersBolsimon.Count(gameObject => gameObject.isActiveAndEnabled == true) == 1)
        {
            // textUI.text = $"El bolsimon {playersBolsimon[playersBolsimon.FindIndex(gameObject => gameObject.isActiveAndEnabled)].Name} ha ganado. Volviendo al menu.";
            // returnToMenuTimerStart = true;
            //OnJuegoTerminado?.Invoke(playersBolsimon.Find(gameObject => gameObject.isActiveAndEnabled == true));
            ControladorVictoria.Instance.ManejarFinDeJuego(playersBolsimon.Find(gameObject => gameObject.isActiveAndEnabled == true));
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("EscenaMenu");
    }

    // void RemoveAction(BolsimonController origin)
    // {
    //     turnActions.RemoveAll(Action => Action.origin == origin);
    //     turnActions.RemoveAll(Action => Action.target == origin);
    // }

    // public void Hola()
    // {
    //     print("hola");
    // }

    public void IsShielded(BolsimonController origin)
    {
        ActualizarUI($"¡{origin.Name} fue protegido por su Escudo!");
    }

    public void ShieldUsageMessage(BolsimonController origin, string status)
    {
        if (status == "error") ActualizarUI($"{origin.Name} se ha quedado sin escudos.");
        else if (status == "succes") ActualizarUI($"{origin.Name} ha usado un escudo. quedan {origin.shieldCount - 1} disponibles.");
    }

    void ActualizarUI(string text)
    {
        if (UIWaitForUpdate) {UIWaitForUpdate = false;}
        textUI.text = text;
        UIWaitTimeStart = true;
    }

    // void GenerateAbilityList()
    // {
    //     testHabilidades.Add(new MuroLLamas());
    //     testHabilidades.Add(new Lluvia());
    //     testHabilidades.Add(new Fotosintesis());
    // }

}

public enum TargetType
{
    SingleTarget,
    AllExceptSelf
}
public class Action
{
    public BolsimonController origin;
    public int abilityIndex;
    public BolsimonController target;
    public float speed;
    public TargetType targetType;

    public Action(BolsimonController origin, int abilityIndex, BolsimonController target, float speed, TargetType targetType)
    {
        this.origin = origin;
        this.abilityIndex = abilityIndex;
        this.target = target;
        this.speed = speed;
        this.targetType = targetType;
    }
}