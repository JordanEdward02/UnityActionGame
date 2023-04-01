using UnityEngine;
using UnityEngine.SceneManagement;

// Interface for all the objects which the player can possibly pick up.
public interface Object
{
    bool CanBePicked();

    //Throws the Object with the given power relative to the parent player.
    void Use(PlayerInteractions player, float power);
    void Drop();

    //Moves the object to the player
    void UpdateObject(PlayerInteractions parent);

    GameObject GetObject();
}

// How the player interacts with object they are currently holding.

public class PlayerObject
{
    Object currentObject;
    PlayerInteractions parent;


    public PlayerObject(PlayerInteractions parent)
    {
        this.parent = parent;
    }

    public bool HoldingObject()
    {
        return currentObject != null;
    }

    public void Use(float power)
    {
        if (currentObject != null)
        {
            currentObject.Use(parent, power);
        }
    }

    public void ObjectDisconnect()
    {
        currentObject = null;
    }

    public void Drop()
    {
        if (currentObject != null)
        {
            currentObject.Drop();
            currentObject = null;
        }
    }

    public bool PickUp(GameObject newObject)
    {
        // If an object is picked up and doesn't destroy itself, it needs to be manually destroyed by the level
        // controller when it finishes.
        if (currentObject == null && newObject.GetComponent<Object>().CanBePicked())
        {
            currentObject = newObject.GetComponent<Object>();
            UnityEngine.Object.DontDestroyOnLoad(newObject);
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (currentObject != null) currentObject.UpdateObject(parent);
    }

    public void Destroy()
    {
        try
        {
            if (currentObject.GetObject() != null)
                GameObject.Destroy(currentObject.GetObject());
        }
        catch
        {
            return;
        }
    }
}
