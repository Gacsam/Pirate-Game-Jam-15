using System.Collections;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold = 50;   // default starting amount
    public int fire = 0;
    public int arsenic = 0;
    public int moon = 0;
    public int borax = 0;

    // income player has over time (prevent stalemate)
    private int income = 3;

    private void Start() {
        if(GameMan.Alchemy.Inventory == null){
            Debug.Log("inventory null");
            GameMan.Alchemy.Inventory = this;
        }

        if(GameMan.temp != null){
            // Copy previous game data
            Debug.Log("fire: " + GameMan.temp.fire);
            gold = GameMan.temp.gold;
            fire = GameMan.temp.fire;
            arsenic = GameMan.temp.arsenic;
            moon = GameMan.temp.moon;
            borax = GameMan.temp.borax;
        }
        
        StartCoroutine(IncomeOverTime());
        UpdateButtonOpacity();
    }

    // update TMP (Gold)
    public void UpdateGold(){
    
        GameObject.Find("Gold").GetComponent<TextMeshProUGUI>().text = "Gold: " + gold.ToString();
    }

    // used by shadow tower (we want same income)
    public int GetIncome(){return income;}

    // could just access the public variables but this allows you to pass in enums :D
    public void AddInventory(ElementType element){
        if(element == ElementType.Fire){fire++;}
        else if(element == ElementType.Arsenic){arsenic++;}
        else if(element == ElementType.Moon){moon++;}
        else if(element == ElementType.Borax){borax++;}
        UpdateButtonOpacity();

    }

    public void AddInventory(ElementType element, int amount){
        // only allow addition
        if(amount < 1){return;}
        if(element == ElementType.Fire){fire+=amount;}
        else if(element == ElementType.Arsenic){arsenic+=amount;}
        else if(element == ElementType.Moon){moon+=amount;}
        else if(element == ElementType.Borax){borax+=amount;}
        UpdateButtonOpacity();

    }

    public void DeductInventory(ElementType element){
        if(element == ElementType.Fire){fire--;}
        else if(element == ElementType.Arsenic){arsenic--;}
        else if(element == ElementType.Moon){moon--;}
        else if(element == ElementType.Borax){borax--;}
        UpdateButtonOpacity();

    }

    public void DeductInventory(ElementType element, int amount){
        // only allow deduction
        if(amount > -1){return;}
        if(element == ElementType.Fire){fire+=amount;}
        else if(element == ElementType.Arsenic){arsenic+=amount;}
        else if(element == ElementType.Moon){moon+=amount;}
        else if(element == ElementType.Borax){borax+=amount;}
        UpdateButtonOpacity();

    }

    public void UpdateButtonOpacity(){
        if(fire <= 0){GameMan.fireButton.DisableShard();}
        else{GameMan.fireButton.EnableShard();}

        if(arsenic <= 0){GameMan.arsenicButton.DisableShard();}
        else{GameMan.arsenicButton.EnableShard();}

        if(moon <= 0){GameMan.moonButton.DisableShard();}
        else{GameMan.moonButton.EnableShard();}

        if(borax <= 0){GameMan.boraxButton.DisableShard();}
        else{GameMan.boraxButton.EnableShard();}
    }

    // countdown
    IEnumerator IncomeOverTime(){
        gold+=income;
        UpdateGold();
        yield return new WaitForSeconds(1);
        StartCoroutine(IncomeOverTime());
    }

}
