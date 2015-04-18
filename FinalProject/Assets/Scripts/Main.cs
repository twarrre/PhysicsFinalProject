﻿using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
    [SerializeField]
    private Car car;

    [SerializeField]
    private Box zeroBox;

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
        car.CheckCarCollision(zeroBox);
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


}
