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

        foreach (var button in Camera.main.GetComponentsInChildren<Button>())
        {
            switch (button.name)
            {
                // INCLUDE only these specific buttons
                // We can reverse 'break' and 'continue' to EXCLUDE instead
                case "Melee Spawn Button": break;
                case "Ranged Spawn Button": break;
                default: continue;
            }
            // LAMBDA FUNCTIONS OP
            button.onClick.AddListener(() => InteractedWithButton(button));
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

    private void InteractedWithButton(Button interactedButton)
    {
        bool unitSpawned = false;
        switch (interactedButton.name)
        {
            case "Melee Spawn Button":
                unitSpawned = SpawnUnit(UnitSide.Alchemy, UnitType.Melee);
                if (unitSpawned)
                {
                    // Do stuff like take coins away
                }
                break;
            case "Ranged Spawn Button":
                unitSpawned = SpawnUnit(UnitSide.Alchemy, UnitType.Ranged);
                if (unitSpawned)
                {
                    // Do stuff like take coins away
                }
                break;
            default: 
                Debug.Log("No interaction found with \"" + name + "\"");
                break;
        }
        if(!unitSpawned)
            StartCoroutine(ButtonFlashColour(interactedButton, Color.red));
    }

    IEnumerator ButtonFlashColour(Button buttonToFlash, Color colourToSet){
        // Check if it's not red already
        if (buttonToFlash.image.color != colourToSet)
        {
        var defaultColor = buttonToFlash.image.color;
        buttonToFlash.image.color = colourToSet;
        yield return new WaitForSeconds(0.1f);
            buttonToFlash.image.color = defaultColor;
        }
        yield return null;
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
    public bool SpawnUnit(UnitSide side, UnitType type) 
    {
        if(side == UnitSide.Alchemy)
        {
            return GameMan.Alchemy.Tower.GetComponent<Tower_Spawner>().SpawnUnit(type);
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

    // <summary>
    // Cloud controll when moving camera to show depth
    // <summary>
    public static List<Clouds> clouds = new List<Clouds>();
    public static void MoveCloud(Vector2 direction){
        foreach (Clouds cloud in clouds){
            cloud.MoveCloud(direction);
        }
    }
}