using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    //Public tweakables
    public GameObject gameManager;
    public float mass;

    //Public properties
    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }

    //Private fields for internal logic
    private float radius;

	void Start ()
    {
        Radius = GetComponent<CircleCollider2D>().radius * transform.localScale.magnitude;
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid") || collision.CompareTag("Ship")) gameManager.GetComponent<GameManager>().GameOver();
    }
}
