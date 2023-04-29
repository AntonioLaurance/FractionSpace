using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class final : MonoBehaviour
{
    private string tempPuntos = "puntaje";
    private int puntos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadData(){
        puntos = PlayerPrefs.GetInt(tempPuntos, 0);
    }

    private void SaveData(){
        PlayerPrefs.SetInt(tempPuntos, puntos);
    }

    private void Awake(){
        LoadData();
        Debug.Log("Puntos guardados: " + puntos);
    }
}
