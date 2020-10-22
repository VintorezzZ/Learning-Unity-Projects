using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{

    // public GameObject bullet;
    public GameObject asteroid;
    // public GameObject asteroidContainer;

    private void Start()
    {
        Cursor.visible = false;
        
        asteroid.SetActive(false);
        CreateAsteroids(4);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);
    }

    private void CreateAsteroids(float asteroidsNum)
    {
        for (int i = 1; i <= asteroidsNum; i++) {
            GameObject asteroidsClone = Instantiate(asteroid, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            // asteroidsClone.transform.parent = asteroidContainer.transform;
            asteroidsClone.SetActive(true);
        }

    }

     public void RocketFail()
    {
        print("ROCKET IS DEAD");

    }



}
