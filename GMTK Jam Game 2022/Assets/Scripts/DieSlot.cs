using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DieSlot : MonoBehaviour, IDropHandler
{
   public bool isEmpty = true;

   public GameObject slottedDie = null;

   public void OnDrop(PointerEventData eventData)
   {
        if (eventData.pointerDrag != null && isEmpty)
        {
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            slottedDie = eventData.pointerDrag.gameObject;
            slottedDie.GetComponent<DragDrop>().slotted = true;
            isEmpty = false;
        }
   }

   void Update()
    {
        if (slottedDie != null)
        {
            if (slottedDie.GetComponent<DragDrop>().slotted == false)
            {
                isEmpty = true;
            }
        }
    }

}
