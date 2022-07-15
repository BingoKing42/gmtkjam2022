using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public string unitName;

    public int damage;

    public int maxHP;
    public int currentHP;

    public int d4s;
    public int d8s;
    public int d10s;
    public int d12s;
    public int d20s;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int heal)
    {
        currentHP += heal;

        if(currentHP > maxHP)
            currentHP = maxHP;
    }
}
