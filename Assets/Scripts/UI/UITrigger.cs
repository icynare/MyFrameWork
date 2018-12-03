using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITrigger : UnityEngine.EventSystems.EventTrigger
{

    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;

	public static UITrigger Get(GameObject go, bool scale = true)
    {
        UITrigger uTrigger = go.GetComponent<UITrigger>();
        if(uTrigger == null)
        {
            uTrigger = go.AddComponent<UITrigger>();
        }
        return uTrigger;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        if (onClick != null)
            onClick(gameObject);
        else
            Debug.Log("onClick == null");
    }

}
