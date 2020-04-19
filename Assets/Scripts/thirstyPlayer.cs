using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class thirstyPlayer : NetworkBehaviour
{
    float xAxi = 0, zAxi = 0;
    private Rigidbody myBody;
    float speed = 400;
    public RectTransform healthBar;
    private Water waterSource;
    private bool Drinking;


    [SyncVar]
    float WaterLevel;

    // Start is called before the first frame update
    void Start()
    {
        WaterLevel = 100;
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isLocalPlayer)
        {
            xAxi = Input.GetAxis("Horizontal");
            zAxi = Input.GetAxis("Vertical");
        }
        if (Input.GetKeyDown("e") && waterSource != null)
        {
            WaterLevel += waterSource.amountOfWater;
            if (WaterLevel > 100)
            {
                float extraWater = WaterLevel - 100;
                waterSource.amountOfWater = extraWater;
                WaterLevel -= extraWater;
            }
            else
            {
                waterSource.amountOfWater = 0;
                Destroy(waterSource.gameObject);
                waterSource = null;
            }
        }
    }



    private void FixedUpdate()
    {
        myBody.AddForce(speed * xAxi, 0f, zAxi * speed);

        healthBar.anchorMax = new Vector2(WaterLevel / 100f, 0.5f);

        WaterLevel -= 0.01f;

        if (WaterLevel < 0)
        {
            //GameOver
            Debug.Log("you died");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        waterSource = other.gameObject.GetComponent<Water>();
        Debug.Log("I'm near water");
    }

    private void OnTriggerExit(Collider other)
    {
        waterSource = null;
        Debug.Log("I'm not near water");
    }
}

