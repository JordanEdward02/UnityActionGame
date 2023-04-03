using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Need to assign the key to one of the player objects after a number of the archers are killed.
// Also add all of the throwable objects to the scene. Like rocks on the floor, sticks/stumps
public class Brain5 : Brain
{
    int archerCount = 0;
    int killedArchers = 0;

    private void Update()
    {
        int newCount = FindObjectsOfType<ArcherController>().Length;
        if (newCount > archerCount)
            archerCount = newCount;
        if (newCount < archerCount)
        {
            ++killedArchers;
            archerCount = newCount;
        }
    }
}
