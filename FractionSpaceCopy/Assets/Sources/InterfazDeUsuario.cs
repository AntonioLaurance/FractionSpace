using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfazDeUsuario : MonoBehaviour
{
    #region CAMPOS
    [Header("Mensaje Salteable (Cualquier tecla)")]
    [SerializeField]
    private GameObject mensajeSaltableObjeto;
    [SerializeField]
    private TMP_Text mensajeSaltableTexto;
    private bool mensajeSaltableActivo;
    #endregion

    #region METODOS

    private void Start()
    {
        mensajeSaltableActivo = true;
        mensajeSaltableObjeto.SetActive(true);
    }

    private void OnGUI()
    {
        if (mensajeSaltableActivo)
        {
            if (Input.anyKeyDown)
            {
                LimpiarMensajeTecla();
            }
        }
    }

   /* public void MostrarMensajeSalteableTecla(string mensaje)
    {
        mensajeSaltableActivo = true;
        mensajeSaltableTexto.text = mensaje;
        mensajeSaltableObjeto.SetActive (true);
    }
   */
    private void LimpiarMensajeTecla()
    {
        mensajeSaltableActivo = false;
        mensajeSaltableObjeto.SetActive(false) ;
    }

    #endregion
}
