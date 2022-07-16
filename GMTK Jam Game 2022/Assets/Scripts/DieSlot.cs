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
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            slottedDie = eventData.pointerDrag.gameObject;
            slottedDie.GetComponent<DragDrop>().pickedUp = false;
            isEmpty = false;
        }
   }

   void Update()
    {
        if (slottedDie != null)
        {
            if (slottedDie.GetComponent<DragDrop>().pickedUp == true)
            {
                isEmpty = true;
            }
        }
    }

}
