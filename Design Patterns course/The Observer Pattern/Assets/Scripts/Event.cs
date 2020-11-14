using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Game Event", order = 52)]
public class Event : ScriptableObject
{
    private List<EventListener> eListeners = new List<EventListener>();

    public void Register(EventListener listener)
    {
        eListeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        eListeners.Remove(listener);
    }

    public void Occurred(GameObject go)
    {
        for (int i = 0; i < eListeners.Count; i++)
        {
            eListeners[i].OnEventOccurs(go);
        }
    }
}
