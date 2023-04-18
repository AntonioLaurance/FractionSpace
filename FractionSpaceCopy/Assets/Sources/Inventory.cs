using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;

    void Start(){
        HasChanged();
    }

    /*
     * Orden de los operadores para envio al servidor
     * slot0 -> numerador1
     * slot3 -> denominador1
     * slot1 -> numerador2
     * slot4 -> denoninador2
     * slot2 -> num
     * slot5 -> den
    */
    public void HasChanged () {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (Transform slotTansform in slots){
            GameObject item = slotTansform.GetComponent<SLOT>().item;
            string element = slotTansform.ToString();

            // Debug.Log(slotTansform.name);
            if (item){
                builder.Append(item.name);
                builder.Append(" - ");
             
                Debug.Log(builder.ToString().Length);
                Debug.Log(element);

                // Comprobamos si las 6 fichas ya están puestas
                if (builder.ToString().Length == 54)
                {
                    Debug.Log("Enviar operación al servidor.");

                    // Simulamos un formulario web
                    WWWForm form = new WWWForm();
                    form.AddField("exercise");
                    
                }
            }
        }
        inventoryText.text = builder.ToString();
    }
}

namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasChanged();
    }
}
