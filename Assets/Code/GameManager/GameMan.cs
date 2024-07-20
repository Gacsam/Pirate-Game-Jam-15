using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameMan : MonoBehaviour
{
    // GameMananager singleton to track all the variables that we need accessible
    private static GameMan instance;
    // Having an "Instance" allows us to call GameMan.X rather than GameMan.instance.X
    public static GameMan Instance
    {
        get
        {
            // Find self and set as instance, if the "spot" is already taken throw a log
            if (instance == null)
            {
                instance = FindObjectOfType<GameMan>();
            }
            else
            {                
                // idk go boom
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(instance != this){
            Destroy(this.gameObject);
        }

        if(alchemy == null)
        {
            alchemy = new OpposingSide();
        }
        if (shadow == null)
        {
            shadow = new OpposingSide();
        }
    }
        
    // Similarly, by creating an OppositeSide class, we can refer to these sides outside the GameMan
    // From inside Alchemy Side tower's code, we can call "GameMan.Alchemy.Tower = this" to add it to the manager
    // This will allow things like Player and AI managers to get their specific information
    private static OpposingSide alchemy;
    public static OpposingSide Alchemy { get => alchemy; set => alchemy = value; }
    private static OpposingSide shadow;
    public static OpposingSide Shadow { get => shadow; set => shadow = value; }

    // Adding some methods that will allow specific side units and towers to get their values by specific sides
    public static BaseObject GetClosestEnemy(UnitSide side)
    {
        if (side == UnitSide.alchemy)
        {
            return Shadow.ClosestUnit;
        }
        else
        {
            return Alchemy.ClosestUnit;
        }
    }
    // Get gold for each side
    public static int GetGold(UnitSide side)
    {
        if(side == UnitSide.alchemy)
        {
            return Alchemy.Inventory.gold;
        }
        else
        {
            return Shadow.Inventory.gold;
        }
    }
    // Modify them by a specific value, includes -values
    public static void ModifyGold(UnitSide side, int gold)
    {
        if (side == UnitSide.alchemy)
        {
            Alchemy.Inventory.gold += gold;
        }
        else
        {
            Shadow.Inventory.gold += gold;
        }
    }
    // Set them to a specific value
    public static void SetGold(UnitSide side, int gold)
    {
        if (side == UnitSide.alchemy)
        {
            Alchemy.Inventory.gold = gold;
        }
        else
        {
            Shadow.Inventory.gold = gold;
        }
    }

    // returns true/false based on whether unit was spawned, works for either side, maybe could look into 1v1
    // would be a cool thing to work with
    public bool SpawnUnit(UnitSide side, UnitType type)
    {
        if(side == UnitSide.alchemy)
        {
            return GameMan.Alchemy.Tower.SpawnUnit(type);
        }
        else
        {
            return GameMan.Shadow.Tower.SpawnUnit(type);
        }
    }
}