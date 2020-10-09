using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public ParticleSystem expl;

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;            
            Instantiate(expl, transform.position, transform.rotation);
            Destroy(this.gameObject, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            GetDamage(maxHealth);
            Instantiate(expl, transform.position, transform.rotation);
            Destroy(this.gameObject, 0f);
            GameManager.instance.SetDeathUI();
        }
        else if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().GetDamage(maxHealth);
            GetDamage(maxHealth);
            Instantiate(expl, transform.position, transform.rotation);
            Destroy(this.gameObject, 0f);
            GameManager.instance.SetDeathUI();
        }    
    }
}
