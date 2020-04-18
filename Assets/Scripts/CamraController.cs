using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraController : MonoBehaviour
{
    Player currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentPlayer.isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }
}
