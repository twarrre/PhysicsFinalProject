using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

    public const int WIDTH  = 100;
    public const int HEIGHT = 100;

    public int mass = 500;

    public float radius;

    public Vector3 velocity;
    public Vector3 theta;
    public Vector3 omega;
    public Vector3 alpha;

    public Vector3 corner1;
    public Vector3 corner2;
    public Vector3 corner3;
    public Vector3 corner4;

    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;

    public float inertia;

    public float e = 0.0f;

	// Use this for initialization
	void Start ()
    {
        radius = Mathf.Sqrt(Mathf.Pow(WIDTH / 2.0f, 2.0f) + Mathf.Pow(HEIGHT / 2.0f, 2.0f)); 
        sphere1 = GameObject.Find("Sphere1");
        sphere2 = GameObject.Find("Sphere2");
        sphere3 = GameObject.Find("Sphere3");
        sphere4 = GameObject.Find("Sphere4");

        mass = 0;
        velocity = corner1 = corner2 = corner3 = corner4 = theta = alpha = omega = new Vector3();
        inertia = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void UpdateForces()
    {
        corner1 = sphere1.transform.position;
        corner2 = sphere2.transform.position;
        corner3 = sphere3.transform.position;
        corner4 = sphere4.transform.position;
    }

    public void UpdatePhysics()
    {
        //calculating the total inertia
        inertia = MomentOfInertiaRectangle(mass, WIDTH, HEIGHT);
        this.transform.position = CalculateDisplacement(velocity, new Vector3(), Time.deltaTime, this.transform.position);
    }

    public static Vector3 CalculateFinalVelocity(Vector3 velocity, Vector3 acceleration, float time)
    {
        return velocity + (acceleration * time);
    }

    public static Vector3 CalculateDisplacement(Vector3 velocity, Vector3 acceleration, float time, Vector3 pos)
    {
        return pos + (velocity * time) + ((acceleration * Mathf.Pow((float)time, 2) / 2.0f));
    }

    public static float MomentOfInertiaRectangle(float mass, float width, float height)
    {
        return (float)((mass * ((float)Mathf.Pow(width, 2) + (float)Mathf.Pow(height, 2))) / 12.0);
    }
}
