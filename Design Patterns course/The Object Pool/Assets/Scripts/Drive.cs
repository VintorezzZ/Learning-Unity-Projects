using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject bullet;
    public Slider healthbar;
    public GameObject explosion;
    public AudioSource destroyed;
    public AudioSource hit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            if (healthbar != null)
                healthbar.value -= 10;
            //hit.Play();
            if (healthbar.value <= 0)
            {
                //Instantiate(explosion, this.transform.position, Quaternion.identity);
                healthbar.value = 0;
                if (healthbar != null)
                    Destroy(healthbar.gameObject, 0.1f);
                this.gameObject.GetComponent<Renderer>().enabled = false;
                Destroy(this.gameObject, 2f);
                //destroyed.Play();

            }
        }
    }

    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        transform.Translate(translation, 0, 0);

        if(Input.GetKeyDown("space"))
        {
            //Instantiate(bullet, this.transform.position, Quaternion.identity);
            GameObject bullet = Pool.singleton.Get("Bullet");
            if(bullet != null)
            {
                bullet.transform.position = this.transform.position;
                bullet.SetActive(true);
            }
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position) +
                                  new Vector3(0,-130,0);
        if(healthbar != null)
            healthbar.transform.position = screenPos;
    }
}