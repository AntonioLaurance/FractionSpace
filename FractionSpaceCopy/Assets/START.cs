using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class START : MonoBehaviour
{
    public bool pasarEscena;
    public int indicaEscena;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCode.Mouse0)){
            cambiarEscena(indicaEscena);
            
        }

        if(pasarEscena){
            cambiarEscena(indicaEscena);
        }
        
    }

    public void cambiarEscena(int indice){
        SceneManagement.LoadScene(indice)
    }
}
