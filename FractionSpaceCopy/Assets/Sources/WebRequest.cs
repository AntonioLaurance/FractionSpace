using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WebRequest : MonoBehaviour
{
    // public string username;
    // public int numList;

    // Para hacer nuestro código parametrizable en la UI de Unity
    public TMP_InputField tmp_grupo;
    public TMP_InputField tmp_lista;


    IEnumerator LoginWeb()
    {
        string grupo = tmp_grupo.text;
        string lista = tmp_lista.text;

    }

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(Upload());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
