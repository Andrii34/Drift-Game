using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressableButton : MonoBehaviour
{
    public bool IsPressed { get; private set; }

    [SerializeField]
    private float sensitivity = 2f; 

    public float DampenPress { get; private set; } 

    private void Start()
    {
        SetUpButton();
    }

    private void Update()
    {
        UpdateDampenPress();
    }

    private void UpdateDampenPress()
    {
        if (IsPressed)
        {
            DampenPress += sensitivity * Time.deltaTime;
        }
        else
        {
            DampenPress -= sensitivity * Time.deltaTime;
        }
        DampenPress = Mathf.Clamp01(DampenPress);
    }

    private void SetUpButton()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        AddEventTrigger(trigger, EventTriggerType.PointerDown, () => SetPressState(true));
        AddEventTrigger(trigger, EventTriggerType.PointerUp, () => SetPressState(false));
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action action)
    {
        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(_ => action.Invoke());
        trigger.triggers.Add(entry);
    }

    private void SetPressState(bool isPressed)
    {
        IsPressed = isPressed;
    }
}
