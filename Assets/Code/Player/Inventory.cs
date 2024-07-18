using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold = 0;
    public int fire = 0;
    public int water = 0;
    public int air = 0;
    public int earth = 0;


    public void AddInventory(itemType item){
        if(item == itemType.gold){gold++;}
        else if(item == itemType.fire){fire++;}
        else if(item == itemType.water){water++;}
        else if(item == itemType.air){air++;}
        else if(item == itemType.earth){earth++;}
    }

    public void AddInventory(itemType item, int amount){
        // only allow addition
        if(amount < 1){return;}
        if(item == itemType.gold){gold+=amount;}
        else if(item == itemType.fire){fire+=amount;}
        else if(item == itemType.water){water+=amount;}
        else if(item == itemType.air){air+=amount;}
        else if(item == itemType.earth){earth+=amount;}
    }

    public void DeductInventory(itemType item){
        if(item == itemType.gold){gold++;}
        else if(item == itemType.fire){fire++;}
        else if(item == itemType.water){water++;}
        else if(item == itemType.air){air++;}
        else if(item == itemType.earth){earth++;}
    }

    public void DeductInventory(itemType item, int amount){
        // only allow deduction
        if(amount > -1){return;}
        if(item == itemType.gold){gold+=amount;}
        else if(item == itemType.fire){fire+=amount;}
        else if(item == itemType.water){water+=amount;}
        else if(item == itemType.air){air+=amount;}
        else if(item == itemType.earth){earth+=amount;}
    }
}
