using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain3 : BrainClass
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject player;
        if (player = GameObject.Find("Player(Clone)"))
        {
            player.transform.position = playerSpawn.position;
            player.transform.rotation = playerSpawn.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
