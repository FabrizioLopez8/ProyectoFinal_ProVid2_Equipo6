using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public interface ITestAbilities
{
    string Nombre { get; }
    void Ejecutar(TestBolsimon origen, TestBolsimon objetivo);
}


public class Habilidad1 : ITestAbilities
{
    public string Nombre => "Habilidad1";

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        objetivo.TestRecibir(origen, this.Nombre);
    }

}