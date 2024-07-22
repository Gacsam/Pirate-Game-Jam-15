using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpposingSide
{
    // By creating a class with specific getters and setters for values, we can call get/set GameMan.Alchemy.Tower
    private BaseObject tower;
    public BaseObject Tower
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
            if(spawnedUnits.Count == 0 && Tower == null) return null;

            if (spawnedUnits.Count > 0)
            {
                return spawnedUnits[0];
            }
            else
            {
                return Tower.GetComponent<BaseObject>();
            }
        }

        set
        {
            spawnedUnits[0] = value;
        }
    }

    // Remove the earliest unit spawned, closest to opposite side
    public void UnitDied()
    {
        spawnedUnits.RemoveAt(0);
    }

    // Add newest unit to end of list
    public void AddUnit(BaseMovingUnit newUnit)
    {
        spawnedUnits.Add(newUnit);
    }

    internal void UnitSwapped(BaseObject replacedUnit, BaseObject newUnit)
    {
        var index = spawnedUnits.FindIndex(unit => unit.gameObject == replacedUnit.gameObject);
        spawnedUnits[index] = newUnit;
    }
}