using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpposingSide
{
    // By creating a class with specific getters and setters for values, we can call get/set GameMan.Alchemy.Tower
    private TestTower tower;
    public TestTower Tower
    {
        get
        {
            return tower;
        }
        set
        {
            tower = value;
        }
    }

    private Inventory inventory;
    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
        set
        {
            inventory = value;
        }
    }

    public List<BaseObject> spawnedUnits = new List<BaseObject>();
    // Always return the top unit of the stack (closest), if none exist, return tower
    // 
    public BaseObject ClosestUnit
    {
        get
        {
            // return null if nothing exists, this should be a game over for one of the sides
            if(spawnedUnits == null && Tower == null) return null;

            if (spawnedUnits.Count > 0)
            {
                return spawnedUnits[0];
            }
            else
            {
                return Tower.GetComponent<BaseObject>();
            }
        }
    }

    // Remove the earliest unit spawned, closest to opposite side
    public void UnitDied()
    {
        spawnedUnits.RemoveAt(0);
    }

    // Add unit to the end of the stack
    public bool CreateNewUnit(UnitType type)
    {
        // call tower to create an instance of unitType
        // something like Tower.CreateUnit(type)

        // return the gameobject spawned here
        // if gameobject is null, return false for UI stuff / can't spawn
        //if not null, add it to and return true
        spawnedUnits.Add(null);
        return false;
    }
}