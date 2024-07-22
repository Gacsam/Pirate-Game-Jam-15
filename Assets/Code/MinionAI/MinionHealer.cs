using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealer : BaseMovingUnit, IHealing, IMelee
{
    [SerializeField]
    private float healPerSecond = 1.0f;
    public float HealPerSecond { get => healPerSecond; set => healPerSecond = value; }



    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        Object.Destroy(this.gameObject);
    }

    void IHealing.HealAllyInFront(BaseObject healTarget)
    {
        healTarget.ModifyHealth(healPerSecond * Time.deltaTime);
    }
    void IMelee.Attack()
    {
        MockAttack();
    }
}
