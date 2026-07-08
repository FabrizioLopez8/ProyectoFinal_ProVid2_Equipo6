using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


//Ojala haber descubierto antes los Scriptable objects.
public interface ITestAbilities
{
    string Nombre { get; }
    string Tipo { get; }
    TargetType targetType {get; }
    void Ejecutar(TestBolsimon origen, TestBolsimon objetivo);
}


public class MuroLLamas : ITestAbilities
{
    public string Nombre => "MuroLLamas";
    public string Tipo => "Fuego";
    public TargetType targetType => TargetType.AllExceptSelf;

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        objetivo.RecibirAtaque(origen, ((int)(origen.dano * 0.75)));
    }

}

public class Lluvia : ITestAbilities
{
    public string Nombre => "Lluvia";
    public string Tipo => "Agua";
    public int duración = 3;
    public TargetType targetType => TargetType.AllExceptSelf;

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        objetivo.AplicarEfectoNegativo(this.Nombre, this.duración, origen);
    }
}

public class Fotosintesis : ITestAbilities
{
    public string Nombre => "Fotosintesis";
    public string Tipo => "Planta";
    public TargetType targetType => TargetType.SingleTarget;

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        origen.Curarse();
    }
}

public class Attack : ITestAbilities
{
    public string Nombre => "Ataque";
    public string Tipo => "Normal";
    public TargetType targetType => TargetType.SingleTarget;

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        objetivo.RecibirAtaque(origen, origen.dano);
    }

}

public class Shield : ITestAbilities
{
    public string Nombre => "Escudo";
    public string Tipo => "Normal";
    public TargetType targetType => TargetType.SingleTarget;

    public void Ejecutar(TestBolsimon origen, TestBolsimon objetivo)
    {
        objetivo.UseShield();
    }

    // public int Algo()
    // {
    //     return 0;
    // }


}