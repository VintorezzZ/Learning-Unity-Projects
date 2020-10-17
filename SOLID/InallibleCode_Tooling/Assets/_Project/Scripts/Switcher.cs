using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public List<GameObject> ChangeableObjects;
    
    private readonly List<IChangeable> _changeableObjects = new List<IChangeable>();

    private void Start()
    {
        foreach (var changeable in ChangeableObjects)
        {
            _changeableObjects.Add(changeable.GetComponent<IChangeable>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (var changeable in _changeableObjects)
            {
                changeable.Next();
            }
        }
    }
}