using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    protected int type = (int)ItemDatabaseSO.ItemType.mats;
    protected int itemID = (int)ItemDatabaseSO.ItemID.mushroom;
    protected int amount = 0;

    public Sprite sprite;

    
    public int Type
    {
        get { return type; }

    }
    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }
    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public abstract void ItemOnClickBehavior();
}
