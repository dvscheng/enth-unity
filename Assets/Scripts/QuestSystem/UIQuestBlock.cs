using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestBlock : MonoBehaviour {
    private Quest linkedQuest;
    public Quest LinkedQuest
    {
        get { return linkedQuest; }
    }
    ItemDatabaseSO itemDB;

    readonly float DISTANCE_BETWEEN_OBJECTIVES = 32f;
    public List<GameObject> questObjectiveGOs;

    void Awake()
    {
        itemDB = ScriptableObject.CreateInstance<ItemDatabaseSO>();
    }

    /* Initializes all QuestBlocks. */
    public void Initialize(Quest quest)
    {
        linkedQuest = quest;
        questObjectiveGOs = new List<GameObject>();

        /* Initialize a prefab for every QuestObjective. */
        List<QuestObjective> questObjectives = quest.QuestObjectives;
        for (int i = 0; i < questObjectives.Count; i++)
        {
            GameObject NewUIQuestObjective = Instantiate(
                Resources.Load<GameObject>("Prefabs/QuestSystem/UIQuestObjective"), gameObject.transform);

            QuestObjective questObjective = questObjectives[i];
            if (questObjective is ItemQuestObjective)
            {
                NewUIQuestObjective.transform.GetChild(0).GetComponent<Image>().sprite =
                    itemDB.itemList[(questObjective as ItemQuestObjective).ItemNeeded].sprite;
                NewUIQuestObjective.transform.GetChild(1).GetComponent<Text>().text =
                    (questObjective as ItemQuestObjective).AmountCompleted + "/" + (questObjective as ItemQuestObjective).AmountNeeded;
                NewUIQuestObjective.transform.GetChild(2).GetComponent<Text>().text =
                    (questObjective as ItemQuestObjective).Description;
                (questObjective as ItemQuestObjective).CheckInventory();
            } else
            {
                //other types
            }

            questObjectiveGOs.Add(NewUIQuestObjective);
        }
    }

    /* Used to sync the tracker with the actual quest progress. */
    public void RefreshVisuals()
    {
        for (int i = 0; i < questObjectiveGOs.Count; i++)
        {
            GameObject questObjectiveGO = questObjectiveGOs[i];
            QuestObjective questObjective = questObjectiveGO.GetComponent<QuestObjective>();
            if (questObjective is ItemQuestObjective)
            {
                questObjectiveGO.transform.GetChild(0).GetComponent<Image>().sprite =
                    itemDB.itemList[(questObjective as ItemQuestObjective).ItemNeeded].sprite;
                questObjectiveGO.transform.GetChild(1).GetComponent<Text>().text =
                    (questObjective as ItemQuestObjective).AmountCompleted + "/" + (questObjective as ItemQuestObjective).AmountNeeded;
                questObjectiveGO.transform.GetChild(2).GetComponent<Text>().text =
                    (questObjective as ItemQuestObjective).Description;
            } else
            {
                // other types
            }
        }
    }

    /* Notifies the quest of a potential change and then checks if there needs to be an update. */
    public void NotifyChange(int itemID, int amount)
    {
        if (linkedQuest.NotifyChange(itemID, amount))
        {
            RefreshVisuals();
        }
    }
}
