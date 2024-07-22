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
            if (value is BaseObject baseObjectData && baseObjectData.thisUnitType == UnitType.Tower) {
                tower = value;
            }else Debug.Log("Apparently not a tower");
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

    // Heal closest ally (infront and back)(used by borax)
    public void HealCloseAllies(BaseObject targetUnit){

        float healAmount = 0.5f;

        if(spawnedUnits.Count == 0){return;}


        int index = spawnedUnits.FindIndex(unit => unit.gameObject == targetUnit.gameObject);
        if(spawnedUnits.Count == 1){spawnedUnits[0].ModifyHealth(healAmount);}

        // unit at front, so only heal self and back
        else if(index == 0){
            spawnedUnits[0].ModifyHealth(healAmount);
            spawnedUnits[1].ModifyHealth(healAmount);
        
        }

        // unit at back, so heal self and front
        else if(index == spawnedUnits.Count-1){
            spawnedUnits[index].ModifyHealth(healAmount);
            spawnedUnits[index-1].ModifyHealth(healAmount);
            
        }

        // unit at middle, so heal self, front and back
        else if(index == spawnedUnits.Count-1){
            spawnedUnits[index].ModifyHealth(healAmount);
            spawnedUnits[index-1].ModifyHealth(healAmount);
            spawnedUnits[index+1].ModifyHealth(healAmount);

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