using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factorial : MonoBehaviour
{

    [SerializeField] private ulong number;
    [SerializeField] private ulong permutation;

    void Start()
    {
        Factorise(number);
        Arrange(number, permutation);
        Combine(number, permutation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private ulong Factorise(ulong n)
    {
        Debug.Log("Factorising " + n);
        ulong r = 1;
        for (ulong i = 0; i < n; i++)
        {
            r *= n - i;
        }
        Debug.Log("Factorised  to " + r);
        return r;
    }

    private ulong Arrange(ulong n, ulong p)
    {
        Debug.Log("Aranging " + n + " and " + p);
        ulong r = Factorise(n)/n>p?Factorise((n-p)):Factorise(n);
        Debug.Log("Arranged to " + r);
        return r;
    }
    private ulong Combine(ulong n, ulong p)
    {
        Debug.Log("Combining " + n + " and " + p);
        ulong r = Arrange(p,n)/Factorise(p);
        Debug.Log("Combined to " + r);
        return r;
    }
}
