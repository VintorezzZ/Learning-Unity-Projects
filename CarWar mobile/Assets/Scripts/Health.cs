using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public ParticleSystem expl;
    [SerializeField] private int pointsToAdd;
    
    void Start()
    {
        health = maxHealth;
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0) 
        {
            health = 0;
            GameManager.instance.CountPoints(pointsToAdd);
            GameManager.instance.PlayFeedbacks();
            Instantiate(expl, transform.position,transform.rotation);
            Destroy(this.gameObject, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            GetDamage(maxHealth);
        }
        else if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().GetDamage(maxHealth);
        }

    }

}
