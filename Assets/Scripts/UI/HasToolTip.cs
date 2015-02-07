using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HasToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string toolTipText;
    private ToolTipManager _manager;

    void Awake() {
	// grab manager in awake, it may be inactive during start
        _manager = FindObjectOfType<ToolTipManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _manager.SetText(toolTipText);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _manager.ClearText();
    }
}