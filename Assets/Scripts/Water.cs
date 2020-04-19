using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Water : NetworkBehaviour
{
    [SyncVar]
    public float amountOfWater;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// sets the water to the passed in value
    /// </summary>
    /// <param name="waterAmount"></param>
    public void setUpWater(int waterAmount)
    {
        amountOfWater = waterAmount;
    }


    /// <summary>
    /// sets the water to a random number between 0 and 60
    /// </summary>
    /// <param name="waterAmount"></param>
    public void setUpWater()
    {
        amountOfWater = Random.Range(20f, 50f);
    }
}
