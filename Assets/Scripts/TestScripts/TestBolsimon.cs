using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBolsimon : MonoBehaviour
{

    public string Name;
    public TestBolsimon target;

    public List<ITestAbilities> habilidades = new List<ITestAbilities>();
    // Start is called before the first frame update
    void Start()
    {
        habilidades.Add(new Habilidad1());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            habilidades[0].Ejecutar(this, target);
        }
    }

    public void SetTarget(TestBolsimon target1)
    {
        target = target1;
    }

    public void TestRecibir(TestBolsimon origen, string nombreHabilidad)
    {
        print($"{origen.Name} ha usado {nombreHabilidad} on {Name}");
    }
}
