using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITestTypes
{
    string Name {get; }
    ITestTypes IneffectiveAgainst {get; }
}

public class TypeFire : ITestTypes
{
    public string Name => "Fuego";
    public ITestTypes IneffectiveAgainst => new TypeWater();
}

public class TypeWater : ITestTypes
{
    public string Name => "Agua";
    public ITestTypes IneffectiveAgainst => new TypePlant();
}

public class TypePlant : ITestTypes
{
    public string Name => "Planta";
    public ITestTypes IneffectiveAgainst => new TypeFire();
}

// public class TestType : MonoBehaviour
// {
//     public ITestTypes Fuego = new TypeFire();
// }