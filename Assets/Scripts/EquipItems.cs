using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItems : Item {

    public EquipItems(int ItemID, int amount)
    {
        /* Get a reference to the ItemDatabase. */
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();

        /* Set the params. */
        type = (int)ItemDatabase.ItemType.equip;
        itemID = ItemID;
        this.amount = amount;
        sprite = itemDB.itemDictionary[itemID];

    }

    override
    public void ItemOnClickBehavior()
    {

    }
}
