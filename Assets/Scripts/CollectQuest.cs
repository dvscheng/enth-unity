using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CollectQuest : MonoBehaviour {
    QuestTrackerUI questTracker;

    public static readonly int MAX_COLLECT_AMOUNT = 4;
    int firstItemID;
    int secondItemID;
    int firstAmount;
    int secondAmount;
    // show quests on top right with kill/collect count?

    public void Awake()
    {
        questTracker = QuestTrackerUI.Instance;
    }

    public void Initialize(int firstItemID, int secondItemID, int firstAmount, int secondAmount)
    {
        this.firstItemID = firstItemID;
        this.secondItemID = secondItemID;
        this.firstAmount = firstAmount;
        this.secondAmount = secondAmount;
    }

    /* Called by QuestDialogue. */
    public void AddToQuestTrackerUI()
    {
        // refresh the quest UI.
    }
}
