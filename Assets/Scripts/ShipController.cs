using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //Public tweakables
    public GameObject gameManager;
    public GameObject LEFT_ENGINE_PARTICLES;
    public GameObject RIGHT_ENGINE_PARTICLES;
    public float thrust;
    public float rotationSpeed;
    public Vector2 START_VELOCITY;
    public GameObject BULLET;
    public float BULLET_SPEED;

    //Private fields for internal logic
    private Rigidbody2D shipRigidBody2D;
    private float bulletCooldown;

	void Start ()
    {
        shipRigidBody2D = GetComponent<Rigidbody2D>();
        shipRigidBody2D.velocity = START_VELOCITY;
        bulletCooldown = 0f;
	}
	
	void Update ()
    {
        if (bulletCooldown > 0f) bulletCooldown -= Time.deltaTime;
        ApplyMovementInput();
        GetFire();
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    //Read Input axes and apply their values to the rigid body
    void ApplyMovementInput ()
    {
        //Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Calculate thruster force
        float thrustMagnitude = vertical * thrust;
        Vector2 thrustForce = thrustMagnitude * transform.up;

        //Apply thruster force
        shipRigidBody2D.AddForce(thrustForce);
        LEFT_ENGINE_PARTICLES.GetComponent<ParticleSystem>().maxParticles = (int) (vertical * 100f);
        RIGHT_ENGINE_PARTICLES.GetComponent<ParticleSystem>().maxParticles = (int)(vertical * 100f);

        //Calculate rotation
        float rotation = horizontal * rotationSpeed;

        //Apply rotation
        transform.Rotate(-transform.forward, rotation);
    }

    void GetFire ()
    {
        //Get input axis
        float fire = Input.GetAxis("Fire1");

        if (fire > 0f && bulletCooldown <= 0f)
        {
            bulletCooldown = 0.5f;
            GameObject bullet = Instantiate(BULLET);
            bullet.transform.position = transform.position + transform.up * 0.2f;
            bullet.GetComponent<Rigidbody2D>().velocity = shipRigidBody2D.velocity;
            Vector2 forward = new Vector2(transform.up.x, transform.up.y);
            bullet.GetComponent<Bullet>().Launch(forward * BULLET_SPEED);
            bullet.GetComponent<Gravity>().planet = GetComponent<Gravity>().planet;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        gameManager.GetComponent<GameManager>().GameOver();
        Debug.Log("Destroyed ship");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            gameManager.GetComponent<GameManager>().GameOver();
        }
    }
}
