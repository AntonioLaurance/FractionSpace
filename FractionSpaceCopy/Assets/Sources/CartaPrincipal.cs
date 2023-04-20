using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaPrincipal : MonoBehaviour
{
    GameObject funcionamiento;
    SpriteRenderer spriteRenderer;
    public Sprite[] caras;
    public Sprite enfrente;
    public Sprite detras;
    public int indexCaras;
    public bool matched = false;


    public void OnMouseDown()
    {
        if(matched == false)
        {
            if (spriteRenderer.sprite == detras) //Si el sprite renderer que se está usando es el que se encuentra si ser revelada se voltea
            {
                if (funcionamiento.GetComponent<Funcionamiento>().DosCartasAbiertas() == false)
                {
                    spriteRenderer.sprite = caras[indexCaras];
                    funcionamiento.GetComponent<Funcionamiento>().AgregarCaraVisible(indexCaras);
                    matched = funcionamiento.GetComponent<Funcionamiento>().VerificarMatch();
                }
            }
            else
            {
                spriteRenderer.sprite = detras; //Si la carta ya está siendo revelada se voltea para ser oculta de nuevo
                funcionamiento.GetComponent<Funcionamiento>().QuitarCaraVisible(indexCaras);
            }
        }
        
    }

    private void Awake()
    {
        funcionamiento = GameObject.Find("Funcionamiento");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
