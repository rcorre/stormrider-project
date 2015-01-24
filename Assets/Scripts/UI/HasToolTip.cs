using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HasToolTip : MonoBehaviour {
    public string toolTipText;
    private RectTransform _area;
    private ToolTipManager _manager;

    void Awake() {
        _area = GetComponent<RectTransform>();
	// grab manager in awake, it may be inactive during start
        _manager = FindObjectOfType<ToolTipManager>();
    }

    void Start() {
	// set up mouse handlers
	var handlers = new List<EventTrigger.Entry>();

	// set tooltip text on mouse enter
	handlers.Add(CreateHandler(EventTriggerType.PointerEnter,
	    (ev) => _manager.SetText(toolTipText)));

	// clear tooltip on mouse leave
	handlers.Add(CreateHandler(EventTriggerType.PointerExit,
	    (ev) => _manager.ClearText()));

	// attach an event trigger with the given delegates
        var trigger = gameObject.AddComponent<EventTrigger>();
	trigger.delegates = handlers;
    }

    // return an event trigger entry that calls action upon receiving an event of the given type 
    EventTrigger.Entry CreateHandler(EventTriggerType type, Action<BaseEventData> action) {
        var entry = new EventTrigger.Entry();
        entry.eventID = type;

        entry.callback = new EventTrigger.TriggerEvent();
        var listener = new UnityAction<BaseEventData>(action);

        entry.callback.AddListener(listener);
        return entry;
    }
}