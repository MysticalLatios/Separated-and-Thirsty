using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Rigidbody mybody;
    // Start is called before the first frame update
    void Start()
    {
        mybody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {

            if (Input.GetKeyDown("w"))
            {
                mybody.AddForce(new Vector3(100f, 0f, 0f));
            }
            else if (Input.GetKeyDown("s"))
            {
                mybody.AddForce(new Vector3(-100f, 0f, 0f));

            }
            else if (Input.GetKeyDown("a"))
            {
                mybody.AddForce(new Vector3(0f, 1000f, 0f));

            }
            else if (Input.GetKeyDown("d"))
            {
                mybody.AddForce(new Vector3(0f, -1000f, 0f));
            }
        }



    }
}
