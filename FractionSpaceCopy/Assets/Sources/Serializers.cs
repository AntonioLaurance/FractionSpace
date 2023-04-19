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
    public int numerador1;
    public int denominador1;
    public int numerador2;
    public int denominador2;

    // Respuesta
    public int num;
    public int den;
}

[System.Serializable]

public class Result
{
    public int num;
    public int den;
    public bool correcto;
    public int devpor;
    public int devval;
}
