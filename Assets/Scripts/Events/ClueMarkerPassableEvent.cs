using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClueMarkerPassableEvent : ScriptableObject
{
    // Start is called before the first frame update
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<ClueMarkerPassableEventListener> eventListeners = new List<ClueMarkerPassableEventListener>();

    public void Raise(ClueMarker cm)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(cm);
    }

    public void RegisterListener(ClueMarkerPassableEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(ClueMarkerPassableEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
