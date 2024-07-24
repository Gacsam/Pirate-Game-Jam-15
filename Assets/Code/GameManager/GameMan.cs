using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameMan : MonoBehaviour
{
    /// <summary>
    /// Global value for melee range. Additionally, represents the minimum distance between units.
    /// </summary>
    public static float globalMeleeRange = 0.5f;
    // GameMananager singleton to track all the variables that we need accessible
    private static GameMan instance;
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
            alchemy.Inventory = new Inventory();
        }
        if (shadow == null)
        {
            shadow = new OpposingSide();
            shadow.Inventory = new Inventory();
        }
    }
    // Having a public static "Instance" allows us to call GameMan.X rather than GameMan.instance.X
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameSpeed == GameSpeed.Normal)
            {
                GameSpeed = GameSpeed.Stop;
            }else if (GameSpeed == GameSpeed.Stop)
            {
                GameSpeed = GameSpeed.Normal;
            }
        }
    }

    static GameSpeed currentWorldSpeed = GameSpeed.Normal;

    public static GameSpeed GameSpeed { get { return currentWorldSpeed; } set {
            switch (value)
            {
                case GameSpeed.Stop:
                    Time.timeScale = 0;
                    break;
                case GameSpeed.Normal:
                    Time.timeScale = 1;
                    break;
                case GameSpeed.Fast:
                    Time.timeScale = 2;
                    break;
                case GameSpeed.Superfast:
                    Time.timeScale = 3;
                    break;
                default:
                    break;
            }
            currentWorldSpeed = value;
        } 
    }
            
    // Similarly, by creating an OppositeSide class, we can refer to these sides outside the GameMan
    // From inside Alchemy Side tower's code, we can call "GameMan.Alchemy.Tower = this" to add it to the manager
    // This will allow things like Player and AI managers to get their specific information
    private static OpposingSide alchemy;
    public static OpposingSide Alchemy { get => alchemy; }
    private static OpposingSide shadow;
    public static OpposingSide Shadow { get => shadow; }

    /// <summary>
    /// Can be called by any unit to check the closest enemy unit to their tower
    /// </summary>
    /// <param name="side">the side of the unit, represented by this.thisUnitSide</param>
    /// <returns>Closest enemy unit based on side inputted</returns>
    public static BaseObject GetClosestEnemy(UnitSide side)
    {
        if (side == UnitSide.Alchemy)
        {
            return Shadow.ClosestUnit;
        }
        else
        {
            return Alchemy.ClosestUnit;
        }
    }

    // used by borax to heal close allies
    public static void HealCloseAllies(UnitSide side,BaseObject targetUnit){
        if(side == UnitSide.Alchemy){Alchemy.HealCloseAllies(targetUnit);}
        else{Shadow.HealCloseAllies(targetUnit);}
    }

    /// <summary>
    /// Get the gold value from a specific side
    /// </summary>
    /// <param name="side">the side of the unit, represented by this.thisUnitSide</param>
    /// <returns>Amount of gold a specific side has</returns>
    public static int GetGold(UnitSide side)
    {
        if(side == UnitSide.Alchemy)
        {
            return Alchemy.Inventory.gold;
        }
        else
        {
            return Shadow.Inventory.gold;
        }
    }
    /// <summary>
    /// Modify gold by a specific value, includes -values
    /// </summary>
    /// <param name="side">the side of the unit, represented by this.thisUnitSide</param>
    /// <param name="gold">gold to modify the side's value</param>
    public static void ModifyGold(UnitSide side, int gold)
    {
        if (side == UnitSide.Alchemy)
        {
            Alchemy.Inventory.gold += gold;
        }
        else
        {
            Shadow.Inventory.gold += gold;
        }
    }

    /// <summary>
    /// Set a specified side's gold to value
    /// </summary>
    /// <param name="side">the side of the unit, represented by this.thisUnitSide</param>
    /// <param name="gold">gold to set the side's value to</param>
    public static void SetGold(UnitSide side, int gold)
    {
        if (side == UnitSide.Alchemy)
        {
            Alchemy.Inventory.gold = gold;
        }
        else
        {
            Shadow.Inventory.gold = gold;
        }
    }
    
    /// <summary>
    /// Attempts to spawn a unit, could be modified to work for either side. (Maybe could look into 1v1, would be a cool thing to work with)
    /// </summary>
    /// <param name="side">the side of the unit, represented by this.thisUnitSide</param>
    /// <param name="type">the unit type, represented by this.thisUnitType</param>
    /// <returns>true if unit successfully spawned</returns>
    public static bool SpawnUnit(UnitSide side) 
    {
        if(side == UnitSide.Alchemy)
        {
            return GameMan.Alchemy.Tower.GetComponent<Tower_Spawner>().SpawnUnit();
        }
        else
        {
            return false;
            // return GameMan.Shadow.Tower.SpawnUnit(type);
        }
    }
    /// <summary>
    /// Function called by either tower once they're destroyed, start a win/lose screen and stuff.
    /// </summary>
    public static void TowerDestroyed(UnitSide side)
    {

    }

    public static void CalculateSpawnPosition(ref GameObject unit)
    {
        var offset = unit.GetComponent<Collider2D>().bounds.extents.y;
        // Create a raycast to see where ground is
        RaycastHit2D hit = Physics2D.Raycast(unit.transform.position, Vector2.down, 100, ~LayerMask.GetMask("Ignore Raycast"));
        var newMinionPosition = unit.transform.position;
        if (hit)
        {
            newMinionPosition.y = hit.point.y + offset - (0.01f * Random.Range(0,50));
        }
        unit.transform.position = newMinionPosition;
    }

    // <summary>
    // Cloud control when moving camera to show depth
    // <summary>
    public static List<Clouds> clouds = new List<Clouds>();
    public static void MoveCloud(Vector2 direction)
    {
        foreach (Clouds cloud in clouds)
        {
            cloud.MoveCloud(direction);
        }
    }

    // <summary>
    // Update health slider for towers
    // <summary>
    public static void UpdateHealthSlider(){
        alchemy.Tower.UpdateHealthSlider();
        shadow.Tower.UpdateHealthSlider();
    }
}