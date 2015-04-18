using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Box box1;

    public const float CAR_WIDTH = 200;
    public const float CAR_HEIGHT = 100;

    public const float TANK_WIDTH = 40;
    public const float TANK_HEIGHT = 80;

    public const float DRIVER_WIDTH = 40;
    public const float DRIVER_HEIGHT = 40;

    public GameObject tank;
    public GameObject driver;
    public GameObject centerOfMass;
    public GameObject upForceObject;
    public GameObject leftForceObject;
    public GameObject rightForceObject;

    public float carMass;
    public float tankMass;
    public float driverMass;

    public float radius;

    public Vector3 force;
    public Vector3 leftForce;
    public Vector3 rightForce;
    public Vector3 velocity;

    public float totalInertia;

    public Vector3 theta;
    public Vector3 omega;
    public Vector3 alpha;

    public Vector3 radialUp;
    public Vector3 radialLeft;
    public Vector3 radialRight;

    public Vector3 acceleration;
    public Vector3 angle;

    public float drag = 1;

    public const float C = 300f;
    public float c;

    public const float Crot = 500000f;
    public float crot;

    public Vector3[] corners;

    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;

    // Use this for initialization
    void Start()
    {
        Time.fixedDeltaTime = 1.0f / 60.0f;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        tank = GameObject.Find("Tank");
        driver = GameObject.Find("Driver");
        centerOfMass = GameObject.Find("COM");
        upForceObject = GameObject.Find("UpForce");
        leftForceObject = GameObject.Find("LeftForce");
        rightForceObject = GameObject.Find("RightForce");

        upForceObject.SetActive(false);
        leftForceObject.SetActive(false);
        rightForceObject.SetActive(false);

        radius = Mathf.Sqrt(Mathf.Pow(CAR_WIDTH / 2.0f, 2.0f) + Mathf.Pow(CAR_HEIGHT / 2.0f, 2.0f)); 
        carMass = 1000;
        tankMass = 0;
        driverMass = 0;
        totalInertia = 0;

        force = new Vector3();
        velocity = new Vector3();

        theta = new Vector3();
        alpha = new Vector3();
        omega = new Vector3();

        radialUp = new Vector3();
        radialLeft = new Vector3();
        radialRight = new Vector3();

        acceleration = new Vector3();
        angle = new Vector3();

        c = C * drag;
        crot = Crot * drag;

        sphere1 = GameObject.Find("CarSphere1");
        sphere2 = GameObject.Find("CarSphere2");
        sphere3 = GameObject.Find("CarSphere3");
        sphere4 = GameObject.Find("CarSphere4");
        corners = new Vector3[4];
    }

    public void UpdateForces()
    {
        corners[0] = sphere1.transform.position;
        corners[1] = sphere2.transform.position;
        corners[2] = sphere3.transform.position;
        corners[3] = sphere4.transform.position;

        force = new Vector3();
        leftForce = new Vector3();
        rightForce = new Vector3();

        upForceObject.SetActive(false);
        leftForceObject.SetActive(false);
        rightForceObject.SetActive(false);
    }

    public void UpdatePhysics()
    {
        Vector3 uforce = Quaternion.Euler(0, 0, theta.z * Mathf.Rad2Deg) * force;
        Vector3 lforce = Quaternion.Euler(0, 0, theta.z * Mathf.Rad2Deg) * leftForce;
        Vector3 rforce = Quaternion.Euler(0, 0, theta.z * Mathf.Rad2Deg) * rightForce;

        Vector3 com = CalculateCenterOfMass(new float[3] { carMass, tankMass, driverMass }, new Vector3[3] { this.transform.position, tank.transform.position + this.transform.position, driver.transform.position + this.transform.position });

        //calculating the induvidual momements of inertia
        float carInertia = MomentOfInertiaRectangle(carMass, CAR_WIDTH, CAR_HEIGHT);
        float tankInertia = MomentOfInertiaRectangle(tankMass, TANK_WIDTH, TANK_HEIGHT);
        float driverInertia = MomentOfInertiaRectangle(driverMass, DRIVER_WIDTH, DRIVER_HEIGHT);

        //calculating the distance of the object to the center of mass
        float carDistance = Vector3.Distance(this.transform.position, com);
        float tankDistance = Vector3.Distance(tank.transform.position + this.transform.position, com);
        float driverDistance = Vector3.Distance(driver.transform.position + this.transform.position, com);

        //calculating the inertia of each object about the center of mass
        float carInertiaAboutTheCom = MomentOfInertiaAboutACOM(carInertia, carMass, carDistance);
        float tankInertiaAboutTheCom = MomentOfInertiaAboutACOM(tankInertia, tankMass, tankDistance);
        float driverInertiaAboutTheCom = MomentOfInertiaAboutACOM(driverInertia, driverMass, driverDistance);

        //calculating the total inertia
        totalInertia = (carInertiaAboutTheCom + tankInertiaAboutTheCom + driverInertiaAboutTheCom);

        acceleration = (uforce) / (tankMass + driverMass + carMass);

        radialUp = (upForceObject.transform.position + this.transform.position) - com;
        radialLeft = (leftForceObject.transform.position + this.transform.position) - com;
        radialRight = (rightForceObject.transform.position + this.transform.position) - com;

        Vector3 tourqueUp = Vector3.Cross(radialUp, uforce);
        Vector3 tourqueLeft = Vector3.Cross(radialLeft, lforce);
        Vector3 tourqueRight = Vector3.Cross(radialRight, rforce);

        alpha = (tourqueLeft + tourqueRight) / totalInertia;

        //maybe just tourques and then the mass is replaced with total inierita?
        angle = ((tourqueLeft + tourqueRight) / crot * Time.deltaTime) + (((tourqueLeft + tourqueRight) - (crot * omega)) / crot) * (totalInertia / crot) * (Mathf.Exp(-(crot * Time.deltaTime / totalInertia)) - 1); //(omega * (float)Time.deltaTime) + ((alpha * (float)Mathf.Pow((float)Time.deltaTime, 2)) / 2.0f);
        theta += angle;
        omega = ((tourqueLeft + tourqueRight) - Mathf.Exp(-(crot * Time.deltaTime / totalInertia)) * ((tourqueLeft + tourqueRight) - crot * omega)) / crot; //(alpha * (float)Time.deltaTime);

        //uforce cause equation takes mass into consideration sp not acceleration?
        velocity = (uforce - Mathf.Exp(-(c * Time.deltaTime / (tankMass + driverMass + carMass))) * (uforce - c * velocity)) / c;
        this.transform.position = this.transform.position + (acceleration / c * Time.deltaTime) + ((acceleration - (c * velocity)) / c) * ((tankMass + driverMass + carMass) / c) * (Mathf.Exp(-(c * Time.deltaTime / (tankMass + driverMass + carMass))) - 1);

        //velocity = CalculateFinalVelocity(velocity, acceleration, Time.deltaTime);
        //car.transform.position = CalculateDisplacement(velocity, acceleration, Time.deltaTime, car.transform.position);

        this.transform.RotateAround(centerOfMass.transform.position, new Vector3(0, 0, 1), angle.z * Mathf.Rad2Deg);
    }
    // Update is called once per frame
    void Update()
    {

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

    public static float MomentOfInertiaAboutACOM(float inertia, float mass, float distance)
    {
        return (float)(inertia + (mass * (float)Mathf.Pow(distance, 2)));
    }

    public static float MomentOfInertiaRectangle(float mass, float width, float height)
    {
        return (float)((mass * ((float)Mathf.Pow(width, 2) + (float)Mathf.Pow(height, 2))) / 12.0);
    }
}
