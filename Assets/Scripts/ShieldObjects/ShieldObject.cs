using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Shield
{
    bool CanBePicked();
    // Toggles if the user is blocking with the shield
    // Moves the shield to the player's location
    void UpdateShield(PlayerInteractions player);
    // Throws the shield
    void Use(PlayerInteractions player, float power);
    GameObject GetShield();
}

public class ShieldObject
{
    public Shield shield;
    public PlayerInteractions parent;

    public ShieldObject(PlayerInteractions newParent)
    {
        parent = newParent;
    }

    public void UpdateShield()
    {
        if (shield != null) shield.UpdateShield(parent);
    }
    public bool PickUp(GameObject newObject)
    {
        // If an object is picked up and doesn't destroy itself, it needs to be manually destroyed by the level
        // controller when it finishes.
        if (shield == null && newObject.GetComponent<Shield>().CanBePicked())
        {
            shield = newObject.GetComponent<Shield>();
            UnityEngine.Object.DontDestroyOnLoad(newObject);
            return true;
        }
        return false;
    }
    public void Use(float power)
    {
        if (shield != null) shield.Use(parent,power);
    }

    public bool HoldingShield()
    {
        return shield != null;
    }

    public void ShieldDisconnect()
    {
        shield = null;
    }

    public void Destroy()
    {
        try
        {
            if (shield.GetShield() != null) GameObject.Destroy(shield.GetShield());
        }
        catch { return; }
    }
}
