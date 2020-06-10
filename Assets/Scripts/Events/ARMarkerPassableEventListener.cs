using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMarkerPassableEventListener : MonoBehaviour
{

    [Tooltip("Event to register with.")]
    public ARMarkerPassableEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public ARMarkerEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(ARMarker cm)
    {
        Response.Invoke(cm);
    }
}
