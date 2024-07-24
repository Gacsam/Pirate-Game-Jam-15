using System.Collections;
using UnityEngine;

public class Tower_Spawner : BaseObject
{
    // minion prefabs
    public GameObject melePrefab;
    public int minionCost = 30;

    // cooldowns
    public float CD = 2f;
    private bool canSpawnMelee = true;

    // player inventory reference
    private Inventory playerInventory;

    void Start() {
        playerInventory = GetComponent<Inventory>();
    }

    // used on button
    public bool SpawnUnit(){
        
        if(!canSpawnMelee) {return false;}
        if(playerInventory.gold < minionCost){return false;}
        if (!spawnAreaClear) return false;

        var newUnit = Instantiate(melePrefab, transform.position, Quaternion.identity);
        GameMan.CalculateSpawnPosition(ref newUnit);
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
        canSpawnMelee = false;
        yield return new WaitForSeconds(CD);
        canSpawnMelee = true;
    }


    private bool spawnAreaClear = true;

    public void OnTriggerStay2D(Collider2D collision)
    {
        spawnAreaClear = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        spawnAreaClear = true;
    }

    protected override void HandleDestruction()
    {
        // Destruction animation
        GameMan.TowerDestroyed(thisUnitSide);
    }
}
