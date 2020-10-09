using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lr;
    public Transform laser;

    private void Update()
    {
        lr.SetPosition(0, laser.position);
        RaycastHit hit;
        if (Physics.Raycast(laser.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
        }
        else lr.SetPosition(1, laser.position + (transform.forward * 5000));
    }
}
