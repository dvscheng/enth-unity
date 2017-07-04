using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CollectQuest : MonoBehaviour {
    public NPCs _NPC;
    public bool IsComplete { get; set; }
    public static readonly int MAX_COLLECT_AMOUNT = 4;
    public int firstItemID;
    public int firstAmount;
    public int secondItemID;
    public int secondAmount;
    public int firstCompleted;
    public int secondCompleted;

    /* Unity inspector stuff. */
    public GameObject firstImage;
    public GameObject firstText;
    public GameObject secondImage;
    public GameObject secondText;


    /* Position in the traker is defaulted to -1, meaning it's not in the tracker. */
    public int posInTracker = -1;

    public void Initialize(NPCs NPC, int firstItemID, int secondItemID, int firstAmount, int secondAmount)
    {
        _NPC = NPC;
        this.firstItemID = firstItemID;
        this.secondItemID = secondItemID;
        this.firstAmount = firstAmount;
        this.secondAmount = secondAmount;
        AddToQuestTrackerUI();
    }

    /* Called by QuestDialogue. */
    public void AddToQuestTrackerUI()
    {
        QuestTrackerUI.Instance.AddQuest(this);
    }

    public void Complete()
    {
        PlayerInventory.Instance.RemoveFromInventory(firstItemID, (int)ItemDatabase.ItemType.mats, firstAmount);
        PlayerInventory.Instance.RemoveFromInventory(secondItemID, (int)ItemDatabase.ItemType.mats, secondAmount);
        QuestTrackerUI.Instance.RemoveQuest(this);
        _NPC.IsQuestGiver = false;

        // give player rewards
        PlayerController.Instance.BonusAtt += 2;
        PlayerController.Instance.BonusDef += 2;
        PlayerController.Instance.BonusDefPen += 1.99f;
        UIManager.Instance.RefreshStats();
    }
}
