using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class just makes sure to put the player in the correct location upon starting the level.
// Other scenes are built to reuse the same 

public class Brain3 : Brain
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
}
