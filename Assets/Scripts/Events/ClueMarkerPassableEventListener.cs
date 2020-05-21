using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClueMarkerPassableEventListener : MonoBehaviour
{

    [Tooltip("Event to register with.")]
    public ClueMarkerPassableEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public ClueMarkerEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(ClueMarker cm)
    {
        Response.Invoke(cm);
    }
}
