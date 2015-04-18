using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

    public const int WIDTH  = 100;
    public const int HEIGHT = 100;

    public int mass;

    public Vector3 velocity;

    public Vector3 corner1;
    public Vector3 corner2;
    public Vector3 corner3;
    public Vector3 corner4;

    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;

    public float e = 0.0f;

	// Use this for initialization
	void Start ()
    {
        sphere1 = GameObject.Find("Sphere1");
        sphere2 = GameObject.Find("Sphere2");
        sphere3 = GameObject.Find("Sphere3");
        sphere4 = GameObject.Find("Sphere4");

        mass = 0;
        velocity = corner1 = corner2 = corner3 = corner4 = new Vector3();

	}
	
	// Update is called once per frame
	void Update ()
    {
        corner1 = sphere1.transform.position;
        corner2 = sphere2.transform.position;
        corner3 = sphere3.transform.position;
        corner4 = sphere4.transform.position;
	}
}
