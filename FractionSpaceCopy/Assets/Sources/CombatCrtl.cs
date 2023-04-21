using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CombatCrtl : MonoBehaviour
{
    public int EnemyN = 2, PlayerN = 1;
    public int EnemySelect, PlayerSelect;
    public GameObject enemies, players;
    public DateTime fechaInicio;
    public DateTime fechaFin;
    public Partida partida;
    bool turn = true;
    bool final = false;


    public void Start()
    {
        fechaInicio = DateTime.Now;
        charachter stats = players.transform.GetChild(PlayerSelect).GetComponent<charachter>();
        stats.Select(true);
    }

    public void Update()
    {
        if (EnemyN >= 0)
        {
            enemies.transform.GetChild(EnemySelect).GetComponent<charachter>().Select(false);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                EnemySelect--;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                EnemySelect++;
            EnemySelect = Mathf.Clamp(EnemySelect, 0, EnemyN);
            enemies.transform.GetChild(EnemySelect).GetComponent<charachter>().Select(true);
        }


        if((EnemyN < 0) && (!final))
        {
            fechaFin = DateTime.Now;
            final = true;
            Debug.Log("Final del nivel");

            StartCoroutine(GetPlayerID());
        }
    }

    public void Atack()
    {
        if (turn && PlayerN >= 0)
        {
            charachter ch = players.transform.GetChild (PlayerSelect).GetComponent<charachter>();
            ch.Atack();

            if (PlayerSelect == PlayerN)
            {
                PlayerSelect = 0;
                turn = false;
                StartCoroutine(AtackE());
            }
            else PlayerSelect++;

            ch.Select(false);
            ch = players.transform.GetChild(PlayerSelect).GetComponent<charachter>();
            ch.Select(true);
        }
    }

    IEnumerator AtackE()
    {
        if (EnemyN >= 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            for (int i = 0; i <= EnemyN; i++)
            {
                enemies.transform.GetChild(i).GetComponent<charachter>().Atack();   
                yield return new WaitForSecondsRealtime(1f);
            }
            turn = true;
        }
    }

    IEnumerator GetPlayerID()
    {
        Debug.Log("Dentro de GetPlayerID");

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

    IEnumerator SendProgress(int userID)
    {
        partida.fecha_inicio = fechaInicio.ToString("yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture);
        partida.fecha_fin = fechaFin.ToString("yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture);
        partida.puntaje = 100;
        partida.nivel = 2;
        partida.usuario = userID;

        // Serializamos
        string message = JsonUtility.ToJson(partida);
        Debug.Log(message);

        // Simulamos formulario Web
        WWWForm form = new WWWForm();
        form.AddField("partida", message);

        using (UnityWebRequest www = UnityWebRequest.Post("http://20.198.1.48:8080/apipartidasunity", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
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
