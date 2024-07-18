using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BaseObject : MonoBehaviour
{
    public float thisUnitHealth = 1;
    public unitSide thisUnitSide = 0;
    public unitType thisUnitType = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, damageType typeOfDamage = 0)
    {
        if (typeOfDamage == damageType.standard)
        {
            thisUnitHealth -= damage;
        }
        else if (typeOfDamage == damageType.fire) { }
        else return; // ignore other damage types temporarily

        if (this.thisUnitHealth <= 0) {
            HandleDeath();
        }
    }

    // virtual to allow override
    protected virtual void HandleDeath()
    {
        // play sounds
        // play animations
        // instantiate vfx

        // destroy this unit at the very end
        if (this.thisUnitType == 0) {

        }

        // if enemy && melee || ranged
        else if (thisUnitSide == unitSide.shadow  && (thisUnitType == unitType.melee || thisUnitType == unitType.ranged))
        {
            ItemDrop.DropItem();
        }
        Object.Destroy(this.gameObject);
    }
}

public enum damageType { standard, fire, water, air, earth }
public enum unitSide { player, shadow }
public enum unitType { tower, melee, ranged, siege }