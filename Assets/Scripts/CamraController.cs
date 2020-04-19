using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraController : MonoBehaviour
{
    Transform carTransform;
    Transform cameraTransform;

    //Publics
    public Camera playerCamera;
    public Vector3 followoffset;
    public float followspeed;
    public float lookspeed;


    public void LookAtPlayer()
    {
        //This is jank I have no idea if this will work
        Vector3 DirectionToPlayer = carTransform.position - cameraTransform.position;
        Quaternion DirectionDetla = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);

        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, DirectionDetla, lookspeed * Time.deltaTime);
    }

    public void MoveToPlayer()
    {
        Vector3 PlayerPosition = carTransform.position + carTransform.forward * followoffset.z + carTransform.right * followoffset.x + carTransform.up * followoffset.y;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, PlayerPosition, followspeed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        carTransform = transform;
        cameraTransform = playerCamera.GetComponent<Transform>();

        //disable the camera and audio for the server
        if (!transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        MoveToPlayer();
    }
}
