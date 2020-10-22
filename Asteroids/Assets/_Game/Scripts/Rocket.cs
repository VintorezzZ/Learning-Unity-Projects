using UnityEngine;

public class Rocket : MonoBehaviour
{

    public GameObject bullet;
    public GameObject rockeEngine;
    // public GameObject bulletContainer;

    private ParticleSystem ps;
    //private float thrust = 5f;
    private float thrust = 6f;
    private float rotationSpeed = 180f;
    private float MaxSpeed = 4.5f;

    private Camera mainCam;
    private Rigidbody rb;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();

        ps = rockeEngine.GetComponent<ParticleSystem>();
        ps.Stop();

        bullet.SetActive(false);
    }

    private void FixedUpdate()
    {
        ControlRocket();
        CheckPosition();

        
    }

  
    private void Update()
    {
       if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        {
            Shoot();
        }

        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            ps.Play();
        }
        if (Input.GetKeyUp("up") || Input.GetKeyUp("w"))
        {
            ps.Stop();
        } 
    }
  

    private void ControlRocket()
    {
        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * thrust * Input.GetAxis("Vertical"));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed));
    }

    private void CheckPosition()
    {

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge)
        {
            transform.position = new Vector2(sceneLeftEdge, transform.position.y);
        }
        if (transform.position.x < sceneLeftEdge)
        {
            transform.position = new Vector2(sceneRightEdge, transform.position.y);
        }
        if (transform.position.y > sceneTopEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneBottomEdge);
        }
        if (transform.position.y < sceneBottomEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneTopEdge);
        }

    }

    void Shoot()
    {
        GameObject bulletClone = Instantiate(bullet, new Vector2(bullet.transform.position.x, bullet.transform.position.y), transform.rotation);
        bulletClone.SetActive(true);
        bulletClone.GetComponent<Bullet>().KillOldBullet();
        bulletClone.GetComponent<Rigidbody>().AddForce(transform.up * 350);
    }
}
