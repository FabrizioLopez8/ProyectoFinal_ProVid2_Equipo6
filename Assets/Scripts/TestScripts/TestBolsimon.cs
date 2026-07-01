using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestBolsimon : MonoBehaviour
{

    public string Name;
    public TestBolsimon target;
    public bool currentTurn;

    public int vida;
    public int dano;

    public List<ITestAbilities> habilidades = new List<ITestAbilities>();
    // Start is called before the first frame update
    void Start()
    {
        habilidades.Add(new Attack());
        habilidades.Add(new Shield());
        habilidades.Add(new Habilidad1());
        // foreach (ITestAbilities a in habilidades)
        // {
        //     print(a.Nombre);
        // }
    }

    // Update is called once per frame
    void Update()
    {
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

    public void RecibirAtaque(TestBolsimon origen)
    {
        vida -= dano;
        if (vida <= 0)
        {
            gameObject.SetActive(false);
        }
        TestGM.Instance.ActualizarVida(this, vida);
    }

    public void SetTurn(bool value)
    {
        target = null;
        currentTurn = value;
    }

    public void Attack()
    {
        if (currentTurn && target != null)
        {
            //TestGM.Instance.Hola();
            print($"{this} + {habilidades.FindIndex(h => h.Nombre == "Ataque")} + {target}");
            TestGM.Instance.AddAction(this, habilidades.FindIndex(h => h.Nombre == "Ataque"), target);
        }
    }
}
