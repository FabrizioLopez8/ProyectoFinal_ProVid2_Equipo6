using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAStrategy
{
    string Name {get; }
    public void ChooseAction(BolsimonController origin);
}

public class AggressiveIA : IAStrategy
{
    public string Name => "AgressiveIA";
    public void ChooseAction(BolsimonController origin)
    {
        List<BolsimonController> otherPlayers = new List<BolsimonController>();
        Debug.Log($"{otherPlayers}");
        foreach (BolsimonController b in TestGM.Instance.playersBolsimon)
        {
            if(b != origin) otherPlayers.Add(b);
        }

        int randomAbility = Random.Range(0, 3);
        if (origin.abilities[randomAbility].Name == "Escudo")
        {
            int randomModifier = Random.Range(0, 2);
            if (randomModifier == 0) randomAbility -= 1;
            else randomAbility +=1;
        }

        if (origin.abilities[randomAbility].Tipo != ElementType.none) origin.Ability();
        else
        {
            int randomTarget = Random.Range(0, 2);
            origin.SetTarget(otherPlayers[randomTarget]);
            origin.Attack();
        }

    }

}

public class DefensiveIA : IAStrategy
{
    public string Name => "DefensiveIA";

    public void ChooseAction(BolsimonController origin)
    {
        List<BolsimonController> otherPlayers = new List<BolsimonController>();
        Debug.Log($"{otherPlayers}");
        foreach (BolsimonController b in TestGM.Instance.playersBolsimon)
        {
            if(b != origin) otherPlayers.Add(b);
        }

        int randomAbility = Random.Range(0, 3);
         Debug.Log($"{randomAbility}");
        if (origin.Health <= (int)(origin.baseHealth * 0.6) && (randomAbility != 1 || randomAbility != 2))
        {
            int extraRandomChanceHeal = Random.Range(0,2);
            if (extraRandomChanceHeal == 1) origin.Ability();
        }
        else
        {
             Debug.Log($"{origin.abilities[randomAbility]}");
            if (origin.abilities[randomAbility].Name == "Escudo") origin.Shield();
            else
            {
                int randomTarget = Random.Range(0, 2);
                origin.SetTarget(otherPlayers[randomTarget]);
                origin.Attack();
            }
        }
    }
}

public class DebuffingIA : IAStrategy
{
    public string Name => "DebuffingIA";

    public void ChooseAction(BolsimonController origin)
    {
        List<BolsimonController> otherPlayers = new List<BolsimonController>();
        Debug.Log($"{otherPlayers}");
        foreach (BolsimonController b in TestGM.Instance.playersBolsimon)
        {
            if(b != origin) otherPlayers.Add(b);
        }

        if (otherPlayers[0].debuffDuration <= 0 || otherPlayers[1].debuffDuration <= 0) origin.Ability();
        else
        {
            int randomAbility = Random.Range(0, 2);
            if (origin.abilities[randomAbility].Name == "Escudo") origin.Shield();
            else
            {
                int randomTarget = Random.Range(0, 2);
                origin.SetTarget(otherPlayers[randomTarget]);
                origin.Attack();
            }
        }
    }
}