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


    public void LevelClick()
    {
        // Obtenemos el texto de lo ingresado en nuestra interfaz gráfica (GUI)
        string grupo = tmp_grupo.text;
        string lista = tmp_lista.text;

        // 1. Obtener referencia al GameObject vacío
        GameObject persist = GameObject.Find("User");

        // 2. Obtener una referencia al UserData dentro de User
        UserData ud = persist.GetComponent<UserData>();

        // Definimos los atributos del jugador
        ud.player.grupo = grupo;
        ud.player.numList = System.Int32.Parse(lista);

        // Serializamos
        string message = JsonUtility.ToJson(ud.player);

        // Enviamos objeto serializado al servidor 
        StartCoroutine(LoginWeb(message));

        // Desplegamos el nivel 1
        SceneManager.LoadScene("Nivel 1");
    }


    public void Auth()
    {
        // Obtenemos el texto de lo ingresado en nuestra interfaz gráfica (GUI)
        string grupo = tmp_grupo.text;
        int lista = System.Int32.Parse(tmp_lista.text);

        // StartCoroutine(LoginWeb(grupo, lista));
    }

    IEnumerator LoginWeb(string data)
    {
        Debug.Log("Dentro de LoginWeb");

        // Establecemos encabezado
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        // Simulamos un formulario web
        WWWForm form = new WWWForm();

        // Agregamos atributos a nuestro JSON
        form.AddField("player", data);
        // form.AddField("group", grupo);

        // Imprimimos formulario que enviamos
        Debug.Log(form.data);
        Debug.Log(data);

        // Método POST
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/auth", form))
        {
            yield return www.SendWebRequest();

            // Checamos si hay error
            if (www.result != UnityWebRequest.Result.Success)
            {
                // Imprimimos mensaje de error
                Debug.Log(www.error);
            }
            else
            {
                string txt = www.downloadHandler.text;
                Debug.Log(txt);         // raw text

                // Deserialización
                Verify u = JsonUtility.FromJson<Verify>(txt);

                // Imprimimos datos de lo que recibimos
                Debug.Log(u.valid);
                Debug.Log(u.token);

                // Cambiamos de escena
                LevelClick();
            }
        }
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
