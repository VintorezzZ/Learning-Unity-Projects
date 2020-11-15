using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToSpawn : MonoBehaviour
{
    public Event dropped;
    public Event pickup;
    public Image icon;

    // Start is called before the first frame update
    void Start()
    {
        dropped.Occurred(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            pickup.Occurred(this.gameObject);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 5);
        }
    }
}
