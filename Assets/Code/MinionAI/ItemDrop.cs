using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public static void DropItem(){

        if(GameObject.Find("Player Tower") == null){return;}

        // get player inventory
        Inventory playerInventory = GameObject.Find("Player Tower").GetComponent<Inventory>();

        // random drop
        int amountOfItems = System.Enum.GetValues(typeof(itemType)).Length;
        itemType dropedItem = (itemType)Random.Range(1,amountOfItems);  // random drop

        // add to player inventory
        playerInventory.AddInventory(dropedItem);

        // gold drop
        playerInventory.AddInventory(itemType.gold,20);

        // drop with custom amount
        // playerInventory.AddInventory(dropedItem,10);

    }

}

// add more items here
// shards, gold coins, etc...
public enum itemType { none, gold, fire, water, air, earth }