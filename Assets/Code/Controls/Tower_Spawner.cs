using System.Collections;
using UnityEngine;

public class Tower_Spawner : BaseObject
{
    // minion prefabs
    public GameObject melePrefab;
    public int minionCost = 30;

    // cooldowns
    public float CD = 2f;
    private bool canSpawnMele = true;

    // player inventory reference
    private Inventory playerInventory;

    void Start() {
        playerInventory = GetComponent<Inventory>();
    }

    // used on button
    public bool SpawnMeleeMinion(){
        
        if(!canSpawnMele){return false;}
        if(playerInventory.gold < minionCost){return false;}

        var newUnit = Instantiate(melePrefab, transform.position, Quaternion.identity);
        if (newUnit.GetComponent<BaseMovingUnit>().thisUnitSide == UnitSide.Alchemy) {
            GameMan.Alchemy.AddUnit(newUnit.GetComponent<BaseMovingUnit>());
        }
        else
        {
            GameMan.Shadow.AddUnit(newUnit.GetComponent<BaseMovingUnit>());
        }

        playerInventory.gold -= minionCost;
        playerInventory.UpdateGold();

        StartCoroutine(MeleCountdown());
        return true;
    }

    // countdown
    IEnumerator MeleCountdown(){
        canSpawnMele = false;
        yield return new WaitForSeconds(CD);
        canSpawnMele = true;
    }


    private bool spawnAreaClear = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        spawnAreaClear = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        spawnAreaClear = true;
    }

    [SerializeField]
    GameObject rangedPrefab;
    /// <summary>
    /// function name for GameMan to call. Checks if the tower doesn't already have a sprite on top of it.
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public bool SpawnUnit(UnitType unitType)
    {
        if (spawnAreaClear)
        {
            if(unitType == UnitType.Melee)
                return SpawnMeleeMinion();
            else if (unitType == UnitType.Ranged){
                var newChar = Instantiate(rangedPrefab, transform.position, Quaternion.identity);
                GameMan.ModifyGold(thisUnitSide, newChar.GetComponent<BaseMovingUnit>().unitCost);
                return true;
            }
            // spawn units and all that stuff based on type
            // such as type == melee then SpawnMeleeMinion()

            // can do a gold check with GameMan.GetGold(thisUnitSide)
            // return false if cannot afford it etc
        }
        return false;
    }

    protected override void HandleDestruction()
    {
        // Destruction animation
        GameMan.TowerDestroyed(thisUnitSide);
    }
}
