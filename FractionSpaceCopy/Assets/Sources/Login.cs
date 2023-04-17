using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    // Para hacer nuestro código parámetrizable en la UI de Unity
    public TMP_InputField tmp_grupo;
    public TMP_InputField tmp_lista;

    // Se activa cada vez que se oprime el botón
    public void LoginWeb()
    {
        // Obtenemos el texto de lo ingresado en nuestra interfaz gráfica (GUI)
        string grupo = tmp_grupo.text;
        string lista = tmp_lista.text;

        // 1. Obtener referencia al GameObject vacío
        GameObject persist = GameObject.Find("User");

        // 2. Obtener referencia al UserData dentro de User
        UserData ud = persist.GetComponent<UserData>();

        // Enviamos el texto obtenido al servidor
        StartCoroutine(SendToServer(lista, grupo));
    }

    IEnumerator SendToServer(string lista, string grupo)
    {
        // Simulamos un formulario web
        WWWForm form = new WWWForm();

        form.AddField("numList", lista);
        form.AddField("group", grupo);

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
                Player user = JsonUtility.FromJson<Player>(txt);

                // Camvbiamos de nivel
                SceneManager.LoadScene("Nivel 1");

            }
        }
    }
}
