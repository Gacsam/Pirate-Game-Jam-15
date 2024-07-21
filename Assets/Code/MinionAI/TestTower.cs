using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Means we're using all variables of BaseObject and adding more on top of it
public class TestTower : BaseObject
{
    private bool spawnAreaClear = true;
    // since BaseObject has these two abstract classes, they're required to exist
    public override void HandleCombat()
    {
        // Put pew pew code here, or a call to turret to new pew
    }
    public override void MoveTowardsOppositeTower()
    {
        // Do nothing lol towers don't move
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        spawnAreaClear = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        spawnAreaClear = true;
    }

    // function name to hook up to GameMan
    public bool SpawnUnit(UnitType unitType)
    {
        if (spawnAreaClear)
        {
            // spawn units and all that stuff based on type
            // such as type == melee then SpawnMeleeMinion()

            // can do a gold check with GameMan.GetGold(thisUnitSide)
            // return false if cannot afford it etc
        }
        return false;
    }

    // Hook this up into GameMan
    public void Start()
    {
        if (thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.Tower = this;
        }
        else
        {
            GameMan.Shadow.Tower = this;
        }
    }
}