using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseObject : MonoBehaviour
{
    public float thisUnitHealth = 1;
    public unitSide thisUnitSide = 0;

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
        if (typeOfDamage == damageType.standard) {
            thisUnitHealth -= damage;
        }
        else if (typeOfDamage == damageType.fire) { }
        else return;
    }
}

public enum damageType { standard, fire, water, air, earth }
public enum unitSide { player, shadow }