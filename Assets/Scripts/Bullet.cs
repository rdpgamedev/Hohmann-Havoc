using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    //Public tweakables

    //Private fields for internal logic

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.magnitude > 15f)
        {
            Destroy(gameObject);
        }
	}

    public void Launch (Vector2 vel)
    {
        GetComponent<Rigidbody2D>().velocity += vel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            try
            {
                collision.gameObject.GetComponent<Asteroid>().Destroy();
            }
            catch
            {
                Debug.Log(collision.gameObject);
            }
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Ship"))
        {
            collision.GetComponent<ShipController>().Destroy();
            return;
        }

        if (collision.CompareTag("Planet"))
        {
            Destroy(gameObject);
            return;
        }
    }
}
