using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItems : Item {

    public EquipItems(int ItemID, int amount)
    {
        /* Get a reference to the ItemDatabase. */
        itemDatabase = ScriptableObject.CreateInstance<ItemDatabaseSO>();

        /* Set the params. */
        type = (int)ItemDatabaseSO.ItemType.equip;
        itemID = ItemID;
        this.amount = amount;
        sprite = itemDatabase.itemList[itemID].sprite;

    }

    override
    public void ItemOnClickBehavior()
    {

    }
}
