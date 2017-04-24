using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    //Public tweakables
    public Vector2 START_VELOCITY;
    public float VELOCITY_DAMPENER;

    //Private fields for internal logic
    private Rigidbody2D asteroidRigidBody2D;

    void Start ()
    {
        asteroidRigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        asteroidRigidBody2D.velocity = asteroidRigidBody2D.velocity * VELOCITY_DAMPENER;
	}

    public void Destroy()
    {
        Destroy(gameObject);
        GameManager.instance.IncreaseScore();
        Debug.Log("Destroyed asteroid");
    }
}
