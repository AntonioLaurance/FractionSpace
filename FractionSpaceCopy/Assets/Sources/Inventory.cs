using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;

    void Start () {
        HasChanged ();
    }

    public void HasChanged () {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (Transform slotTansform in slots){
            GameObject item = slotTansform.GetComponent<SLOT>().item;
            if (item){
                builder.Append (item.name);
                builder.Append (" - ");
            }
        }
        inventoryText.text = builder.ToString ();
    }
}

namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasChanged();
    }
}
