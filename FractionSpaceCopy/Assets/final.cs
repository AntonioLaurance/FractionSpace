using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class final : MonoBehaviour
{
    private int puntos_uno;
    private int puntos_dos;
    private int puntos_tres;
    private int puntos_totales;
    public TMP_Text puntaje;
    // Start is called before the first frame update
    void Start()
    {
        puntos_uno = Funcionamiento.puntos;
        puntos_tres = CombatManager.puntos;
        puntos_totales = Inventory.puntos;
        Debug.Log("Puntos totales: " + puntos_totales);

        puntaje.text = puntos_totales.ToString();
    }

    
}
