using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : MonoBehaviour
{
    public Transform gun;
    public Transform barrel;
    public float gunSpeed;
    public float barrelSpeed;
    private float gunAngle;
    private float barrelAngle;
    public GameObject bullet;
    public Transform barrelL;
    public Transform barrelR;
    public float bulletSpeed;
    private float nextTimeToFire;
    private float fireRate = 5f;
    public AudioClip shotSound;
    public AudioSource audioSource;
    public ParticleSystem shootVFX;

    private int currentAmmo = 30;
    private float timer;


    private void Start()
    {
        
    }
    private void Update()
    {        
        RotateGun();
        RotateBarrel();

        if (Input.GetButton("Fire1") && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (currentAmmo > 0)
            {
                Shoot();
            }                        
        }

        UpdateAmmo();
        GameManager.instance.UpdateAmmo(currentAmmo);
    }

    void RotateGun()
    {
        gunAngle += Input.GetAxis("Mouse X") * GameManager.instance.sens * Time.deltaTime;
        gunAngle = Mathf.Clamp(gunAngle,-90,90);
        gun.localRotation = Quaternion.AngleAxis(gunAngle, Vector3.up);
    }

    void RotateBarrel()
    {
        barrelAngle += Input.GetAxis("Mouse Y") * barrelSpeed * -Time.deltaTime;
        barrelAngle = Mathf.Clamp(barrelAngle, -1, 20);
        barrel.localRotation = Quaternion.AngleAxis(barrelAngle, Vector3.right);
    }

    void Shoot()
    {        
        GameObject bulletInstanceR;
        GameObject bulletInstanceL;

        //bulletInstanceR = Instantiate(bullet, barrelR.position, Quaternion.identity) as GameObject;
        bulletInstanceL = Instantiate(bullet, barrelL.position, Quaternion.identity) as GameObject;

        //Rigidbody bulletRbR;
        Rigidbody bulletRbL;

       // bulletRbR = bulletInstanceR.GetComponent<Rigidbody>();
        bulletRbL = bulletInstanceL.GetComponent<Rigidbody>();

       // bulletRbR.AddForce(barrelR.forward * bulletSpeed);
        bulletRbL.AddForce(barrelL.forward * bulletSpeed);

       // Destroy(bulletInstanceR, 10f);
        Destroy(bulletInstanceL, 10f);

        currentAmmo--;



        audioSource.PlayOneShot(shotSound);
        shootVFX.Play(shootVFX);
    }

    void UpdateAmmo()
    {        
        timer += Time.deltaTime;
        if (timer >= 2.5f)
        {
            currentAmmo++;
            timer = 0;
        }

        if (currentAmmo < 0)
        {
            currentAmmo = 0;
        }
    }

}
