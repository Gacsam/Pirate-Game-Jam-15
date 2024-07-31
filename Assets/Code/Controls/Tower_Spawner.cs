using UnityEngine;

public class Tower_Spawner : BaseObject
{
    // minion prefabs
    public GameObject melePrefab;
    public int minionCost = 30;

    // cooldowns
    public float CD = 2f;
    
    void Start() {
        GameMan.Alchemy.Tower = this;
    }

    // used on button
    public bool SpawnUnit(){
        if(GameMan.Alchemy.Inventory.gold < minionCost){return false;}
        if (!spawnAreaClear) return false;

        var newUnit = Instantiate(melePrefab, GameMan.CalculateSpawnPosition(gameObject.transform, melePrefab), Quaternion.identity);
        
        if (newUnit.GetComponent<BaseMovingUnit>().thisUnitSide == UnitSide.Alchemy) {
            GameMan.Alchemy.AddUnit(newUnit.GetComponent<BaseMovingUnit>());
        }
        else
        {
            GameMan.Shadow.AddUnit(newUnit.GetComponent<BaseMovingUnit>());
        }

        GameMan.Alchemy.Inventory.gold -= minionCost;
        GameMan.Alchemy.Inventory.UpdateGold();

        return true;
    }

    private void Update() {
        spawnAreaClear = true;
    }


    private bool spawnAreaClear = true;

    public void OnTriggerStay2D(Collider2D collision){
        // Make sure it's a unit blocking the exit
        if (collision.GetComponent<BaseUnit>() != null && collision.GetComponent<BaseUnit>().thisUnitSide == thisUnitSide)
        {
            spawnAreaClear = false;
        }
    }

    protected override void HandleDestruction()
    {
        // Destruction animation
        GameMan.TowerDestroyed(thisUnitSide);
    }
}
