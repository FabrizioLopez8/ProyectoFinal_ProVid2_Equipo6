using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Bolsimon
{
    public static string Name(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return "Fuego";
            case ElementType.Water:
                return "Agua";
            case ElementType.Plant:
                return "Planta";
            default:
                return null;
        }
    }

    public static float BaseSpeed(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return 300;
            case ElementType.Water:
                return 350;
            case ElementType.Plant:
                return 400;
            default:
                return 0;
        }
    }

    public static float BaseDamage(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return 175;
            case ElementType.Water:
                return 125;
            case ElementType.Plant:
                return 150;
            default:
                return 0;
        }
    }

    public static int BaseHealth(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return 400;
            case ElementType.Water:
                return 500;
            case ElementType.Plant:
                return 450;
            default:
                return 0;
        }
    }

    public static List<Abilities> Abilities(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return new List<Abilities>
                {
                    new Attack(),
                    new Shield(),
                    new MuroLLamas()
                };
            case ElementType.Water:
                return new List<Abilities>
                {
                    new Attack(),
                    new Shield(),
                    new Lluvia()
                };
            case ElementType.Plant:
                return new List<Abilities>
                {
                    new Attack(),
                    new Shield(),
                    new Fotosintesis()
                };
            default:
                return null;
        }
    }

    public static Color Colors(ElementType elementType)
    {
         switch (elementType)
        {
            case ElementType.Fire:
                return Color.red;
            case ElementType.Water:
                return Color.blue;
            case ElementType.Plant:
                return Color.green;
            default:
                return Color.white;
        }
    }

    public static ElementType GetElementTypeOnColor(Color color)
    {

        if (color == Color.red) return ElementType.Fire;
        else if (color == Color.blue) return ElementType.Water;
        else if (color ==Color.green) return ElementType.Plant;
        else return 0;
                
    }

    public static IAStrategy GetIAStrategy(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                return new AggressiveIA();
            case ElementType.Water:
                return new DebuffingIA();
            case ElementType.Plant:
                return new DefensiveIA();
            default:
                return null;
        }
    }
}
