using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrackerUI : MonoBehaviour {
    #region Singleton Behaviour
    private static QuestTrackerUI _instance;   // singleton behaviour
    public static QuestTrackerUI Instance
    {
        get { return _instance; }
    }
    #endregion

    ItemDatabase itemDB;

    public GameObject questObjectArea;  // UNITY

    readonly float PADDING_FROM_TOP = -27f;
    readonly int MAX_NUM_QUESTS = 2;
    int currNumQuests = 0;
    List<GameObject> quests;

    // Use this for initialization
    void Awake()
    {
        #region Singleton Behaviour
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
            return;
        }
        //DontDestroyOnLoad(gameObject); already in UI 
        #endregion

        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();
        quests = new List<GameObject>(MAX_NUM_QUESTS);
    }
	
    /* Refresh ALL quests. */
    public void Refresh()
    {
        foreach (GameObject quest in quests)
        {
            DoRefreshing(quest);
        }
    }

    /* Refresh a specific quest given the posInTracker variable. */
    public void Refresh(int posInTracker)
    {
        DoRefreshing(quests[posInTracker]);
    }

    /* Does the refreshing of the QuestItem. */
    private void DoRefreshing(GameObject quest)
    {
        CollectQuest questInfo = quest.GetComponent<CollectQuest>();

        /* Apply the correct position for the quest in the tracker. */
        RectTransform rect = quest.GetComponent<RectTransform>();
        float height = rect.sizeDelta.y;
        float posY = (questInfo.posInTracker * (height + PADDING_FROM_TOP)) + -100f;
        //rect.localPosition = new Vector2(rect.localPosition.x, posY);

        /* Checks for items already existing in the inventory. */
        int inventFirstAmount = PlayerInventory.Instance.FindInInventory(questInfo.firstItemID);
        int inventSecondAmount = PlayerInventory.Instance.FindInInventory(questInfo.secondItemID);
        if (inventFirstAmount != -1)
            questInfo.firstCompleted += inventFirstAmount;
        if (inventSecondAmount != -1)
            questInfo.secondCompleted += inventSecondAmount;

        questInfo.firstImage.GetComponent<Image>().sprite = itemDB.itemDictionary[questInfo.firstItemID];
        questInfo.secondImage.GetComponent<Image>().sprite = itemDB.itemDictionary[questInfo.secondItemID];
        questInfo.firstText.GetComponent<Text>().text = questInfo.firstCompleted + "/" + questInfo.firstAmount;
        questInfo.secondText.GetComponent<Text>().text = questInfo.secondCompleted + "/" + questInfo.secondAmount;
    }

	public void AddQuest(CollectQuest quest)
    {
        if (currNumQuests != MAX_NUM_QUESTS)
        {
            quests.Add(quest.gameObject);
            quest.posInTracker = quests.IndexOf(quest.gameObject);

            DoRefreshing(quest.gameObject);
            currNumQuests++;
        }
        else
        {
            // display "too many quests" message
        }
    }

    public void RemoveQuest(CollectQuest quest)
    {
        quests.RemoveAt(quest.posInTracker);

        /* Update the posInTracker variables for each quest. */
        for (int i = 0; i < quests.Count; i++)
        {
            CollectQuest questInfo = quests[i].GetComponent<CollectQuest>();
            questInfo.posInTracker = i;
        }

        Refresh();
    }

    /* Applies appropriate changes to the tracker UI when notified of a change. */
    public void NotifyQuestTracker(Item item)
    {
        foreach (GameObject quest in quests)
        {
            CollectQuest questInfo = quest.GetComponent<CollectQuest>();
            if (item.ItemID == questInfo.firstItemID)
            {
                questInfo.firstCompleted += item.Amount;
                DoRefreshing(quest);
            }
            if (item.ItemID == questInfo.secondItemID)
            {
                questInfo.secondCompleted += item.Amount;
                DoRefreshing(quest);
            }

            /* If the quest is finished... */
            if (questInfo.firstCompleted >= questInfo.firstAmount && questInfo.secondCompleted >= questInfo.secondAmount)
            {
                print("iscomplete = true");
                questInfo.IsComplete = true;
            }
        }
    }
}
