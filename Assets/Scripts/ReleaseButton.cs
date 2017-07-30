using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ReleaseButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onRelease = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " pressed");
        onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " released");
        onRelease.Invoke();
    }
}
