using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalMarkerPassableEvent : ScriptableObject
{
    // Start is called before the first frame update
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<AnimalMarkerPassableEventListener> eventListeners = new List<AnimalMarkerPassableEventListener>();

    public void Raise(AnimalMarker cm)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(cm);
    }

    public void RegisterListener(AnimalMarkerPassableEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(AnimalMarkerPassableEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
