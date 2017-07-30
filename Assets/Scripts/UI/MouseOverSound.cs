using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class MouseOverSound : MonoBehaviour, IPointerEnterHandler {

    private Selectable select;

    private void Awake()
    {
        select = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (select.interactable)
        {
            AudioManager.Instance.PlayEffect("mouseover");
        }       
    }
}
