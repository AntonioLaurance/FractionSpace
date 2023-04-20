using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta : MonoBehaviour {

    public int id;
    public Juego juego;
    public GameObject cartaBocaArriba;
    public GameObject cartaBocaAbajo;
    public bool bocaArriba = false;
    public bool estaBocaAbajo = true;

    // método que se llama cuando se hace clic en la carta
    public void OnMouseDown() {
        if (!bocaArriba) {
            juego.CartaSeleccionada(this);
        }
    }

    // método para voltear la carta boca arriba
    public void VoltearBocaArriba() {
        cartaBocaArriba.SetActive(true);
        cartaBocaAbajo.SetActive(false);
        bocaArriba = true;
    }

    // método para voltear la carta boca abajo
    public void VoltearBocaAbajo() {
        cartaBocaArriba.SetActive(false);
        cartaBocaAbajo.SetActive(true);
        bocaArriba = false;
    }

    public void Voltear() {
        estaBocaAbajo = !estaBocaAbajo;
        cartaBocaArriba.SetActive(!estaBocaAbajo);
        cartaBocaAbajo.SetActive(estaBocaAbajo);
    }

    // método para determinar si la carta está boca abajo
    public bool EstaBocaAbajo() {
        return estaBocaAbajo;
    }
}
