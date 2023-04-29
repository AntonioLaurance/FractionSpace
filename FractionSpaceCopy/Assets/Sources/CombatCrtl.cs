using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatCrtl : MonoBehaviour
{
    public int EnemyN = 2, PlayerN = 1;
    public int EnemySelect, PlayerSelect;
    public GameObject enemies, players;
    public DateTime fechaInicio;
    public Partida partida;
    public DateTime fechaFin;
    public float timeStart;
    public TMP_Text puntaje;
    private int puntos;
    bool turn = true;
    bool final = false;
    bool timerActive = true;
    private string tempPuntos = "puntaje";
    
    public void Start()
    {
        fechaInicio = DateTime.Now;
        charachter stats = players.transform.GetChild(PlayerSelect).GetComponent<charachter>();
        stats.Select(true);
        puntaje.text = ("");
        

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
            Puntuacion();
            StartCoroutine(GetPlayerID());
            Invoke("CambiaAEscena", 5f);
        }
    }

    public void CambiaAEscena()
    {
        SceneManager.LoadScene("Nivel 3");
    }

    public void Puntuacion()//Asigna los puntos obtenidos según el tiempo que se tardó en resolver el juego y los imprime
    {
        timerActive = false;
        fechaFin = DateTime.Now;
        if(timeStart < 40)
        {
            puntos  = puntos + 100;
            SaveData();
            Debug.Log(puntos);
        }
        else if(timeStart < 45)
        {
            puntos  = puntos + 95;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 50)
        {
            puntos = puntos + 90;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 55)
        {
            puntos  = puntos + 85;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 60)
        {
            puntos  = puntos + 80;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 65)
        {
            puntos  = puntos + 75;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 70)
        {
            puntos  = puntos + 70;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 75)
        {
            puntos  = puntos + 65;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 80)
        {
            puntos  = puntos + 60;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 85)
        {
            puntos  = puntos + 65;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 90)
        {
            puntos  = puntos + 55;
            SaveData();
            Debug.Log(puntos);
        }
        else
        {
            puntos  = puntos + 50;
            SaveData();
            Debug.Log(puntos);
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
        partida.puntaje = puntos;
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
