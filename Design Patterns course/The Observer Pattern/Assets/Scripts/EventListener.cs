using UnityEngine.Events;
using UnityEngine;


[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject> { }

public class EventListener : MonoBehaviour
{
    public Event getEvent;
    public UnityGameObjectEvent response = new UnityGameObjectEvent();

    private void OnEnable()
    {
        getEvent.Register(this);
    }

    private void OnDisable()
    {
        getEvent.Unregister(this);
    }

    public void OnEventOccurs(GameObject go)
    {
        response.Invoke(go);
    }
}
