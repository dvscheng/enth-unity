using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    protected int type = (int)ItemDatabase.ItemType.error;
    protected int itemID = (int)ItemDatabase.ItemID.none;
    protected int amount = 0;

    protected ItemDatabase itemDB;
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
