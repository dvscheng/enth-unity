using System.Collections.Generic;
using UnityEngine;

public class ItemQuestObjective : QuestObjective {
    string description;
    public string Description
    {
        get
        {
            return description;
        }
    }
    int itemNeeded;
    public int ItemNeeded
    {
        get
        {
            return itemNeeded;
        }
    }
    int amountNeeded;
    public int AmountNeeded
    {
        get
        {
            return amountNeeded;
        }
    }
    int amountCompleted;
    public int AmountCompleted
    {
        get
        {
            return amountCompleted;
        }
    }

	public ItemQuestObjective(string description, int item, int amount)
    {
        this.description = description;
        itemNeeded = item;
        amountNeeded = amount;
        amountCompleted = 0;
    }

    /* Checks for items already existing in the inventory. */
    public void CheckInventory()
    {
        int amountInInventory = PlayerInventory.Instance.FindInInventory(itemNeeded);
        if (amountInInventory > 0)
        {
            amountCompleted += PlayerInventory.Instance.FindInInventory(itemNeeded);
            if (amountCompleted >= amountNeeded)
            {
                isCompleted = true;
            }
        }
    }

    /* Called to signify a potential change to the quest. */
    public void NotifyChange(int itemID, int amount)
    {
        if (itemID == itemNeeded)
        {
            amountCompleted += amount;
            if (amountCompleted < 0)
            {
                Debug.Log("An amount was passed into NotifyChange that dropped our amountCompleted below 0. Amount: " + amount + "Completed " + amountCompleted);
                amountCompleted = 0;
            } else if (amountCompleted >= amountNeeded)
            {
                isCompleted = true;
            } else if (amountCompleted < amountNeeded)
            {
                isCompleted = false;
            }
        }
    }
}
