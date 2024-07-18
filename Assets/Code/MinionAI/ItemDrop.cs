using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public static void DropItem(){

        // get player inventory
        Inventory playerInventory = GameObject.Find("Player").GetComponent<Inventory>();

        // random drop
        int amountOfItems = System.Enum.GetValues(typeof(itemType)).Length;
        itemType dropedItem = (itemType)Random.Range(1,amountOfItems);  // random drop

        // add to player inventory
        playerInventory.AddInventory(dropedItem);

        // drop with custom amount
        // playerInventory.AddInventory(dropedItem,10);

    }

}

// add more items here
// shards, gold coins, etc...
public enum itemType { none, gold, fire, water, air, earth }