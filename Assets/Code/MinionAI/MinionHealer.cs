using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealer : BaseMovingUnit, IHealing, IMelee
{
    [SerializeField]
    private float healthPerSecond = 1.0f;
    [SerializeField]
    bool healEverySecond = false;
    public float HealPerSecond { get => healthPerSecond; set => healthPerSecond = value; }



    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        Object.Destroy(this.gameObject);
    }

    float healTimer = 0;
    void IHealing.HealAllyInFront(BaseObject healTarget)
    {
        if (healEverySecond)
        {
            if (healTimer == 0) healTarget.ModifyHealth(healthPerSecond);
            healTimer += Time.deltaTime;
            if (healTimer >= 1) healTimer = 0;
        }
        else healTarget.ModifyHealth(healthPerSecond * Time.deltaTime);
    }
    void IMelee.Attack()
    {
        MockAttack();
    }
}