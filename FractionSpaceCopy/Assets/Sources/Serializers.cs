using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Player
{
    public string group;
    public int numList;
}

[System.Serializable]

public class Verify
{
    public bool valid;
    public string token;
}

[System.Serializable]

public class Exercise
{
    // Operadores
    public int num1;
    public int den1;
    public int num2;
    public int den2;

    // Respuesta
    public int num;
    public int den;
}
