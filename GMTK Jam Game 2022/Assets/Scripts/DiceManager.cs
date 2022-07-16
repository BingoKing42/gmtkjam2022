using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DiceManager : MonoBehaviour
{
    public TextMeshProUGUI d4;
    public TextMeshProUGUI d6;
    public TextMeshProUGUI d8;
    public TextMeshProUGUI d10;
    public TextMeshProUGUI d12;
    public TextMeshProUGUI d20;

    private int d4roll;
    private int d6roll;
    private int d8roll;
    private int d10roll;
    private int d12roll;
    private int d20roll;

    private int damage_num;
    private int heal_num;
    private string effects_text;

    void Start()
    {
        d4.text = "4";
        d6.text = "6";
        d8.text = "8";
        d10.text = "10";
        d12.text = "12";
        d20.text = "20";
    }

    public int[] RollDice()
    {
        d4roll = Random.Range(1, 5);
        d6roll = Random.Range(1, 7);
        d8roll = Random.Range(1, 9);
        d10roll = Random.Range(1, 11);
        d12roll = Random.Range(1, 13);
        d20roll = Random.Range(1, 21);

        d4.text = d4roll.ToString();
        d6.text = d6roll.ToString();
        d8.text = d8roll.ToString();
        d10.text = d10roll.ToString();
        d12.text = d12roll.ToString();
        d20.text = d20roll.ToString();

        return new int[] { d4roll, d6roll, d8roll, d10roll, d12roll, d20roll };
    }


    public void ScorePicks(int num1, int num2, int num3, out int damage, out int heal, out string effects)
    {

        damage_num = (num1 + num2 + num3) / 3;
        heal_num = 0;
        effects_text = "";

        if (num1 == num2 && num2 == num3)
        {
            effects_text = "Triple";
            damage_num *= 3;
        }
        else if (num1 == num2 || num2 == num3 || num1 == num3)
        {
            effects_text = "Double";
            damage_num *= 2;
        }

        if (num1 % 2 == 0 && num2 % 2 == 0 && num3 % 2 == 0)
        {
            effects_text += "\nEvens";
            damage_num += 2;
        }
        else if (num1 % 2 == 1 && num2 % 2 == 1 && num3 % 2 == 1)
        {
            effects_text += "\nOdds";
            heal_num += 2;
        }

        damage = damage_num;
        heal = heal_num;
        effects = effects_text;
    }
}
