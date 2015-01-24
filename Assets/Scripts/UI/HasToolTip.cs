using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class HasToolTip : MonoBehaviour {
    public string toolTipText;
    private RectTransform _area;
    private ToolTipManager _manager;

    void Awake() {
        _area = GetComponent<RectTransform>();
        _manager = FindObjectOfType<ToolTipManager>();
    }

    void Start() {
	// hook up onMouseEnter handler to activate tooltip
        var trigger = gameObject.AddComponent<EventTrigger>();
	var entry   = new EventTrigger.Entry();

	entry.eventID  = EventTriggerType.PointerEnter;
	entry.callback = new EventTrigger.TriggerEvent();

	UnityAction<BaseEventData> callback = 
	    new UnityAction<BaseEventData>(MouseEnterMethod);

	entry.callback.AddListener(callback);

	trigger.delegates = new List<EventTrigger.Entry>();
	trigger.delegates.Add(entry);
    }

    public void MouseEnterMethod(BaseEventData baseEvent) {
        Util.Assert(_manager != null, "manager null");
        _manager.SetText(toolTipText);
    }
}