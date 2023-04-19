using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ContarTiempo : MonoBehaviour
{
    public TMP_Text tmp_tiempo;
    public float tiempoTranscurrido;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DateTime.Now.ToString());
        tiempoTranscurrido = 0;
    }

    void OnGUI()
    {
        tmp_tiempo.text = (tiempoTranscurrido).ToString("f2");
    }

    // Update is called once per frame
    void Update()
    {
        tiempoTranscurrido = Time.realtimeSinceStartup;
    }
}