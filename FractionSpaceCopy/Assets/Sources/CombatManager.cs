using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;


public enum CombatStatus
{
    WAITING_FOR_FIGHTER,
    FIGHTER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN
}

public class CombatManager : MonoBehaviour
{
    public Fighter[] fighters;
    private int fighterIndex;

    private bool isCombatActive;

    private CombatStatus combatStatus;

    private Skill currentFighterSkill;
    public DateTime fechaInicio;
    public DateTime fechaFin;
    public float timeStart;
    public TMP_Text puntaje;
    private int puntos;
    bool timerActive = true;
    private string tempPuntos = "puntaje";
    public Partida partida;




    void Start()
    {
        fechaInicio = DateTime.Now;
        LogPanel.Write("Batalla Iniciada!");

        foreach (var fgtr in this.fighters)
        {
            fgtr.combatManager = this;
        }

        this.combatStatus = CombatStatus.NEXT_TURN;

        this.fighterIndex = -1;
        this.isCombatActive = true;
        StartCoroutine(this.CombatLoop());
        puntaje.text = ("");
    }
    private void LoadData()
    {
        puntos = PlayerPrefs.GetInt(tempPuntos, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(tempPuntos, puntos);
    }

    private void Awake()
    {
        LoadData();
        Debug.Log("Puntos guardados: " + puntos);
    }

    IEnumerator CombatLoop()
    {
        while (this.isCombatActive)
        {
            switch (this.combatStatus)
            {
                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;

                case CombatStatus.FIGHTER_ACTION:
                    LogPanel.Write($"{this.fighters[this.fighterIndex].idName} ha usado {currentFighterSkill.skillName}.");

                    yield return null;

                    // Executing fighter skill
                    currentFighterSkill.Run();

                    // Wait for fighter skill animation
                    yield return new WaitForSeconds(currentFighterSkill.animationDuration);
                    this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;

                    currentFighterSkill = null;
                    break;

                case CombatStatus.CHECK_FOR_VICTORY:
                    foreach (var fgtr in this.fighters)
                    {
                        if (fgtr.isAlive == false)
                        {
                            this.isCombatActive = false;

                            LogPanel.Write("Ganaste!");

                            fechaFin = DateTime.Now;
                            Debug.Log("Final del nivel");
                            Puntuacion();
                            StartCoroutine(GetPlayerID());
                            Invoke("CambiaAEscena", 5f);

                        }
                        else
                        {
                            this.combatStatus = CombatStatus.NEXT_TURN;
                        }
                    }
                    yield return null;
                    break;
                case CombatStatus.NEXT_TURN:
                    yield return new WaitForSeconds(1f);
                    this.fighterIndex = (this.fighterIndex + 1) % this.fighters.Length;

                    var currentTurn = this.fighters[this.fighterIndex];

                    LogPanel.Write($"Es el turno de {currentTurn.idName}.");
                    currentTurn.InitTurn();

                    this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;

                    break;
            }
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
        if (timeStart < 40)
        {
            puntos = puntos + 100;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 45)
        {
            puntos = puntos + 95;
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
            puntos = puntos + 85;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 60)
        {
            puntos = puntos + 80;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 65)
        {
            puntos = puntos + 75;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 70)
        {
            puntos = puntos + 70;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 75)
        {
            puntos = puntos + 65;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 80)
        {
            puntos = puntos + 60;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 85)
        {
            puntos = puntos + 65;
            SaveData();
            Debug.Log(puntos);
        }
        else if (timeStart < 90)
        {
            puntos = puntos + 55;
            SaveData();
            Debug.Log(puntos);
        }
        else
        {
            puntos = puntos + 50;
            SaveData();
            Debug.Log(puntos);
        }
    }


    public Fighter GetOpposingFighter()
    {
        if (this.fighterIndex == 0)
        {
            return this.fighters[1];
        }
        else
        {
            return this.fighters[0];
        }
    }

    public void OnFighterSkill(Skill skill)
    {
        this.currentFighterSkill = skill;
        this.combatStatus = CombatStatus.FIGHTER_ACTION;
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
