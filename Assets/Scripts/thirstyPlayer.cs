using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class thirstyPlayer : NetworkBehaviour
{

    // accelrations for janky movement FIX later
    float xAxi = 0, zAxi = 0;

    // how much water is lost ever 60th of a secound.
    public float dehydrationSpeed;

    // player body
    private Rigidbody myBody;

    // movement speed;
    float speed = 50000;

    // the UI eelement we use to represent the healbar
    public RectTransform healthBar;

    // current close Water srouce null if not close enough to any
    private Water waterSource;

    // player health is synched incase we want car player to be able to see at some point
    [SyncVar]
    float WaterLevel;

    void Start()
    {
        WaterLevel = 100;
        myBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // grab user input every frame
            xAxi = Input.GetAxis("Horizontal");
            zAxi = Input.GetAxis("Vertical");
        

            // drink if button pressed and water source near by
            if (Input.GetKeyDown("e") && waterSource != null)
            {

                // add all of waterSource to user water amount
                WaterLevel += waterSource.amountOfWater;

                // if we have more then 100 round it down to 100 and calculate remaining water
                if (WaterLevel > 100)
                {
                    float extraWater = WaterLevel - 100;
                    waterSource.amountOfWater = extraWater;
                    WaterLevel -= extraWater;
                }
                // Otherwise Destroy the water source and set waterSource to null
                else
                {
                    waterSource.amountOfWater = 0;
                    // TODO this may need to be replaced with despawn for online play.
                    Destroy(waterSource.gameObject);
                    waterSource = null;
                }
            }
        }
    }


    private void FixedUpdate()
    {
        // janky movement applied
        myBody.AddForce(speed * transform.forward * zAxi + speed *transform.right * xAxi);
        

        // Update the healthbar
        healthBar.anchorMax = new Vector2(WaterLevel / 100f, 0.5f);

        // decreasse waterlevels
        WaterLevel -= dehydrationSpeed;

        if (WaterLevel < 0)
        {
            //TODO GameOver
            Debug.Log("you died");
        }
    }

    // walk in to drinking range
    private void OnTriggerEnter(Collider other)
    {
        waterSource = other.gameObject.GetComponent<Water>();
        Debug.Log("I'm near water");
    }

    // walk out of drinking range
    private void OnTriggerExit(Collider other)
    {
        waterSource = null;
        Debug.Log("I'm not near water");
    }
}

