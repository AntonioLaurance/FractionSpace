using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour, IHasChanged{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;
    public Exercise suma;
    public Question pregunta = new Question();
    public Partida partida;
    DateTime dateinit;
    DateTime datefin;

    void Start(){
        // Fecha de inicio
        dateinit = DateTime.Now;

        HasChanged();
    }
    public void CambiaAEscena()
    {
        SceneManager.LoadScene("Estadisticas");
    }
    /*
     * Orden de los operadores para envio al servidor
     * slot0 -> numerador1
     * slot3 -> denominador1
     * slot1 -> numerador2
     * slot4 -> denoninador2
     * slot2 -> num
     * slot5 -> den
     * 
     * slot [element]
     * imagen [item]
    */
    public void HasChanged () {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (Transform slotTansform in slots){
            GameObject item = slotTansform.GetComponent<SLOT>().item;
            string element = slotTansform.ToString();

            // Debug.Log(slotTansform.name);
            if (item){
                builder.Append(item.name);
                builder.Append(" - ");
             
                Debug.Log(element);
                Debug.Log("Número: " + item.name);

                // Asignamos a un elemento de nuestra clase

                // Comprobamos si las 6 fichas ya están puestas
                if (builder.ToString().Length == 24)
                {
                    Debug.Log("Enviar operación al servidor.");
                    // Debug.Log(number);

                    // Obtenemos elementos de la operación de nuestra GUI
                    GameObject slot0 = GameObject.Find("Canvas/Panel/inventario(1)/slot0");
                    suma.numerador1 = System.Int32.Parse(slot0.transform.GetChild(0).name);

                    GameObject slot1 = GameObject.Find("Canvas/Panel/inventario(1)/slot1");
                    suma.numerador2 = System.Int32.Parse(slot1.transform.GetChild(0).name);
                    // pregunta.num2 = System.Int32.Parse(slot1.transform.GetChild(0).name);

                    GameObject slot2 = GameObject.Find("Canvas/Panel/inventario(1)/slot2");
                    suma.num = System.Int32.Parse(slot2.transform.GetChild(0).name);

                    GameObject slot3 = GameObject.Find("Canvas/Panel/inventario(1)/slot3");
                    suma.denominador1 = System.Int32.Parse(slot3.transform.GetChild(0).name);
                    // pregunta.den1 = System.Int32.Parse(slot3.transform.GetChild(0).name);

                    GameObject slot4 = GameObject.Find("Canvas/Panel/inventario(1)/slot4");
                    suma.denominador2 = System.Int32.Parse(slot4.transform.GetChild(0).name);
                    // pregunta.den2 = System.Int32.Parse(slot4.transform.GetChild(0).name);

                    GameObject slot5 = GameObject.Find("Canvas/Panel/inventario(1)/slot5");
                    suma.den = System.Int32.Parse(slot5.transform.GetChild(0).name);

                    // Serializamos
                    string message = JsonUtility.ToJson(suma);
                    Debug.Log(message);

                    // Enviamos datos al servidor
                    StartCoroutine(SendToServer(message));
                }
            }
        }
        inventoryText.text = builder.ToString();
    }

    IEnumerator SendToServer(string json)
    {
        // Simulamos un formulario web
        WWWForm form = new WWWForm();
        form.AddField("exercise", json);

        // Enviamos al servidor para verificar
        using (UnityWebRequest www = UnityWebRequest.Post("http://20.198.1.48:8080/suma", form))
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
                string txt = www.downloadHandler.text;
                Debug.Log(txt);     // raw text

                // Deserialización
                Result answer = JsonUtility.FromJson<Result>(txt);

                // Imprimimos resultados obtenidos desde el servidor
                Debug.Log("Respuesta correcta: " + answer.num + "/" + answer.den);
                Debug.Log("¿Correcto?: " + answer.correcto);
                Debug.Log("Desviación: " + answer.devval);
                Debug.Log("Desviación porcentual: " + answer.devpor + "%");

                if (answer.devpor < 100)
                {
                    pregunta.puntaje = Convert.ToInt32(100 - answer.devpor);
                }
                else
                {
                    pregunta.puntaje = 0;
                }

                // string question = JsonUtility.ToJson(pregunta);
                StartCoroutine(SendQuestion());
            }
        }
    }

    IEnumerator SendQuestion()
    { 
        pregunta.num1 = suma.numerador1;
        pregunta.num2 = suma.numerador2;
        pregunta.den1 = suma.denominador1;
        pregunta.den2 = suma.denominador2;

        pregunta.texto = "";
        pregunta.operacion = "+";

        string question = JsonUtility.ToJson(pregunta);
        
        Debug.Log("Estamos dentro de SendQuestion.");
        Debug.Log(question);

        // Simulamos un formulario web
        WWWForm form = new WWWForm();
        form.AddField("pregunta", question);

        // Enviamos para la base de datos
        using (UnityWebRequest www = UnityWebRequest.Post("http://20.198.1.48:8080/apipreguntasunity", form))
        {
            yield return www.SendWebRequest();

            // Checamos si hay error
            if(www.result != UnityWebRequest.Result.Success)
            {
                // Imprimimos mensaje de error
                Debug.Log(www.error);
                EditorUtility.DisplayDialog("Error de conexión", www.error, "Aceptar");
            }
            else
            {
                string txt = www.downloadHandler.text;
                Debug.Log(txt);         // raw text

                // Fecha de fin del juego
                datefin = DateTime.Now;
                Debug.Log(dateinit);
                Debug.Log(datefin);

                StartCoroutine(GetUserID());
                Invoke("CambiaAEscena", 5f);
            }
        }
    }

    
    IEnumerator GetUserID()
    {
        Debug.Log("Estamos dentro de GetIDPlayer");

        // Obtenemos datos a enviar
        GameObject user = GameObject.Find("User");
        UserData ud = user.GetComponent<UserData>();

        // Serializamos el objeto Player
        string message = JsonUtility.ToJson(ud.player);

        // Simulamos formulario web
        WWWForm form = new WWWForm();
        form.AddField("jugador", message);

        // Método POST
        using (UnityWebRequest www = UnityWebRequest.Post("http://20.198.1.48:8080/apiusuarioidunity", form))
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
                // Obtengo texto que viene del servidor
                string txt = www.downloadHandler.text;

                // Imprimimos ID del jugador
                Debug.Log("Jugador ID: " + txt);

                int userID = System.Int32.Parse(txt);
                StartCoroutine(SendProgress(userID));
            }
        }
    }


    // Función que envía la partida al servidor
    IEnumerator SendProgress(int userID)
    {
        // Ver como sacar las fechas
        partida.fecha_inicio = dateinit.ToString("yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture);
        partida.fecha_fin = datefin.ToString("yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture);
        partida.puntaje = pregunta.puntaje;
        partida.nivel = 3;
        partida.usuario = userID;

        // Serializamos
        string message = JsonUtility.ToJson(partida);
        Debug.Log(message);

        WWWForm form = new WWWForm();
        form.AddField("partida", message);

        using (UnityWebRequest www = UnityWebRequest.Post("http://20.198.1.48:8080/apipartidasunity", form))
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                // Imprimimos mensaje de error
                Debug.Log(www.error);
            }
            else
            {
                // Obtenemos texto que viene del servidor
                string txt = www.downloadHandler.text;

                // Imprimimos texto
                Debug.Log(txt);
            } 
        }
    } 
}

namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasChanged();
    }
}
