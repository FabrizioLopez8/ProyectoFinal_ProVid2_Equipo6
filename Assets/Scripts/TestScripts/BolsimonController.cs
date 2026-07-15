using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class BolsimonController : MonoBehaviour
{

    public string Name;
    public BolsimonController Target;
    public ElementType type;
    public PlayerType playerType;
    public bool currentTurn;
    
    
    private float baseSpeed;
    public float Speed {get; private set;}
    private float BaseDamage;
    public float Damage {get; private set;}
    public int baseHealth;
    public int Health;
   
    
    public List<Abilities> abilities = new List<Abilities>();
   

    public bool isShielded = false;
    public int shieldCount = 3;

    private bool waitTimePassed = true;
    private float waitTime = 0.0f;

    public string debuff;
    public int debuffDuration;
    private BolsimonController debuffOrigin;

    public IAStrategy iaStrategy;

    void Awake()
    {
        GetBolsimonStatsAndAbilities();
        SetBaseStats();
    }
    // Start is called before the first frame update
    void Start()
    {
        // foreach (Abilities a in abilities)
        // {
        //     print(a.Name);
        // }

    }

    // Update is called once per frame
    void Update()
    {
        if (!waitTimePassed)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 1.0f) waitTimePassed = true;
        }
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     abilities[0].Ejecutar(this, Target);
        // }
    }

    public void SetTarget(BolsimonController target1)
    {
        if (currentTurn)
        {
            Target = target1;
        }
    }

//     public void TestRecibir(BolsimonController origin, string abilityName)
//     {
//         print($"{origin
// .Name} ha usado {abilityName} en {Name}");
//     }

    public void ReceiveAttack(BolsimonController origin, float damage)
    {
        int currentHealth = Health;
        if (isShielded)
        {
            TestGM.Instance.IsShielded(this);
        }
        else
        {
            Health -= (int)damage;
            if (Health <= 0)
            {
                gameObject.SetActive(false);
            }
            TestGM.Instance.UpdateHealth(this, Health, currentHealth);
        }
    }

    public void SetTurn(bool value)
    {
        Target = null;
        currentTurn = value;
        if (playerType != PlayerType.IA)
        {
            waitTimePassed = false;
            waitTime = 0.0f;
        }
    }

    public void Attack()
    {
        if (currentTurn && Target != null)
        {
            print("a");
            //TestGM.Instance.Hola();
            //print($"{this} + {abilities.FindIndex(h => h.Name == "Ataque")} + {Target}");
            int actionIndex = abilities.FindIndex(h => h.Name == "Ataque");
            TestGM.Instance.AddAction(this, actionIndex, Target, Speed, abilities[actionIndex].targetType);
        }
    }

    public void Shield()
    {
        if (currentTurn && waitTimePassed)
        {
            print("s");
            int actionIndex = abilities.FindIndex(h => h.Name == "Escudo");
            TestGM.Instance.AddAction(this, actionIndex, this, Speed * 10, abilities[actionIndex].targetType);
        }
    }

    public void UseShield()
    {
        if (shieldCount <= 0)
        {
            TestGM.Instance.ShieldUsageMessage(this, "error");
        }
        else
        {
            TestGM.Instance.ShieldUsageMessage(this, "success");
            isShielded = true;
        }
    }

    public void Ability()
    {
        if (currentTurn && waitTimePassed)
        {
            print("h");
            int actionIndex = abilities.FindIndex(h => h.Tipo == type);
            TestGM.Instance.AddAction(this, actionIndex, this, Speed, abilities[actionIndex].targetType);
        }
    }

    public void ApplyDebuffs(string debuff, int duration, BolsimonController origin)
    {
        this.debuff = debuff;
        debuffDuration = duration;
        debuffOrigin = origin;
    }

    public void ApplyDebuffEffectsOnTurnEnd()
    {
        if (debuff != null && debuffDuration > 0)
        {
            ReceiveAttack(debuffOrigin, (int)(debuffOrigin.Damage * 0.36));
            debuffDuration -= 1;
        }
        else if (debuff != null && debuffDuration == 0)
        {
            debuff = null;
            debuffOrigin = null;
        }
    }

    public void Heal()
    {
        int currentHealth = Health;
        Health += (int)(baseHealth * 0.25);
        TestGM.Instance.UpdateHealth(this, Health, currentHealth);
    }

    public void OnTurnEnd()
    {
        if (isShielded)
        {
            isShielded = false;
            shieldCount -= 1;
        }
    }

    void GetBolsimonStatsAndAbilities()
    {
        Name = Bolsimon.Name(type);
        baseSpeed = Bolsimon.BaseSpeed(type);
        BaseDamage = Bolsimon.BaseDamage(type);
        baseHealth = Bolsimon.BaseHealth(type);
        abilities = Bolsimon.Abilities(type);
        gameObject.GetComponent<SpriteRenderer>().color = Bolsimon.Colors(type);
        if (playerType == PlayerType.IA)
        {
            iaStrategy = Bolsimon.GetIAStrategy(type);
        }
    }

    void SetBaseStats()
    {
        Speed = baseSpeed;
        Damage = BaseDamage;
        Health = baseHealth;
    }

    public void MakeIAPlay()
    {
        print("AiLogic");
        iaStrategy.ChooseAction(this);
    }

    // Eventos que dispara el jugador cuando algo cambia
    public event System.Action OnQuemado;
    public event System.Action OnCurado;
    public event System.Action OnDanioRecibido;
    public event System.Action OnEliminado;

    // Metodos para disparar los eventos desde las acciones
    public void DispararQuemado() => OnQuemado?.Invoke();
    public void DispararCurado() => OnCurado?.Invoke();
    public void DispararDanio() => OnDanioRecibido?.Invoke();
    public void DispararEliminado() => OnEliminado?.Invoke();

    // Limpia todas las suscripciones. Solo se puede asignar "= null" a un evento
    // desde ADENTRO de la clase que lo declara, por eso este metodo vive aca
    // y no se hace jugador.OnQuemado = null directamente desde UIManager.
    public void LimpiarEventos()
    {
        OnQuemado = null;
        OnCurado = null;
        OnDanioRecibido = null;
        OnEliminado = null;
    }
}
