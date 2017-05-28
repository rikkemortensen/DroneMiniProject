using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour {

    Rigidbody myDrone;

    void Awake()
    {
        myDrone = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Rotation();
        ClampingSpeedValues();
        Swerve();

        myDrone.AddRelativeForce(Vector3.up * upForce);
        myDrone.rotation = Quaternion.Euler(new Vector3(tiltAmoundForward, currentYRotation, tiltAmountSideways));
    }

    public float upForce;

    void MovementUpDown()
    {
        if((Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f))
        {
            if(Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
            {
                myDrone.velocity = myDrone.velocity;
            }
            if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                myDrone.velocity = new Vector3(myDrone.velocity.x, Mathf.Lerp(myDrone.velocity.y, 0, Time.deltaTime * 5), myDrone.velocity.z);
                upForce = 281;
            }
            if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                myDrone.velocity = new Vector3(myDrone.velocity.x, Mathf.Lerp(myDrone.velocity.y, 0, Time.deltaTime * 5), myDrone.velocity.z);
                upForce = 110;
            }
            if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                upForce = 410;
            }
        }

        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            upForce = 135;
        }

        if(Input.GetKey(KeyCode.I))
        {
            upForce = 450;
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce = 500;
            }
        }
        else if(Input.GetKey(KeyCode.K))
        {
            upForce = -200;
        }
        else if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            upForce = 98.1f;
        }   
    }

    private float movementForwardSpeed = 500.00f;
    private float tiltAmoundForward = 0;
    private float tiltVelocityForward; 

    void MovementForward()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            myDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            tiltAmoundForward = Mathf.SmoothDamp(tiltAmoundForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        }
    }

    private float wantToRotate;
    [HideInInspector] public float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;

    void Rotation()
    {
        if(Input.GetKey(KeyCode.J))
        {
            wantToRotate -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantToRotate += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantToRotate, ref rotationYVelocity, 0.25f);
    }

    private Vector3 VelocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            myDrone.velocity = Vector3.ClampMagnitude(myDrone.velocity, Mathf.Lerp(myDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            myDrone.velocity = Vector3.ClampMagnitude(myDrone.velocity, Mathf.Lerp(myDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            myDrone.velocity = Vector3.ClampMagnitude(myDrone.velocity, Mathf.Lerp(myDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            myDrone.velocity = Vector3.SmoothDamp(myDrone.velocity, Vector3.zero, ref VelocityToSmoothDampToZero, 0.95f);
        }
    }

    private float SideMovementAmount = 300.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;

    void Swerve()
    {
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            myDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * SideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20 * Input.GetAxis("Horizontal"), ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }
    }
}
