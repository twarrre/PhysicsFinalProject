using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

    public const int WIDTH = 100;
    public const int HEIGHT = 100;

    public int mass;

    public Vector3 velocity;

    public Vector3 corner1;
    public Vector3 corner2;
    public Vector3 corner3;
    public Vector3 corner4;

	// Use this for initialization
	void Start ()
    {
        mass = 0;
        velocity = corner1 = corner2 = corner3 = corner4 = new Vector3();

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
