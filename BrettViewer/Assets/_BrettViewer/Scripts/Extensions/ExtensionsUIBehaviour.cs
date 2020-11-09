using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ExtensionsUIBehaviour{

	public static void AddOnPointerDown(this UIBehaviour uiBehaviour, System.Action Action) {

		EventTrigger trigger = uiBehaviour.GetComponent<EventTrigger>();
		if(trigger == null) {
			trigger = uiBehaviour.gameObject.AddComponent<EventTrigger>();
		}

		var pointerDown = new EventTrigger.Entry();
		pointerDown.eventID = EventTriggerType.PointerDown;
		pointerDown.callback.AddListener((BaseEventData e)=>{ Action(); });
		trigger.triggers.Add(pointerDown);
	}
}
