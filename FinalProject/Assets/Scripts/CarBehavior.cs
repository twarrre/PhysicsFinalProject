using UnityEngine;
using System.Collections;

public class CarBehavior : MonoBehaviour 
{
    public const float CAR_WIDTH     = 200;
    public const float CAR_HEIGHT    = 100;

    public const float TANK_WIDTH    = 40;
    public const float TANK_HEIGHT   = 80;

    public const float DRIVER_WIDTH  = 40;
    public const float DRIVER_HEIGHT = 40;

    public GameObject car;
    public GameObject tank;
    public GameObject driver;
    public GameObject centerOfMass;
    public GameObject upForce;
    public GameObject leftForce;
    public GameObject rightForce;

    public float carMass;
    public float tankMass;
    public float driverMass;

    public Vector3 force;
    public Vector3 velocity;

	// Use this for initialization
	void Start ()
    {
        Time.fixedDeltaTime         = 1 / 60;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount  = 0;

        car          = GameObject.Find("Car");
        tank         = GameObject.Find("Tank");
        driver       = GameObject.Find("Driver");
        centerOfMass = GameObject.Find("COM");
        upForce      = GameObject.Find("UpForce");
        leftForce    = GameObject.Find("LeftForce");
        rightForce   = GameObject.Find("RightForce");

        upForce.SetActive(false);
        leftForce.SetActive(false);
        rightForce.SetActive(false);

        carMass    = 1000;
        tankMass   = 0;
        driverMass = 0;

        force        = new Vector3();
        velocity     = new Vector3();
	}

    void OnGUI()
    {
        string label = "Car Position: " + car.transform.position;
        label        += "\nTank Position (U and I) : " + tank.transform.position;
        label        += "\nDriver Position (G and H) : " + driver.transform.position;
        label        += "\nCenter of Mass : " + centerOfMass.transform.position;
        label        += "\nCar Mass : " + carMass + " kg";
        label        += "\nTank Mass (T and Y) : " + tankMass + " kg";
        label        += "\nDriver Mass (D and F) : " + driverMass + " kg";
        label        += "\nTotal Mass  : " + carMass + " + " + tankMass + " + " + driverMass + " = " + (carMass + tankMass + driverMass) + " kg";
        label        += "\n\nForce : " + force + " N";
        label        += "\nVelocity : " + velocity + " m/s";
        GUI.Label(new Rect(0, 0, 500, 500), label);
    }

	// Update is called once per frame
	void Update ()
    {
        force = new Vector3();
        upForce.SetActive(false);
        leftForce.SetActive(false);
        rightForce.SetActive(false);
        CheckInput();
	}

    void FixedUpdate()
    {
        centerOfMass.transform.position = CalculateCenterOfMass(new float[3] { carMass, tankMass, driverMass }, new Vector3[3] { car.transform.position, tank.transform.position + car.transform.position, driver.transform.position + car.transform.position });

        Vector3 acceleration = force / (tankMass + driverMass + carMass);

        velocity = CalculateFinalVelocity(velocity, acceleration, Time.fixedDeltaTime);
        car.transform.position = CalculateDisplacement(velocity, acceleration, Time.fixedDeltaTime, car.transform.position);
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            tankMass = Mathf.Clamp(tankMass + 10, 0, 200);          
        if (Input.GetKeyDown(KeyCode.T))
            tankMass = Mathf.Clamp(tankMass - 10, 0, 200);
        if (Input.GetKeyDown(KeyCode.F))
            driverMass = Mathf.Clamp(driverMass + 10, 0, 200);
        if (Input.GetKeyDown(KeyCode.D))
            driverMass = Mathf.Clamp(driverMass - 10, 0, 200);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            force = new Vector3(0, 10000, 0);
            upForce.SetActive(true);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            force = new Vector3(0, -10000, 0);
            upForce.SetActive(true);
        }  
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rightForce.SetActive(true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftForce.SetActive(true);
        }
    }

    public static Vector3 CalculateCenterOfMass(float[] masses, Vector3[] positions)
    {
        if (masses.Length != positions.Length)
            return new Vector3();

        Vector3 com = new Vector3(0, 0, 0);
        float totalMass = 0;

        for (int i = 0; i < masses.Length; i++)
        {
            com.x += positions[i].x * masses[i];
            com.y += positions[i].y * masses[i];
            totalMass += masses[i];
        }

        com.x /= totalMass;
        com.y /= totalMass;

        return com;
    }

    public static Vector3 CalculateFinalVelocity(Vector3 velocity, Vector3 acceleration, float time)
    {
        return velocity + (acceleration * time);
    }

    public static Vector3 CalculateDisplacement(Vector3 velocity, Vector3 acceleration, float time, Vector3 pos)
    {
        return pos + (velocity * time) + ((acceleration * Mathf.Pow((float)time, 2) / 2.0f));
    }
}
