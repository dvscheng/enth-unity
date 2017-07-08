using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatItems : Item {

    public MatItems(int ItemID, int amount)
    {
        /* Get a reference to the ItemDatabase. */
        itemDatabase = ScriptableObject.CreateInstance<ItemDatabaseSO>();

        /* Set the params. */
        type = (int)ItemDatabaseSO.ItemType.mats;
        itemID = ItemID;
        this.amount = amount;
        sprite = itemDatabase.itemList[itemID].sprite;
    }
    
    override
	public void ItemOnClickBehavior()
    {

    }
}
