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

// Para enviar al servidor para verificar
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

public class Question
{
    public string texto;
    public string operacion;

    public int num1;
    public int den1;
    public int num2;
    public int den2;

    public int puntaje;
}

[System.Serializable]

public class Result
{
    // Respuesta
    public int num;
    public int den;

    public bool correcto;

    // Desviaciones
    public float devpor;
    public float devval;
}
