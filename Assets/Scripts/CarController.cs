using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : NetworkBehaviour
{
    //Inputs
    private float FBInput; //Foward Back input
    private float LRInput; //left Right input

    //Where the car is and how fast its trying to go
    private float DirectionAngle; //where the car is curently pointing
    private float CurentTorque; //What the motor is outputing to the wheels

    private bool flip;

    //Wheel colliders
    public WheelCollider FrontLeftWheel, FrontRightWheel;
    public WheelCollider BackLeftWheel, BackRightWheel;

    //Locations of the wheels
    public Transform FrontLeftTrans, FrontRightTrans;
    public Transform BackLeftTrans, BackRightTrans;

    //Vars to ajust for tuinning
    public float maxTurnAngle;
    public float maxMotorForce;

    public void GetPlayerInput()
    {
        LRInput = Input.GetAxis("Horizontal");   
        FBInput = Input.GetAxis("Vertical");  
        if (Input.GetKeyDown(KeyCode.E))
        {
            flip = true;
        } 
    }

    public void UpdateTurn()
    {
        DirectionAngle = maxTurnAngle * LRInput; //Set the wheel angle to the input
        FrontLeftWheel.steerAngle = DirectionAngle;
        FrontRightWheel.steerAngle = DirectionAngle;
    }

    public void UpdateMotor()
    {
        //Update torque
        CurentTorque = FBInput * maxMotorForce * -1;

        FrontRightWheel.motorTorque = CurentTorque;
        FrontLeftWheel.motorTorque = CurentTorque;
        BackLeftWheel.motorTorque = CurentTorque;
        BackRightWheel.motorTorque = CurentTorque;
    }

    private void UpdateWheels()
    {
        UpdateWheel(FrontRightWheel, FrontRightTrans);
        UpdateWheel(FrontLeftWheel, FrontLeftTrans);
        UpdateWheel(BackRightWheel, BackRightTrans);
        UpdateWheel(BackLeftWheel, BackLeftTrans);
    }

    private void UpdateWheel(WheelCollider collider_in, Transform trans_in)
    {
        Vector3 OriginPos = trans_in.position;
        Quaternion OriginQuat = trans_in.rotation;

        //TODO: Set the wheel height to the terrain under it.
        //For now it will set to worldpos?

        //Get new values for the wheels from the world, wtf c sharp why do it like this
        collider_in.GetWorldPose(out OriginPos, out OriginQuat);

        trans_in.position = OriginPos;
        trans_in.rotation = OriginQuat;

        //fix wheel being 90 degress of

        trans_in.rotation *= Quaternion.Euler(0, 0, 90);
    }

    // Fixed update for physics
    void FixedUpdate()
    {
        if (transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
        { 
            GetPlayerInput();
            UpdateTurn();
            UpdateMotor();
            UpdateWheels();
            if (transform.position.y < -80f)
            {
                transform.position = new Vector3 (0f,5f, 0f);
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (flip)
            {
                flip = false;
                transform.SetPositionAndRotation(transform.position + new Vector3(0f,2f,0f), Quaternion.identity);
            }
        }

    }

    void Start()
    {
        if (transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
        { 
            Debug.Log("You have Authority over a car");
        }
    }
}
