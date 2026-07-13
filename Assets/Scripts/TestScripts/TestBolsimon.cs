using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class TestBolsimon : MonoBehaviour
{

    public string Name;
    public TestBolsimon target;
    public string type;
    public bool currentTurn;
    public float speed;
    public float baseSpeed;

    public int danoBase;
    public int vida;
    public int vidaBase;
    public int dano;
    public bool isShielded = false;
    public int shieldCount = 3;

    private bool waitTimePassed = true;
    private float waitTime = 0.0f;

    public string efectoNegativo;
    public int efectoNegativoDuracion;
    private TestBolsimon efectoNegativoOrigen;

    public List<ITestAbilities> habilidades = new List<ITestAbilities>();
    // Start is called before the first frame update
    void Start()
    {
        habilidades.Add(new Attack());
        habilidades.Add(new Shield());
        foreach (ITestAbilities hab in TestGM.Instance.testHabilidades)
        {
            if (hab.Tipo == this.type)
            {
                habilidades.Add(hab);
            } 
        }
        foreach (ITestAbilities a in habilidades)
        {
            print(a.Nombre);
        }

        baseSpeed = speed;
        danoBase = dano;
        vidaBase = vida;
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
        //     habilidades[0].Ejecutar(this, target);
        // }
    }

    public void SetTarget(TestBolsimon target1)
    {
        if (currentTurn && target1 != this)
        {
            target = target1;
        }
    }

    public void TestRecibir(TestBolsimon origen, string nombreHabilidad)
    {
        print($"{origen.Name} ha usado {nombreHabilidad} en {Name}");
    }

    public void RecibirAtaque(TestBolsimon origen, int danoARecibir)
    {
       
        int vidaPreDano = vida;

        vida -= danoARecibir;

        TestGM.Instance.ActualizarVida(this, vida, vidaPreDano);
    }

    public void SetTurn(bool value)
    {
        target = null;
        currentTurn = value;
        waitTimePassed = false;
        waitTime = 0.0f;
    }

    public void Attack()
    {
        if (currentTurn && target != null)
        {
            //TestGM.Instance.Hola();
            //print($"{this} + {habilidades.FindIndex(h => h.Nombre == "Ataque")} + {target}");
            int actionIndex = habilidades.FindIndex(h => h.Nombre == "Ataque");
            TestGM.Instance.AddAction(this, actionIndex, target, speed, habilidades[actionIndex].targetType);
        }
    }

    public void Shield()
    {
        if (currentTurn && waitTimePassed)
        {
            int actionIndex = habilidades.FindIndex(h => h.Nombre == "Escudo");
            TestGM.Instance.AddAction(this, actionIndex, this, speed * 10, habilidades[actionIndex].targetType);
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

    public void Habilidad()
    {
        if (currentTurn && waitTimePassed)
        {
            int actionIndex = habilidades.FindIndex(h => h.Tipo == type);
            TestGM.Instance.AddAction(this, actionIndex, this, speed, habilidades[actionIndex].targetType);
        }
    }

    public void AplicarEfectoNegativo(string efectoNegativo, int duración, TestBolsimon origen)
    {
        this.efectoNegativo = efectoNegativo;
        this.efectoNegativoDuracion = duración;
        this.efectoNegativoOrigen = origen;
    }

    public void AplicarEfectos()
    {
        if (efectoNegativo != null && efectoNegativoDuracion > 0)
        {
            RecibirAtaque(efectoNegativoOrigen, ((int)(efectoNegativoOrigen.dano * 0.30)));
            efectoNegativoDuracion -= 1;
        }
        else if (efectoNegativo != null && efectoNegativoDuracion == 0)
        {
            efectoNegativo = null;
            efectoNegativoOrigen = null;
        }
    }

    public void Curarse()
    {
        int vidaPreCura = vida;
        vida += ((int)(vidaBase * 0.25));
        TestGM.Instance.ActualizarVida(this, vida, vidaPreCura);
    }

    public void OnTurnEnd()
    {
        if (isShielded)
        {
            isShielded = false;
            shieldCount -= 1;
        }
    }
}
