using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DiceManager : MonoBehaviour
{   
    public GameObject d4;
    public GameObject d6;
    public GameObject d8;
    public GameObject d10;
    public GameObject d12;
    public GameObject d20;

    private int d4roll;
    private int d6roll;
    private int d8roll;
    private int d10roll;
    private int d12roll;
    private int d20roll;

    private int damage_num;
    private int heal_num;
    private string effects_text;

    public int[] RollDice()
    {
        d4roll = Random.Range(1, 5);
        d4.GetComponent<DragDrop>().updateValue(d4roll);

        d6roll = Random.Range(1, 7);
        d6.GetComponent<DragDrop>().updateValue(d6roll);

        d8roll = Random.Range(1, 9);
        d8.GetComponent<DragDrop>().updateValue(d8roll);

        d10roll = Random.Range(1, 11);
        d10.GetComponent<DragDrop>().updateValue(d10roll);

        d12roll = Random.Range(1, 13);
        d12.GetComponent<DragDrop>().updateValue(d12roll);

        d20roll = Random.Range(1, 21);
        d20.GetComponent<DragDrop>().updateValue(d20roll);

        return new int[] { d4roll, d6roll, d8roll, d10roll, d12roll, d20roll };
    }

    public void ScorePicks(GameObject die1, GameObject die2, GameObject die3, out int damage, out int heal, out string effects)
    {
        DragDrop die1Info = die1.GetComponent<DragDrop>();
        DragDrop die2Info = die2.GetComponent<DragDrop>();
        DragDrop die3Info = die3.GetComponent<DragDrop>();

        int num1 = die1Info.dieValue;
        int num2 = die2Info.dieValue;
        int num3 = die3Info.dieValue;

        int num1max = die1Info.dieMax;
        int num2max = die2Info.dieMax;
        int num3max = die3Info.dieMax;

        damage_num = (num1 + num2 + num3) / 3;
        heal_num = 0;
        effects_text = "";

        if (num1 + 1 == num2 && num2 + 1 == num3)
        {
            effects_text += "Straight Skates";
            damage_num = num1+num2+num3;
        }

        if (num1 == num2 && num2 == num3)
        {
            effects_text = "Triple Dribble";
            //Triples stun the enemy
        }
        else if (num1 == num2 || num2 == num3 || num1 == num3)
        {
            effects_text = "Double Trouble";
            damage_num *= 2;
        }

        if (num1 % 2 == 0 && num2 % 2 == 0 && num3 % 2 == 0)
        {
            effects_text += "\nEven Stevens";
            damage_num += 5;
        }
        else if (num1 % 2 == 1 && num2 % 2 == 1 && num3 % 2 == 1)
        {
            effects_text += "\nBob's Odd Toe";
            heal_num += 5;
        }

        if (num1 + num2 == num3)
        {
            effects_text += "\nSummation Vacation";
            heal_num += 3;
            damage_num += 3;
        }

        if (num1 == num1max && num2 == num2max && num3 == num3max)
        {
            effects_text += "\nAnalytical Critical";
            heal_num += 15;
        }

        
        damage = damage_num;
        heal = heal_num;
        effects = effects_text;
    }
}
