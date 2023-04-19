using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;
    public Exercise suma;

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
     * 
     * slot [element]
     * imagen [item]
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
             
                Debug.Log(element);
                Debug.Log("Número: " + item.name);
                // int number = System.Int32.Parse(txt);

                // Asignamos a un elemento de nuestra clase
                /* 
                 * if(element == "slot0")
                {
                    suma.num1 = System.Int32.Parse(item.name);
                }

                if (element == "slot1")
                {
                    suma.num2 = System.Int32.Parse(item.name);
                }

                if (element == "slot2")
                {
                    suma.num = System.Int32.Parse(item.name);
                }

                if (element == "slot3")
                {
                    suma.den1 = System.Int32.Parse(item.name);
                }

                if (element == "slot4")
                {
                    suma.den2 = System.Int32.Parse(item.name);
                }

                if (element == "slot5")
                {
                    suma.den = System.Int32.Parse(item.name);
                }
                */

                // Comprobamos si las 6 fichas ya están puestas
                if (builder.ToString().Length == 24)
                {
                    Debug.Log("Enviar operación al servidor.");
                    // Debug.Log(number);

                    // Obtenemos elementos de la operación de nuestra GUI
                    GameObject slot0 = GameObject.Find("Canvas/Panel/inventario(1)/slot1");
                    suma.num1 = System.Int32.Parse(slot0.transform.GetChild(0).name);

                    GameObject slot1 = GameObject.Find("Canvas/Panel/inventario(1)/slot1");
                    suma.num2 = System.Int32.Parse(slot1.transform.GetChild(0).name);

                    GameObject slot2 = GameObject.Find("Canvas/Panel/inventario(1)/slot2");
                    suma.num = System.Int32.Parse(slot2.transform.GetChild(0).name);

                    GameObject slot3 = GameObject.Find("Canvas/Panel/inventario(1)/slot3");
                    suma.den1 = System.Int32.Parse(slot3.transform.GetChild(0).name);

                    GameObject slot4 = GameObject.Find("Canvas/Panel/inventario(1)/slot4");
                    suma.den2 = System.Int32.Parse(slot4.transform.GetChild(0).name);

                    GameObject slot5 = GameObject.Find("Canvas/Panel/inventario(1)/slot5");
                    suma.den = System.Int32.Parse(slot5.transform.GetChild(0).name);

                    // Serializamos
                    string message = JsonUtility.ToJson(suma);
                    Debug.Log(message);

                    // Enviamos datos al servidor
                    SendToServer(message);
                }
            }
        }
        inventoryText.text = builder.ToString();
    }

    IEnumerable SendToServer(string json)
    {
        Debug.Log("Dentro de SendToServer.");

        // Simulamos un formulario web
        WWWForm form = new WWWForm();
        form.AddField("exercise", json);

        // Enviamos al servidor para verificar
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/suma", form))
        {
            yield return www.SendWebRequest();

            // Checamos si hay error
            if (www.result != UnityWebRequest.Result.Success)
            {
                // Imprimimos mensaje de error
                Debug.Log(www.error);
            }
            else
            {
                string txt = www.downloadHandler.text;
                Debug.Log(txt);
            }
        }
    }
}

namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasChanged();
    }
}
