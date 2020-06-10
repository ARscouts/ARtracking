using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalMarkerPassableEventListener : MonoBehaviour
{

    [Tooltip("Event to register with.")]
    public AnimalMarkerPassableEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public AnimalMarkerEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(AnimalMarker cm)
    {
        Response.Invoke(cm);
    }
}
