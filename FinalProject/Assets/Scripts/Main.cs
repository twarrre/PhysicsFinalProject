using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
    [SerializeField]
    private Car car;

    [SerializeField]
    private Box zeroBox;

    //Assignment 11 collision
    Vector3 normalVec;
    float mofI1;
    float mofI2;
    Vector3 vr;
    Vector3 J; // impulse
    Vector3 omega1;
    Vector3 omega2;
    Vector3 UfN;
    Vector3 VfN;
    Vector3 r1;
    Vector3 r2;
    Vector3 contactPoint;

	// Use this for initialization
	void Start ()
    {
	}

    void OnGUI()
    {
        string label = "Car Position: " + car.transform.position;
        label        += "\nTank Position (U and I) : " + car.tank.transform.position;
        label        += "\nDriver Position (G and H) : " + car.driver.transform.position;
        label        += "\nCenter of Mass : " + car.centerOfMass.transform.position;
        label        += "\nCar Mass : " + car.carMass + " kg";
        label        += "\nTank Mass (T and Y) : " + car.tankMass + " kg";
        label        += "\nDriver Mass (D and F) : " + car.driverMass + " kg";
        label        += "\nTotal Mass  : " + car.carMass + " + " + car.tankMass + " + " + car.driverMass + " = " + (car.carMass + car.tankMass + car.driverMass) + " kg";
        label        += "\n\nForce : " + car.force + " N";
        label        += "\nVelocity : " + car.velocity + " m/s";
        label        += "\nOmega : " + car.omega + " rad/s";
        GUI.Label(new Rect(0, 0, 500, 500), label);
    }

	// Update is called once per frame
	void Update ()
    {
        car.UpdateForces();
        zeroBox.UpdateForces();
        CheckInput();
        car.UpdatePhysics();
        zeroBox.UpdatePhysics();
        
        if(CheckCarCollision(car, zeroBox))
        {
            //Debug.Log("HIT");
            /*Vector3 normal = new Vector3(1, 0, 0);
            Vector3 r1 = new Vector3() - car.transform.position;
            Vector3 r2 = new Vector3() - zeroBox.transform.position;

            Vector3 vr = CalculateVR(car.velocity, zeroBox.velocity);

            float j = CalculateJ(vr.x, zeroBox.e, car.carMass, zeroBox.mass, normal, r1, r2, car.totalInertia, zeroBox.inertia);

            car.velocity = CalculateVelocityFinal(j * normal, car.carMass, car.velocity.x, normal);
            zeroBox.velocity = CalculateVelocityFinal(-j * normal, zeroBox.mass, zeroBox.velocity.x, normal);

            car.omega = CalculateAngularVelocity(new Vector3(), r1, j * normal, car.totalInertia);
            zeroBox.omega = CalculateAngularVelocity(new Vector3(), r2, -j * normal, zeroBox.inertia);*/
        }
	}

    /*void FixedUpdate()
    {
        Vector3 uforce = Quaternion.Euler(0, 0, angle.z * Mathf.Rad2Deg) * force;
        Vector3 lforce = Quaternion.Euler(0, 0, angle.z * Mathf.Rad2Deg) * leftForce;
        //Vector3 rfource = Quaternion.Euler(0, 0, theta.z * Mathf.Rad2Deg) * rightForce;

        Vector3 com = CalculateCenterOfMass(new float[3] { carMass, tankMass, driverMass }, new Vector3[3] { car.transform.position, tank.transform.position + car.transform.position, driver.transform.position + car.transform.position });

        //calculating the induvidual momements of inertia
        float carInertia    = MomentOfInertiaRectangle(carMass, CAR_WIDTH, CAR_HEIGHT);
        float tankInertia   = MomentOfInertiaRectangle(tankMass, TANK_WIDTH, TANK_HEIGHT);
        float driverInertia = MomentOfInertiaRectangle(driverMass, DRIVER_WIDTH, DRIVER_HEIGHT);

        //calculating the distance of the object to the center of mass
        float carDistance    = Vector3.Distance(car.transform.position, com);
        float tankDistance   = Vector3.Distance(tank.transform.position + car.transform.position, com);
        float driverDistance = Vector3.Distance(driver.transform.position + car.transform.position, com);

        //calculating the inertia of each object about the center of mass
        float carInertiaAboutTheCom    = MomentOfInertiaAboutACOM(carInertia, carMass, carDistance);
        float tankInertiaAboutTheCom   = MomentOfInertiaAboutACOM(tankInertia, tankMass, tankDistance);
        float driverInertiaAboutTheCom = MomentOfInertiaAboutACOM(driverInertia, driverMass, driverDistance);

        //calculating the total inertia
        totalInertia = (carInertiaAboutTheCom + tankInertiaAboutTheCom + driverInertiaAboutTheCom);

        acceleration = (uforce + lforce) / (tankMass + driverMass + carMass); // + lforce + rfource

        radialUp    = (upForceObject.transform.position + car.transform.position) - com;
        radialLeft  = (leftForceObject.transform.position + car.transform.position) - com;
        //radialRight = (rightForceObject.transform.position + car.transform.position) - com;

        Vector3 tourqueUp    = Vector3.Cross(radialUp, uforce);
        Vector3 tourqueLeft  = Vector3.Cross(radialLeft, lforce);
        //Vector3 tourqueRight = Vector3.Cross(radialRight, rfource);

        alpha = (tourqueUp + tourqueLeft) / totalInertia;//tourqueLeft + tourqueRight
    }*/

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            car.tankMass = Mathf.Clamp(car.tankMass + 10, 0, 200);          
        if (Input.GetKeyDown(KeyCode.T))
            car.tankMass = Mathf.Clamp(car.tankMass - 10, 0, 200);
        if (Input.GetKeyDown(KeyCode.F))
            car.driverMass = Mathf.Clamp(car.driverMass + 10, 0, 200);
        if (Input.GetKeyDown(KeyCode.D))
            car.driverMass = Mathf.Clamp(car.driverMass - 10, 0, 200);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            car.force = new Vector3(0, 10000, 0);
            car.upForceObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            car.force = new Vector3(0, -10000, 0);
            car.upForceObject.SetActive(true);
        }  
        if(Input.GetKey(KeyCode.RightArrow))
        {
            car.rightForce = new Vector3(0, 5000, 0);
            car.rightForceObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            car.leftForce = new Vector3(0, 5000, 0);
            car.leftForceObject.SetActive(true);
        }
    }

    /*
            for (int i = 0; i < theCar.corners.Length; i++ )
            {
                for(int j = 0; j < theBox.corners.Length; j++)
                {
                    Vector3 p = theCar.corners[i] - theBox.corners[j];

                    int index = j + 1;
                    if (j == (theBox.corners.Length - 1))
                        index = 0;

                    Vector3 uVec = theBox.corners[index] - theBox.corners[j];
                    Vector3 u = Vector3.Normalize(uVec);

                    float projection = Vector3.Dot(p, u);
                    float d = Vector3.Cross(p, u).magnitude;
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(Vector3.Cross(u, p), u));

                    float vrn = Vector3.Dot(theCar.velocity - theBox.velocity, normal);

                    Debug.Log("projection > 0.0f: " + (projection > 0.0f));
                    Debug.Log("projection < uVec.magnitude: " + (projection < uVec.magnitude));
                    Debug.Log("d < 0.1f: " + (d < 0.1f));
                    Debug.Log("vrn < 0: " + (vrn < 0.0f));
                    if(projection > 0.0f && projection < uVec.magnitude && d < 0.1f && vrn < 0.0f)
                    {
                        return true;
                    }
                }
            }
            return false;
     */

    void CubeCollisionResponse(ref Car c, ref Box b, Vector3 Ui, Vector3 Vi)
    {
        normalVec = Vector3.Normalize(normalVec);
        r1 = new Vector3(contactPoint.x - c.transform.position.x, contactPoint.y - c.transform.position.y, contactPoint.z - c.transform.position.z);
        r2 = new Vector3(contactPoint.x - b.transform.position.x, contactPoint.y - b.transform.position.y, contactPoint.z - b.transform.position.z);
        mofI1 = 1.0f / 12.0f * c.carMass * ((Mathf.Pow(c.transform.lossyScale.x, 2.0f) + Mathf.Pow(c.transform.lossyScale.y, 2.0f)));
        mofI2 = 1.0f / 12.0f * b.mass * ((Mathf.Pow(b.transform.lossyScale.x, 2.0f) + Mathf.Pow(b.transform.lossyScale.y, 2.0f)));
        vr = (Ui - Vi);
        J = -vr * (b.e + 1.0f) * (1.0f / ((1.0f / c.carMass) + (1.0f / b.mass) + Vector3.Dot(normalVec, Vector3.Cross(Vector3.Cross(r1, normalVec) / mofI1, r1)) + Vector3.Dot(normalVec, Vector3.Cross(Vector3.Cross(r2, normalVec) / mofI2, r2))));
        omega1 = omega1 + (Vector3.Cross(r1, (J.x * normalVec)) / mofI1) + (Vector3.Cross(r1, (J.y * normalVec)) / mofI1) + (Vector3.Cross(r1, (J.z * normalVec)) / mofI1);
        omega2 = omega2 + (Vector3.Cross(r2, ((-J.x) * normalVec)) / mofI2) + (Vector3.Cross(r2, ((-J.y) * normalVec)) / mofI2) + (Vector3.Cross(r2, ((-J.z) * normalVec)) / mofI2);
        UfN.x = ((J.x * normalVec.x) / c.carMass) + (Ui.x * normalVec.x);
        UfN.y = ((J.y * normalVec.y) / c.carMass) + (Ui.y * normalVec.y);
        UfN.z = 0;
        VfN.x = (((-J.x) * normalVec.x) / b.mass) + (Vi.x * normalVec.x);
        VfN.y = (((-J.y) * normalVec.y) / b.mass) + (Vi.y * normalVec.y);
        VfN.z = 0;
        c.velocity = UfN;
        b.velocity = VfN;
    }

    public bool CheckCarCollision(Car theCar, Box theBox)
    {
        if (Vector3.Distance(theCar.transform.position, theBox.transform.position) <= theCar.radius + theBox.radius)
        {
            float clipDist;
            normalVec = NarrowFace(theCar, theBox, out clipDist);
            if (clipDist != float.MaxValue)
            {
                Debug.Log("HIT");
                CubeCollisionResponse(ref car, ref zeroBox, car.velocity, zeroBox.velocity);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 NarrowFace(Car theCar, Box theBox, out float distance)
    {
        ArrayList box1InBox2 = new ArrayList();
        ArrayList box2InBox1 = new ArrayList();

        Vector2[] edges = new Vector2[4];
        edges[0] = theBox.corners[1] - theBox.corners[0];
        edges[1] = theBox.corners[2] - theBox.corners[1];
        edges[2] = theBox.corners[3] - theBox.corners[2];
        edges[3] = theBox.corners[0] - theBox.corners[3];

        Vector2[] normals = new Vector2[4];
        normals[0] = new Vector2(-edges[0].y, edges[0].x);
        normals[1] = new Vector2(-edges[1].y, edges[1].x);
        normals[2] = new Vector2(-edges[2].y, edges[2].x);
        normals[3] = new Vector2(-edges[3].y, edges[3].x);



        for(int i = 0; i < theCar.corners.Length; i++)
        {
            bool[] isInside = new bool[4];
            for(int j = 0; j < edges.Length; j++)
            {
                float dp = Vector2.Dot(normals[j], theCar.corners[i] - theBox.corners[j]);

                if (dp < 0.0f)
                {
                    isInside[j] = true;
                }
            }
            if (isInside[0] && isInside[1] && isInside[2] && isInside[3])
            {
                box1InBox2.Add(theCar.corners[i]);
            }
        }


        Vector2[] edges_2 = new Vector2[4];
        edges_2[0] = theCar.corners[1] - theCar.corners[0];
        edges_2[1] = theCar.corners[2] - theCar.corners[1];
        edges_2[2] = theCar.corners[3] - theCar.corners[2];
        edges_2[3] = theCar.corners[0] - theCar.corners[3];

        Vector2[] normals_2 = new Vector2[4];

        normals_2[0] = new Vector2(-edges_2[0].y, edges_2[0].x);
        normals_2[1] = new Vector2(-edges_2[1].y, edges_2[1].x);
        normals_2[2] = new Vector2(-edges_2[2].y, edges_2[2].x);
        normals_2[3] = new Vector2(-edges_2[3].y, edges_2[3].x);


         for (int i = 0; i < theBox.corners.Length; i++)
         {
             bool[] isInside = new bool[4];
             for (int j = 0; j < edges_2.Length; j++)
             {
                 float dp = Vector2.Dot(normals_2[j], theBox.corners[i] - theCar.corners[j]);
                 if (dp < 0.0f)
                 {
                     isInside[j] = true;
                 }
             }
             if (isInside[0] && isInside[1] && isInside[2] && isInside[3])
             {
                 box2InBox1.Add(theBox.corners[i]);
             }
         }


        if (box1InBox2.Count <= 0 && box2InBox1.Count <= 0)
        {
            distance = float.MaxValue;
            return new Vector3();
        }

        float minDist1 = float.MaxValue;
        float minDist2 = float.MaxValue;

        Vector3 minEdge1 = new Vector3();
        Vector3 minEdge2 = new Vector3();

        for(int i = 0; i < edges.Length; i++)
        {
            float maxDistE2 = float.MaxValue;
            for (int j = 0; j < box1InBox2.Count; j++)
            {
                Vector3 vec = (Vector3)(box1InBox2[j]);
                float dist = Vector3.Dot(vec, new Vector3(edges[i].x, edges[i].y, vec.z));
                if(dist < 0 && dist < maxDistE2)
                {
                    maxDistE2 = dist;
                    Debug.Log("FUCK1");
                }
                    
            }

            if(maxDistE2 < minDist1)
            {
                minDist1 = maxDistE2;
                minEdge1 = edges[i];
            }
        }

        for (int i = 0; i < edges_2.Length; i++)
        {
            float maxDistE2 = float.MaxValue;
            for (int j = 0; j < box2InBox1.Count; j++)
            {
                Vector3 vec = (Vector3)(box2InBox1[j]);
                float dist = Vector3.Dot(vec, new Vector3(edges_2[i].x, edges_2[i].y, vec.z));
                if (dist < 0 && dist < maxDistE2)
                {
                    maxDistE2 = dist;
                    Debug.Log("FUCK2");
                }
                    
            }
            if (maxDistE2 < minDist2)
            {
                minDist2 = maxDistE2;
                minEdge2 = edges[i];
            }
        }

        float clipDist = 0;
        Vector3 normal;
        if(minDist1 < minDist2)
        {
            clipDist = minDist1;
            normal = new Vector3(minEdge1.y, -minEdge1.x, minEdge1.z);
        }
        else
        {
            clipDist = minDist2;
            normal = new Vector3(minEdge2.y, -minEdge2.x, minEdge2.z);
        }

        if (box1InBox2.Count > 0)
        {
            contactPoint = (Vector3)box1InBox2[0];
        }
        else if (box2InBox1.Count > 0)
        {
            contactPoint = (Vector3)box2InBox1[0];
        }
        distance = clipDist;
        return normal;
    }

    public static Vector3 CalculateVR(Vector3 uInitial, Vector3 vInital)
    {
        return uInitial - vInital;
    }

    public static float CalculateJ(float vr, float e, float mass1, float mass2, Vector3 normal, Vector3 r1, Vector3 r2, float inertia1, float inertia2)
    {
        float firstPart = Vector3.Dot(normal, CalculateJPart(r1, normal, inertia1));
        float secondPart = Vector3.Dot(normal, CalculateJPart(r2, normal, inertia2));

        return (-vr) * (e + 1.0f) * (1.0f / ((1.0f / mass1) + (1.0f / mass2) + firstPart + secondPart));
    }

    public static Vector3 CalculateJPart(Vector3 r, Vector3 normal, float inertia)
    {
        return Vector3.Cross(Vector3.Cross(r, normal) / inertia, r);
    }

    public static Vector3 CalculateVelocityFinal(Vector3 jn, float mass, float velix, Vector3 normal)
    {
        return (jn / mass) + (velix * normal);
    }

    public static Vector3 CalculateAngularVelocity(Vector3 angularInital, Vector3 r, Vector3 jn, float inertia)
    {
        return angularInital + (Vector3.Cross(r, jn) / inertia);
    }

    public static Vector3 CalculateAngularDisplacement(Vector3 omega, float time)
    {
        return (omega * time);
    }
}
