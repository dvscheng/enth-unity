using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestTracker : MonoBehaviour {

    #region Singleton Behaviour
    private static UIQuestTracker _instance;   // singleton behaviour
    public static UIQuestTracker Instance
    {
        get { return _instance; }
    }
    #endregion

    ItemDatabaseSO itemDatabase;

    public GameObject questContentArea;  // UNITY

    readonly float PADDING_FROM_TOP = -27f;
    readonly float BLOCK_HEIGHT = 45f;
    int currNumQuests = 0;
    List<GameObject> quests;
    List<float> blockHeights;

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

        itemDatabase = ScriptableObject.CreateInstance<ItemDatabaseSO>();
        quests = new List<GameObject>();
        blockHeights = new List<float>();
    }

    /* Notify each quest that there is a potential change. */
    public void NotifyQuests(int itemID, int amount)
    {
        foreach (GameObject questBlockGO in quests)
        {
            /* Notify the QuestsBlock that there is a potential change. */
            questBlockGO.GetComponent<UIQuestBlock>().NotifyChange(itemID, amount);
        }
    }

    /* VISUAL: Refresh and update the UI placement and visuals of the UI. */
    public void RefreshTrackerUI()
    {

    }

    /* VISUAL: Update the progress of the specified quest. */
    private void RefreshQuest(Quest quest)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            UIQuestBlock questBlock = quests[i].GetComponent<UIQuestBlock>();
            if (questBlock.LinkedQuest.Equals(quest))
            {
                questBlock.RefreshVisuals();
            }
        }
    }

    /* Adds a new quest to the quest tracker. */
    public void AddQuest(Quest quest)
    {
        /* Adds the number of paddings and block heights. */
        float distanceFromTop = (PADDING_FROM_TOP * (currNumQuests + 1));           //negative
        for (int i = 0; i < blockHeights.Count; i++)
        {
            distanceFromTop += blockHeights[i];
        }

        Vector2 pos = new Vector2(0, distanceFromTop);
        GameObject NewUIQuestBlock = Instantiate(Resources.Load<GameObject>("Prefabs/QuestSystem/UIQuestBlock"), pos, Quaternion.identity, gameObject.transform);
        NewUIQuestBlock.GetComponent<UIQuestBlock>().Initialize(quest);

        quests.Add(NewUIQuestBlock);
        currNumQuests++;

        RefreshTrackerUI();
    }

    /* Removes the given quest. */
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            GameObject q = quests[i];
            if (q.GetComponent<Quest>().Equals(quest)) {
                quests.RemoveAt(i);
                Destroy(q);

                Debug.Log("Successfully removed a quest.");
            }
        }
    }
}
