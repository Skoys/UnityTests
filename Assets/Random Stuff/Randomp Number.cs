using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{

    [SerializeField] private int diceNbr = 2;
    [SerializeField] private int diceMax = 6;
    [SerializeField] private int throwNbr = 2;
    [SerializeField] private List<int> rolledNumber = new List<int>();
    [SerializeField] private List<int> doubleNumber = new List<int>();
    [SerializeField] private List<float> doubleNumberStats = new List<float>();

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        System.Random rand = new System.Random();
        rolledNumber.Clear();
        doubleNumber.Clear();
        doubleNumberStats.Clear();
        for (int i = 0; i < diceNbr * 6 + 1; i++)
        {
            rolledNumber.Add(0);
            doubleNumber.Add(0);
            doubleNumberStats.Add(0);
        }

        int lastNbr = 0;
        for (int i = 0; i < throwNbr; i++)
        {
            int nbr = 0;
            for (int j = 0; j < diceNbr; j++)
            {
                nbr += rand.Next(1,diceMax+1);
            }
            rolledNumber[nbr] += 1;

            if (nbr == lastNbr)
            {
                doubleNumber[nbr] += 1;
            }
            else lastNbr = nbr;
        }

        int totNbr = 0;
        for (int i = 0; i < doubleNumber.Count; i++)
        {
            doubleNumberStats[i] = (100 * (float)doubleNumber[i]) / throwNbr;
            totNbr += doubleNumber[i];
        }
        doubleNumberStats[0] = (100 * (float)totNbr) / throwNbr;

        Debug.Log("Finished");
    }
}
