using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    //Static fields
    static float GRAVITY_CONSTANT = 0.2f;
    
    //Public tweakables
    public GameObject planet;
    public float GRAVITY_COEFFICIENT = 1f;
    public int PATH_ACCURACY;
    public float PATH_TIME;

    //Private fields for internal logic
    private Rigidbody2D objRigidBody2D;
    private Vector2 objPosition;
    private float planetMass;
    private Vector2 planetPosition;
    private LineRenderer orbitLine;

    void Start ()
    {
        objRigidBody2D = GetComponent<Rigidbody2D>();
        objPosition = new Vector2(transform.position.x, transform.position.y);
        planetMass = planet.GetComponent<Planet>().mass;
        planetPosition = new Vector2(planet.transform.position.x, planet.transform.position.y);
        orbitLine = GetComponent<LineRenderer>();
	}
	
	void Update ()
    {
        //Update current 2D position for various functions to use
        objPosition.Set(transform.position.x, transform.position.y);

        //Draw prediction of orbital path
        DrawOrbit();
    }

    private void FixedUpdate()
    {
        //Apply gravitational force
        ApplyGravity();
    }

    void ApplyGravity ()
    {
        //Calculate distance vector
        Vector2 distance = planetPosition - objPosition;

        //Calculate gravitational force
        // Fg = G*M*m/d^2
        float mass = objRigidBody2D.mass;
        float force = GRAVITY_CONSTANT * GRAVITY_COEFFICIENT * mass * planetMass / distance.sqrMagnitude;

        //Apply gravitational force
        objRigidBody2D.AddForce(distance.normalized * force);
    }

    void DrawOrbit ()
    {
        Vector3[] orbitPath = GetOrbitPath(PATH_ACCURACY, PATH_TIME);
        orbitLine.positionCount = orbitPath.Length;
        orbitLine.SetPositions(orbitPath);
    }

    /* Get an array of points that approximate the orbital path
     * through an iterative process of stepping through the
     * position and gravity functions.
     * 
     * Parameters:
     * steps: the number of iterations to step through on the path
     * t: the time that this list of points spans
     * 
     * Return type:
     * An array of Vector3 trivially constructed from Vector2 calculations
     * by adding a z of zero to the Vector2 positions
     */
    Vector3[] GetOrbitPath (int steps, float t)
    {
        //Set up loop variables
        Vector2 position = new Vector2();
        Vector2 positionInitial = new Vector2(objPosition.x, objPosition.y);
        Vector2 velocity = new Vector2();
        Vector2 velocityInitial = new Vector2(objRigidBody2D.velocity.x, objRigidBody2D.velocity.y);
        //timestep is the time that each stepthrough of the kinematic functions compute
        float timestep = t / (float)steps;

        //Create path list
        Vector3[] path = new Vector3[steps+1];
        //Add current position to path
        path[0] = new Vector3(positionInitial.x, positionInitial.y, 0f);

        for (int i = 1; i <= steps; ++i)
        {
            //Calculate distance vector
            Vector2 distance = planetPosition - positionInitial;

            //Calculate new gravitational acceleration
            //g = G*M/d^2
            float distanceSquared = distance.sqrMagnitude;
            Vector2 g = (GRAVITY_CONSTANT * GRAVITY_COEFFICIENT * planetMass / distanceSquared) * distance.normalized;

            //Calculate new velocity
            //v = g*t + vInit
            velocity = g * timestep + velocityInitial;

            //Calculate new position
            //x = (g/2)*t^2 + vInit*t + xInit
            position = (g / 2) * timestep * timestep + velocityInitial * timestep + positionInitial;

            //Check if path collides with planet
            if (CollidedPlanet(position))
            {
                //write remaining path vertices to last valid point
                for (int j = i; j <= steps; ++j)
                {
                    path[j] = new Vector3(positionInitial.x, positionInitial.y, 0f);
                }
                break;
            }

            //Add new position to path
            path[i] = new Vector3(position.x, position.y, 0f);
;
            //Update initial position and velocity
            positionInitial.Set(position.x, position.y);
            velocityInitial.Set(velocity.x, velocity.y);
        }
        return path;
    }

    bool CollidedPlanet (Vector2 pos)
    {
        return ((pos - planetPosition).magnitude < planet.GetComponent<Planet>().Radius);
    }
}
