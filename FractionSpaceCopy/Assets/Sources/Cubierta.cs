using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubierta : MonoBehaviour {

    public GameObject cartaPrefab;
    public Sprite[] imagenesCarta;
    public int filas = 2;
    public int columnas = 4;

    private List<int> ids = new List<int>();
    private List<Carta> cartas = new List<Carta>();

    // método para crear las cartas en la cubierta
    private void CrearCartas() {
        int totalCartas = filas * columnas;
        int[] indices = new int[totalCartas];
        for (int i = 0; i < totalCartas; i++) {
            indices[i] = i % (totalCartas / 2);
        }
        for (int i = 0; i < totalCartas; i++) {
            int temp = indices[i];
            int r = Random.Range(i, totalCartas);
            indices[i] = indices[r];
            indices[r] = temp;
        }
        for (int i = 0; i < totalCartas; i++) {
            int id = indices[i];
            GameObject cartaObj = Instantiate(cartaPrefab, transform.position, Quaternion.identity);
            cartaObj.transform.SetParent(transform);
            Carta carta = cartaObj.GetComponent<Carta>();
            carta.id = id;
            carta.juego = GetComponent<Juego>();
            carta.cartaBocaArriba.GetComponent<SpriteRenderer>().sprite = imagenesCarta[id];
            cartas.Add(carta);
        }
    }

    // método para mezclar las cartas en la cubierta
    private void MezclarCartas() {
        for (int i = 0; i < cartas.Count; i++) {
            int r = Random.Range(i, cartas.Count);
            Carta temp = cartas[i];
            cartas[i] = cartas[r];
            cartas[r] = temp;
        }
    }

    // método para iniciar la cubierta
    public void Iniciar() {
        CrearCartas();
        MezclarCartas();
        int cartaIndex = 0;
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                Carta carta = cartas[cartaIndex];
                carta.transform.position = new Vector3(j * 2.5f, -i * 3f, 0f);
                cartaIndex++;
            }
        }
    }

}
