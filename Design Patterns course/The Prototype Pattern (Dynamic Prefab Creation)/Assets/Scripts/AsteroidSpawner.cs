using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Material material;
    public GameObject asteroid;



    public void CreateAsteroid()
    {
        asteroid = ProcAsteroid.Clone(this.transform.position);
        asteroid.GetComponent<MeshRenderer>().sharedMaterial = material;
    }

}
