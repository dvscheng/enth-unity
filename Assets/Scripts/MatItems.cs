using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatItems : Item {

    public MatItems(int ItemID, int amount)
    {
        /* Get a reference to the ItemDatabase. */
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();

        /* Set the params. */
        type = (int)ItemDatabase.ItemType.mats;
        itemID = ItemID;
        this.amount = amount;
        sprite = itemDB.itemToSprite[itemID];
    }
    
    override
	public void ItemOnClickBehavior()
    {

    }
}
