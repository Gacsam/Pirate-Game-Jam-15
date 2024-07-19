using System.Collections;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold = 50;   // default starting amount
    public int fire = 0;
    public int water = 0;
    public int air = 0;
    public int earth = 0;

    // income player has over time (prevent stalemate)
    private int income = 5;


    private TextMeshProUGUI goldTMP;

    void Start() {
        goldTMP = GameObject.Find("Gold").GetComponent<TextMeshProUGUI>();
        StartCoroutine(IncomeOverTime());
    }

    // update TMP (Gold)
    public void UpdateGold(){goldTMP.text = "Gold: " + gold.ToString();}

    // used by shadow tower (we want same income)
    public int GetIncome(){return income;}

    // could just access the public variables but this allows you to pass in enums :D
    public void AddInventory(itemType item){
        if(item == itemType.gold){gold++;}
        else if(item == itemType.fire){fire++;}
        else if(item == itemType.water){water++;}
        else if(item == itemType.air){air++;}
        else if(item == itemType.earth){earth++;}
        UpdateGold();

    }

    public void AddInventory(itemType item, int amount){
        // only allow addition
        if(amount < 1){return;}
        if(item == itemType.gold){gold+=amount;}
        else if(item == itemType.fire){fire+=amount;}
        else if(item == itemType.water){water+=amount;}
        else if(item == itemType.air){air+=amount;}
        else if(item == itemType.earth){earth+=amount;}
        UpdateGold();

    }

    public void DeductInventory(itemType item){
        if(item == itemType.gold){gold++;}
        else if(item == itemType.fire){fire++;}
        else if(item == itemType.water){water++;}
        else if(item == itemType.air){air++;}
        else if(item == itemType.earth){earth++;}
        UpdateGold();

    }

    public void DeductInventory(itemType item, int amount){
        // only allow deduction
        if(amount > -1){return;}
        if(item == itemType.gold){gold+=amount;}
        else if(item == itemType.fire){fire+=amount;}
        else if(item == itemType.water){water+=amount;}
        else if(item == itemType.air){air+=amount;}
        else if(item == itemType.earth){earth+=amount;}
        UpdateGold();

    }

    // countdown
    IEnumerator IncomeOverTime(){
        gold+=income;
        UpdateGold();
        yield return new WaitForSeconds(1);
        StartCoroutine(IncomeOverTime());
    }

}
