using UnityEngine;

public class ItemQuestObjective : QuestObjective {

    public override void OnObjectiveStart()
    {
        CheckInventory();
    }

    /* Checks for items already existing in the inventory. */
    public void CheckInventory()
    {
        int amountInInventory = PlayerInventory.Instance.FindInInventory(objectiveNeeded);
        if (amountInInventory > 0)
        {
            amountCompleted += PlayerInventory.Instance.FindInInventory(objectiveNeeded);
            if (amountCompleted >= amountNeeded)
            {
                isCompleted = true;
            }
        }
    }

    /* Called to signify a potential change to the quest. */
    public override void NotifyChange(int itemID, int amount)
    {
        if (itemID == objectiveNeeded)
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

    /************************************
     Used in QuestDatabase ONLY
    ************************************/
    public ItemQuestObjective(string description, int item_id, int amount)
    {
        this.description = description;
        sprite = ItemDatabaseSO.itemList[item_id].sprite;
        objectiveNeeded = item_id;
        amountNeeded = amount;
        amountCompleted = 0;
    }
}
