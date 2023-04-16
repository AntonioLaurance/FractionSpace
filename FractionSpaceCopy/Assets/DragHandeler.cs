using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    public static GameObject itemBeingDragged;
    Vector3 startPosition;

    // Start is called before the first frame update
    public void OnBeginDrag (PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
    }

    // Update is called once per frame
    public void OnDrag (PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        itemBeingDragged = null;
        transform.position = startPosition;
    }
}
