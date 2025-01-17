using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Matrices : MonoBehaviour
{
    public int size = 3;

    [System.Serializable]
    public class matriceList
    {
        public List<float> matrix;
    }
    [Tooltip("Elements are Rows, Matrixs are Columns")]
    public List<matriceList> matrice = new List<matriceList>();
    [Tooltip("Each matrixs need to be the same size of a matrice row")]
    public List<matriceList> matriceMult = new List<matriceList>();

    public Vector3 vector3 = Vector3.zero;

    void Start()
    {
        float[,] m;
        float[,] mx;
        m = MatriceFromList(matrice);
        mx = MatriceFromList(matriceMult);
        m = MatriceXMatrice(m, mx);
        MatricePrint(m);
        m = MatriceTranspose(m);
        MatricePrint(m);
    }

    public float[,] MatriceFromList(List<matriceList> _m)
    {
        float[,] _matrice = new float[_m.Count(),_m[0].matrix.Count()];

        for (int i = 0; i < _m.Count(); i++)
        {
            for (int j = 0; j < _m[0].matrix.Count(); j++)
            {
                _matrice[i,j] = _m[i].matrix[j];
            }
        }
        return _matrice;
    }

    public float[,] MatriceIdentity(int size)
    {
        float[,] _matrice = new float[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _matrice[i, j] = i==j?1:0;
            }
        }
        return _matrice;
    }

    public void MatricePrint(float[,] _matrice)
    {
        string line = "";
        for (int i = 0; i < _matrice.GetLength(0); i++)
        {
            line = "[ ";
            for (int j = 0; j < _matrice.GetLength(1); j++)
            {
                line += _matrice[i, j] + " ";
            }
            line += "]";
            Debug.Log(line);
        }
        Debug.Log(">>>");
    }

    public float[,] MatriceXMatrice(float[,] _m1, float[,] _m2)
    {
        float[,] r = new float[_m1.GetLength(0), _m1.GetLength(1)];

        if (_m2.GetLength(0) == 1)
        {
            for (int i = 0; i < _m1.GetLength(0); i++) //m1.Rows
            {
                for (int j = 0; j < _m2.GetLength(1); j++) //m2.columns.
                {
                    r[i, j] = _m1[i,j] * _m2[0,j];
                }
            }
            return r;
        }

        for (int i = 0; i < _m1.GetLength(0); i++) //m1.Rows
        {
            for (int j = 0; j < _m2.GetLength(1); j++) //m2.columns.
            {
                float x = 0;
                for (int k = 0; k < _m1.GetLength(1); k++) //m1.columns == m2.rows.
                {
                    x += _m1[i, k] * _m2[k, j];
                }
                r[i, j] = x;
            }
        }

        return r;
    }

    public float[,] MatriceTranspose(float[,] _matrice)
    {
        float[,] r = new float[_matrice.GetLength(0), _matrice.GetLength(1)];

        for (int i = 0; i < _matrice.GetLength(0); i++)
        {
            for (int j = 0; j < _matrice.GetLength(1); j++)
            {
                r[j,i] = _matrice[i,j];
            }
        }

        return r;
    }
}
