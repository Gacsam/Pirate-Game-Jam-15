using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Means we're using all variables of BaseObject and adding more on top of it
public class TestTower : BaseObject
{
    // since BaseObject has these two abstract classes, they're required to exist
    public override void HandleCombat()
    {
        // Put pew pew code here, or a call to turret to new pew
    }
    public override void MoveTowardsOppositeTower()
    {
        // Do nothing lol towers don't move
    }

    // function name to hook up to GameMan
    public bool SpawnUnit(UnitType unitType)
    {
        // spawn units and all that stuff based on type
        // such as type == melee then SpawnMeleeMinion()

        // can do a gold check with GameMan.GetGold(thisUnitSide)
        // return false if cannot afford it etc
        return true;
    }

    // Hook this up into GameMan
    public void Start()
    {
        if (thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.Tower = this.GetComponent<TestTower>();
        }
        else
        {
            GameMan.Shadow.Tower = this.GetComponent<TestTower>();
        }
    }
}