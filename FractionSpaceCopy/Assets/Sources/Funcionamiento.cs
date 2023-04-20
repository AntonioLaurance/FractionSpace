using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Funcionamiento : MonoBehaviour
{
    GameObject carta; //Se crea objeto de la carta
    List<int> indexCaras = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}; //Creamos un index para tagear cada carta
    public static System.Random rnd = new System.Random(); 
    public int shuffleNum = 0; //Creamos variable que será usada para obtener una carta random
    int[] visibleFaces = { -1, -2 }; //Crearemos lista que muestre numero de cartas visibles
    public int contadorAciertos = 0;
    public float timeStart;
    public TMP_Text textBox;
    public TMP_Text puntaje;
    bool timerActive = true;
    private int puntos = 0;

    private void Start() 
    {
        int longOriginal = indexCaras.Count; //Agregamos las cartas al tablero de manera aleatoria manteniendo un órden en la manera en la que
        float posicionY = 3.3f;//               se encuentran formadas
        float posicionX = -6;
        for(int i = 0; i < 16; i++)
        {
            shuffleNum = rnd.Next(0, (indexCaras.Count));
            var temp = Instantiate(carta, new Vector3(posicionX, posicionY, 0), Quaternion.identity);
            posicionX = posicionX + 3;
            if (i == 4)
            {
                posicionY = 0;
                posicionX = -7.5f;
            }else if(i == 10){
                posicionY = -3.3f;
                posicionX = -6;
            }
            temp.GetComponent<CartaPrincipal>().indexCaras = indexCaras[shuffleNum];
            indexCaras.Remove(indexCaras[shuffleNum]);
        }
        textBox.text = timeStart.ToString("F2");
        puntaje.text = ("");

    }

    public bool DosCartasAbiertas()
    {
        bool cartaAbierta = false;//Dice cuando una carta se encuentra abierta
        if (visibleFaces[0] >= 0 && visibleFaces[1] >= 0)
        {
            cartaAbierta = true;
        }
        return cartaAbierta;
    }

    public void AgregarCaraVisible(int index)
    {
        if (visibleFaces[0] == -1) //Agrega una carta visible haciendo que solo pueda voltearse una carta más
        {
            visibleFaces[0] = index;
        }
        else if(visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
        }
    }

    public void QuitarCaraVisible(int index)
    {
        if (visibleFaces[0] == index) //Quita una carta visible diciendo que aún se pueden abrir más cartas
        {
            visibleFaces[0] = -1;
        }
        else if (visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
        }
    }

    public bool VerificarMatch()
    {
        bool correcto = false;//Verifica que las cartas macheen
        if ((visibleFaces[0]%2)==0 )
        {
            if (visibleFaces[0] == (visibleFaces[1] - 1))
            {
                visibleFaces[0] = -1;
                visibleFaces[1] = -2;
                correcto = true;
                contadorAciertos += 1;
                if(contadorAciertos == 8)
                {
                    Debug.Log("Fin");
                    Puntuacion();
                }
            }

        }
        else
        {
            if (visibleFaces[0] == (visibleFaces[1] + 1))
            {
                visibleFaces[0] = -1;
                visibleFaces[1] = -2;
                correcto = true;
                contadorAciertos += 1;
                if (contadorAciertos == 8)
                {
                    Debug.Log("Fin");
                    Puntuacion();
                }
            }
        }
        return correcto;
    }

    public void Puntuacion()//Asigna los puntos obtenidos según el tiempo que se tardó en resolver el juego y los imprime
    {
        timerActive = false;
        if(timeStart < 40)
        {
            puntos = 100;
            puntaje.text = (puntos+" puntos\nTiempo: " + timeStart + " segundos");
        }
        else if(timeStart < 45)
        {
            puntos = 95;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 50)
        {
            puntos = 90;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 55)
        {
            puntos = 85;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 60)
        {
            puntos = 80;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 65)
        {
            puntos = 75;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 70)
        {
            puntos = 70;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 75)
        {
            puntos = 65;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 80)
        {
            puntos = 60;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 85)
        {
            puntos = 65;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else if (timeStart < 90)
        {
            puntos = 55;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }
        else
        {
            puntos = 50;
            puntaje.text = (puntos + " puntos\nTiempo: " + timeStart + " segundos");
        }

    }

    private void Awake()
    {
        carta = GameObject.Find("Carta");
    }

    private void Update() //Avanza el tiempo del cronómetro
    {
        if (timerActive)
        {
            timeStart += Time.deltaTime;
            textBox.text = timeStart.ToString("F2");
        }
    }
}
