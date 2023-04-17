using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class WebRequest : MonoBehaviour
{
    // public string username;
    // public int numList;

    // Para hacer nuestro código parametrizable en la UI de Unity
    public TMP_InputField tmp_grupo;
    public TMP_InputField tmp_lista;

    public void auth()
    {
        // Obtenemos el texto de lo ingresado en nuestra interfaz gráfica (GUI)
        string grupo = tmp_grupo.text;
        string lista = tmp_lista.text;

        // 1. Obtener referencia al GameObject vacío
        GameObject persist = GameObject.Find("User");

        // 2. Obtener una referencia al UserData dentro de User
        UserData ud = persist.GetComponent<UserData>();

        ud.player.grupo = char.Parse(grupo);
        ud.player.numList = System.Int32.Parse(lista);

        SceneManager.LoadScene("Nivel 1");
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

    void Upload()
    {

    }
}
