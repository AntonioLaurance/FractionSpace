using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEditor;
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
        ud.player.group = grupo;
        ud.player.numList = System.Int32.Parse(lista);

        // Serializamos
        string message = JsonUtility.ToJson(ud.player);

        // Enviamos objeto serializado al servidor 
        StartCoroutine(LoginWeb(message));
    }


    public void Auth(bool valid, string token)
    {
        // 1. Obtener referencia al game object vacío
        GameObject persist = GameObject.Find("User");

        // 2. Obtener una referencia al UserData dentro de User
        UserData ud = persist.GetComponent<UserData>();

        // Definimos los atributos de verificación
        ud.verify.valid = valid;
        ud.verify.token = token;

        if(token != "")
        {
            // Desplegamos el nivel 1
            SceneManager.LoadScene("Nivel 1");
        }
        else
        {
            // Mandamos error
            Debug.Log("Usuario no válido.");
            EditorUtility.DisplayDialog("Credenciales incorrectas", "El usuario no es válido","Aceptar");
        }
    }

    IEnumerator LoginWeb(string data)
    {
        // Simulamos un formulario web
        WWWForm form = new WWWForm();
        form.AddField("player", data);

        // Imprimimos formulario que enviamos
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
                EditorUtility.DisplayDialog("Error de conexión", www.error, "Aceptar");
            }
            else
            {
                Debug.Log("Envio de información de usuario exitosa");
                string txt = www.downloadHandler.text;
                Debug.Log(txt);         // raw text

                // Deserialización
                Verify u = JsonUtility.FromJson<Verify>(txt);

                // Imprimimos datos de lo que recibimos
                Debug.Log(u.valid);
                Debug.Log(u.token);

                Auth(u.valid, u.token);
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
