using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItems : Item {

    public UseItems(int ItemID, int amount)
    {
        /* Get a reference to the ItemDatabase. */
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();

        /* Set the params. */
        type = (int)ItemDatabase.ItemType.use;
        itemID = ItemID;
        this.amount = amount;
        sprite = itemDB.itemToSprite[itemID];

    }

    override
    public void ItemOnClickBehavior()
    {

    }
}
