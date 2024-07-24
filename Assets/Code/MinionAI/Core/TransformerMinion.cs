using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransformerMinion : BaseMovingUnit, IMelee
{
    [SerializeField]
    private int oddsOfDroppingElementsOutOfHundred = 1;
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
        if (thisUnitSide == UnitSide.Shadow)
        {
            var allShards = Resources.LoadAll<GameObject>("Prefabs/Items/Alchemy/");
            var randomNumber = Random.Range(0, 100);
            if (randomNumber < oddsOfDroppingElementsOutOfHundred * allShards.Length)
            {
                var shardToPick = randomNumber % allShards.Length;
                Instantiate(allShards[shardToPick]);
            }
        }
        Object.Destroy(this.gameObject);
    }

    [SerializeField]
    GameObject fireGuy, arsenicGuy, moonGuy, boraxGuy;
    public void Upgrade(ElementType upgradeType)
    {
        var replacedUnit = this.GetComponent<BaseUnit>();
        var newUnit = replacedUnit.gameObject;
        if (upgradeType == ElementType.Fire) { newUnit = Instantiate(fireGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Borax) { newUnit = Instantiate(boraxGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Arsenic) { newUnit = Instantiate(arsenicGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Moon) { newUnit = Instantiate(moonGuy, transform.position, Quaternion.identity); }
        // transfer all injuries
        newUnit.GetComponent<BaseUnit>().ModifyHealth(-replacedUnit.GetComponent<BaseUnit>().GetInjuryPoints());
        GameMan.CalculateSpawnPosition(ref newUnit);
        if (GetComponent<BaseObject>().thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }
        else
        {
            GameMan.Shadow.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }
        Destroy(this.unitHealthSlider.gameObject);
        Destroy(this.gameObject);
    }
}