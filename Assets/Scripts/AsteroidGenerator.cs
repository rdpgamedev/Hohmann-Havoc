using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour {
    public GameObject ASTEROID;
    public GameObject PLANET;

    private float asteroidCooldown;

	void Start ()
    {
        asteroidCooldown = 5f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        asteroidCooldown -= Time.deltaTime;
        if (asteroidCooldown < 0f)
        {
            SpawnAsteroid();
        }
	}

    private void SpawnAsteroid ()
    {
        asteroidCooldown = 6f;
        GameObject asteroid = Instantiate(ASTEROID);
        Vector3 random3 = Random.onUnitSphere;
        Vector2 random2 = new Vector2(random3.x, random3.y).normalized;
        Vector2 position = random2 * 14f;
        Vector2 clamped = new Vector2(position.x, Mathf.Clamp(position.y, -8f, 8f));
        asteroid.transform.position = clamped;
        Debug.Log("planet: " + PLANET.transform.position);
        Debug.Log("asteroid: " + asteroid.transform.position);
        asteroid.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * 0.02f + (PLANET.transform.position - asteroid.transform.position) * 0.05f;
        asteroid.GetComponent<Gravity>().planet = PLANET;
    }
}
