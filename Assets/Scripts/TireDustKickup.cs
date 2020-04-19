using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireDustKickup : MonoBehaviour
{
    //Particle systems
    public ParticleSystem FrontLeft;
    public ParticleSystem FrontRight;
    public ParticleSystem BackLeft;
    public ParticleSystem BackRight;

    //Emitters themself
    private ParticleSystem.EmissionModule FrontLeftEmitt;
    private ParticleSystem.EmissionModule FrontRightEmitt;
    private ParticleSystem.EmissionModule BackLeftEmitt;
    private ParticleSystem.EmissionModule BackRightEmitt;


    //Wheel Colliders for getting rpm
    public WheelCollider WheelFrontLeft;
    public WheelCollider WheelFrontRight;
    public WheelCollider WheelBackLeft;
    public WheelCollider WheelBackRight;

    public float EmittRate;

    private void updateFrontAngle()
    {
        UpdateAngle(WheelFrontLeft, FrontLeft.transform);
        UpdateAngle(WheelFrontRight, FrontRight.transform);
    }

    private void UpdateAngle(WheelCollider collider_in, Transform trans_in)
    {
        Vector3 OriginPos = trans_in.position;
        Quaternion OriginQuat = trans_in.rotation;

        //Get new values for the wheels from the world, wtf c sharp why do it like this
        collider_in.GetWorldPose(out OriginPos, out OriginQuat);

        trans_in.position = OriginPos;
        trans_in.rotation = OriginQuat;

        //TODO, stop it from rotating

    }

    private void updateRate()
    {
        FrontLeftEmitt.rateOverTimeMultiplier = WheelFrontLeft.rpm * EmittRate;
        FrontRightEmitt.rateOverTimeMultiplier = WheelFrontRight.rpm * EmittRate;
        BackLeftEmitt.rateOverTimeMultiplier = WheelBackLeft.rpm * EmittRate;
        BackRightEmitt.rateOverTimeMultiplier = WheelBackRight.rpm * EmittRate;
    }

    void FixedUpdate()
    {
        if(transform.parent.parent.GetComponent<NetworkIdentity>().hasAuthority)
        {
            updateFrontAngle();
            updateRate();
        }
    }

    void start()
    {
        if(transform.parent.parent.GetComponent<NetworkIdentity>().hasAuthority)
        {
            FrontLeftEmitt = FrontLeft.emission;
            FrontRightEmitt = FrontRight.emission;
            BackLeftEmitt = BackLeft.emission;
            BackRightEmitt = BackRight.emission;
        }
    }
}
