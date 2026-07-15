using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


//Ojala haber descubierto antes los Scriptable objects.
public interface Abilities
{
    string Name { get; }
    public ElementType Tipo { get; }
    TargetType targetType {get; }
    void Execute(BolsimonController origin, BolsimonController target);
}


public class MuroLLamas : Abilities
{
    public string Name => "MuroLLamas";
    public ElementType Tipo => ElementType.Fire;
    public TargetType targetType => TargetType.AllExceptSelf;

    public void Execute(BolsimonController origin, BolsimonController target)
    {
        target.ReceiveAttack(origin, (float)(origin.Damage * 0.75));
        target.DispararQuemado();
    }

}

public class Lluvia : Abilities
{
    public string Name => "Lluvia";
    public ElementType Tipo => ElementType.Water;
    public int duración = 3;
    public TargetType targetType => TargetType.AllExceptSelf;

    public void Execute(BolsimonController origin, BolsimonController target)
    {
        target.ApplyDebuffs(Name, duración, origin);
    }
}

public class Fotosintesis : Abilities
{
    public string Name => "Fotosintesis";
    public ElementType Tipo => ElementType.Plant;
    public TargetType targetType => TargetType.SingleTarget;

    public void Execute(BolsimonController origin, BolsimonController target)
    {
        origin.Heal();
        target.DispararCurado();
    }
}

public class Attack : Abilities
{
    public string Name => "Ataque";
    public ElementType Tipo => ElementType.none;
    public TargetType targetType => TargetType.SingleTarget;

    public void Execute(BolsimonController origin, BolsimonController target)
    {
        target.ReceiveAttack(origin, origin.Damage);
        target.DispararDanio();
    }

}

public class Shield : Abilities
{
    public string Name => "Escudo";
    public ElementType Tipo => ElementType.none;
    public TargetType targetType => TargetType.SingleTarget;

    public void Execute(BolsimonController origin, BolsimonController target)
    {
        target.UseShield();
    }

    // public int Algo()
    // {
    //     return 0;
    // }


}