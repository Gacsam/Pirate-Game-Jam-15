using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTowerAI : MonoBehaviour
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
        if (canSpawnMele && gold > minionCost){
            Instantiate(minion,transform.position, Quaternion.identity);
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
}
