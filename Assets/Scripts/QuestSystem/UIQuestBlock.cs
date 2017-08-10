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
            GameObject NewUIQuestObjective = Instantiate(Resources.Load<GameObject>("Prefabs/QuestSystem/UIQuestObjective"), gameObject.transform);
            QuestObjective questObjective = questObjectives[i];
            NewUIQuestObjective.transform.GetChild(0).GetComponent<Image>().sprite = itemDB.itemList[questObjective.ObjectiveNeeded].sprite;
            NewUIQuestObjective.transform.GetChild(1).GetComponent<Text>().text = questObjective.AmountCompleted + "/" + questObjective.AmountNeeded;
            NewUIQuestObjective.transform.GetChild(2).GetComponent<Text>().text = questObjective.Description;
            questObjective.OnObjectiveStart();

            questObjectiveGOs.Add(NewUIQuestObjective);
        }
    }

    /* Used to sync the tracker with the actual quest progress. */
    public void RefreshVisuals()
    {
        for (int i = 0; i < questObjectiveGOs.Count; i++)
        {
            GameObject questObjectiveGO = questObjectiveGOs[i];
            QuestObjective questObjective = LinkedQuest.QuestObjectives[i];
            questObjectiveGO.transform.GetChild(0).GetComponent<Image>().sprite = itemDB.itemList[questObjective.ObjectiveNeeded].sprite;
            questObjectiveGO.transform.GetChild(1).GetComponent<Text>().text = questObjective.AmountCompleted + "/" + questObjective.AmountNeeded;
            questObjectiveGO.transform.GetChild(2).GetComponent<Text>().text = questObjective.Description;
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

    /* Notifies the quest of a potential change and then checks if there needs to be an update. */
    public void NotifyChange(int NPC_ID)
    {
        if (linkedQuest.NotifyChange(NPC_ID))
        {
            RefreshVisuals();
        }
    }
}
