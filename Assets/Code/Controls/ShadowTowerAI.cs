using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTowerAI : BaseObject
{
    // simulate gold etc...
    private int gold;
    private int income;
    private int minionCost;

    private bool canSpawnMele = true;
    private float CD;

    public GameObject minion;

    void Start() {
        // copy player stats
        Inventory playerInventory = GameObject.Find("Player Tower").GetComponent<Inventory>();
        gold = playerInventory.gold;
        income = playerInventory.GetIncome();

        Tower_Spawner playerTower = GameObject.Find("Player Tower").GetComponent<Tower_Spawner>();
        minionCost = playerTower.minionCost;
        CD = playerTower.CD;

        StartCoroutine(IncomeOverTime());

    }

    void Update() {
        if (canSpawnMele && gold > minionCost && spawnAreaClear){
            var spawnedMinion = Instantiate(minion,transform.position, Quaternion.identity);
            GameMan.Shadow.AddUnit(spawnedMinion.GetComponent<BaseMovingUnit>());
            gold -= minionCost;
        }
    }



    IEnumerator IncomeOverTime(){
        gold+=income;
        yield return new WaitForSeconds(1);
        StartCoroutine(IncomeOverTime());
    }

    // countdown
    IEnumerator MeleCountdown(){
        canSpawnMele = false;
        yield return new WaitForSeconds(CD);
        canSpawnMele = true;
    }

    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        GameMan.TowerDestroyed(thisUnitSide);
        Object.Destroy(this.gameObject);
    }

    private bool spawnAreaClear = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure it's a unit blocking the exit
        if (collision.GetComponent<BaseUnit>() != null)
        {
            spawnAreaClear = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        spawnAreaClear = true;
    }
}
