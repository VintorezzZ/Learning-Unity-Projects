using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionSound;
    public GameObject explosionVFX;
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            Instantiate(explosionSound, transform.position, transform.rotation);
            Instantiate(explosionVFX, transform.position, transform.rotation);
            Destroy(explosionSound, 4f);
            Destroy(explosionVFX, 4f);
            hit = true;
        }
    }
}
