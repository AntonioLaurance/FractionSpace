using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Juego : MonoBehaviour {

    public Cubierta cubierta;
    public Text textoPuntuacion;
    public Text textoFinJuego;

    private int puntuacion = 0;
    private bool estaSeleccionando = false;
    private Carta cartaSeleccionada1 = null;
    private Carta cartaSeleccionada2 = null;

    // método para iniciar el juego
    private void IniciarJuego() {
        cubierta.Iniciar();
        textoFinJuego.gameObject.SetActive(false);
        puntuacion = 0;
        ActualizarPuntuacion();
    }

    // método para actualizar la puntuación
    private void ActualizarPuntuacion() {
        textoPuntuacion.text = "Puntuación: " + puntuacion;
    }

    // método para seleccionar una carta
    public void CartaSeleccionada(Carta carta) {
        if (estaSeleccionando) {
            return;
        }
        if (carta == cartaSeleccionada1) {
            return;
        }
        if (carta.estaBocaAbajo) {
            carta.Voltear();
            if (cartaSeleccionada1 == null) {
                cartaSeleccionada1 = carta;
            } else {
                cartaSeleccionada2 = carta;
                StartCoroutine(ChequearPareja());
            }
        }
    }

    // método para chequear si dos cartas forman una pareja
    private IEnumerator ChequearPareja() {
        estaSeleccionando = true;
        yield return new WaitForSeconds(1f);
        if (cartaSeleccionada1.id == cartaSeleccionada2.id) {
            puntuacion++;
            ActualizarPuntuacion();
            cartaSeleccionada1.gameObject.SetActive(false);
            cartaSeleccionada2.gameObject.SetActive(false);
            cartaSeleccionada1 = null;
            cartaSeleccionada2 = null;
            if (puntuacion == cubierta.filas * cubierta.columnas / 2) {
                textoFinJuego.text = "¡Has ganado!";
                textoFinJuego.gameObject.SetActive(true);
            }
        } else {
            cartaSeleccionada1.Voltear();
            cartaSeleccionada2.Voltear();
            cartaSeleccionada1 = null;
            cartaSeleccionada2 = null;
        }
        estaSeleccionando = false;
    }

    // método para reiniciar el juego
    public void Reiniciar() {
        IniciarJuego();
    }

    // método para salir del juego
    public void Salir() {
        Application.Quit();
    }

    // método para inicializar el juego
    private void Start() {
        IniciarJuego();
    }

}
