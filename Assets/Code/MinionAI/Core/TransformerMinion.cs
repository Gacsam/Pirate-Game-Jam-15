using UnityEngine;

public class TransformerMinion : BaseMovingUnit, IMelee
{
    [SerializeField]
    private int chanceOfDroppingElement = 1;
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
            var allShards = Resources.LoadAll<GameObject>("Prefabs/Items/Drop/");
            var randomNumber = Random.Range(0, 100);
            if (randomNumber < chanceOfDroppingElement)
            {
                var shardToPick = randomNumber % allShards.Length;
                Instantiate(allShards[shardToPick]);
                ItemDrop.DropItem(thisElementType); // just gonna drop gold
            }
        }
        // add gold for shadow
        else if (thisUnitSide == UnitSide.Alchemy){
            GameObject.Find("Shadow Tower").GetComponent<ShadowTowerAI>().gold += ItemDrop.goldDrop/2;  // too hard to win if it scales the same with player 
        }
        
        Object.Destroy(this.gameObject);
    }

    [SerializeField]
    GameObject fireGuy, arsenicGuy, moonGuy, boraxGuy;
    public void Upgrade(ElementType upgradeType)
    {
        if (thisUnitSide == UnitSide.Shadow) return;

        var replacedUnit = this.GetComponent<BaseUnit>();
        var newUnit = replacedUnit.gameObject;
        if (upgradeType == ElementType.Fire) { newUnit = Instantiate(fireGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Borax) { newUnit = Instantiate(boraxGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Arsenic) { newUnit = Instantiate(arsenicGuy, transform.position, Quaternion.identity); }
        else if (upgradeType == ElementType.Moon) { newUnit = Instantiate(moonGuy, transform.position, Quaternion.identity); }
        // transfer all injuries
        if(newUnit.TryGetComponent<BaseUnit>(out var theUnitData))
        {
            theUnitData.ModifyHealth(-replacedUnit.GetComponent<BaseUnit>().GetInjuryPoints());
            var difference = replacedUnit.GetComponent<SpriteRenderer>().sprite.bounds.size.y - theUnitData.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            transform.position += difference / 2 * Vector3.up;
        }
        // GameMan.CalculateSpawnPosition(ref newUnit);
        if (GetComponent<BaseObject>().thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }
        else
        {
            GameMan.Shadow.UnitUpgraded(replacedUnit, newUnit.GetComponent<BaseObject>());
        }

        GameMan.Alchemy.Inventory.DeductInventory(upgradeType);
        Destroy(this.unitHealthSlider.gameObject);
        Destroy(this.gameObject);
    }
}