using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTransformer : BaseMovingUnit, IMelee
{
    void IMelee.Attack()
    {
        MockAttack();
    }

    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        Object.Destroy(this.gameObject);
    }

    [SerializeField]
    GameObject fireGuy, arsenicGuy, moonGuy, boraxGuy;
    public void Upgrade(DamageType upgradeType)
    {
        var replacedUnit = this.GetComponent<BaseUnit>();
        var newUnit = replacedUnit.gameObject;
        if (upgradeType == DamageType.Fire) { newUnit = Instantiate(fireGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == DamageType.Borax) { newUnit = Instantiate(boraxGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == DamageType.Arsenic) { newUnit = Instantiate(arsenicGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == DamageType.Moon) { newUnit = Instantiate(moonGuy, transform.position, Quaternion.identity); }
        // Set the health to be same as before
        newUnit.GetComponent<BaseUnit>().ModifyHealth(-replacedUnit.GetComponent<BaseUnit>().GetInjuryPoints());
        var oldSize = replacedUnit.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        var newSize = newUnit.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        newUnit.transform.position += Vector3.up * Mathf.Abs(oldSize - newSize);
        if (GetComponent<BaseObject>().thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }
        else
        {
            GameMan.Shadow.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }
        Destroy(unitHealthSlider.gameObject);
        Destroy(replacedUnit.gameObject);
    }
}