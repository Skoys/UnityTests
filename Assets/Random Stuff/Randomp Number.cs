using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    private DiceChoice diceChoice;

    [SerializeField] private int diceNbr = 2;
    [SerializeField] private int diceMax = 6;
    [SerializeField] private int throwNbr = 2;
    [SerializeField] private List<int> rolledNumber = new List<int>();

    public enum DiceChoice
    {
        DoubleDice,
        TripleDice,
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        System.Random rand = new System.Random();
        rolledNumber.Clear();
        for (int i = 0; i < diceNbr * 6 + 1; i++)
        {
            rolledNumber.Add(0);
        }
        for (int i = 0; i < throwNbr; i++)
        {
            int nbr = 0;
            for (int j = 0; j < diceNbr; j++)
            {
                nbr += rand.Next(1,diceMax+1);
            }
            rolledNumber[nbr] += 1;
        }

        Debug.Log("Finished");
    }
}
