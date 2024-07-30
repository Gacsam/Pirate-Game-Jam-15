using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public static void DropItem(ElementType element){

        if(GameObject.Find("Player Tower") == null){return;}

        // get player inventory
        Inventory playerInventory = GameMan.GetPlayerInventory();

        // drop shard relative to element type
        playerInventory.AddInventory(element);

        // gold drop
        playerInventory.gold += 10;
        playerInventory.UpdateGold();

        // drop with custom amount
        // playerInventory.AddInventory(dropedItem,10);

    }

}