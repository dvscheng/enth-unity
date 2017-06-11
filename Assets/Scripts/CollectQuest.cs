using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CollectQuest {
    public static readonly int MAX_COLLECT_AMOUNT = 4;
    int firstItemID;
    int secondItemID;
    int firstAmount;
    int secondAmount;
    // show quests on top right with kill/collect count?

    public CollectQuest(int firstItemID, int secondItemID, int firstAmount, int secondAmount)
    {
        this.firstItemID = firstItemID;
        this.secondItemID = secondItemID;
        this.firstAmount = firstAmount;
        this.secondAmount = secondAmount;
    }

    /* Called by QuestDialogue. */
    public void NotifyQuestTrackerUI()
    {
        // refresh the quest UI.
    }
}
