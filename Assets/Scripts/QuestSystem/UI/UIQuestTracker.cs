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

    public Transform questContentArea;  // UNITY

    int currNumQuests = 0;
    List<GameObject> questBlockGOs;
    List<float> blockHeights;
    QuestDatabase questDatabase;

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

        questDatabase = ScriptableObject.CreateInstance<QuestDatabase>();
        questBlockGOs = new List<GameObject>();
        blockHeights = new List<float>();
    }

    /* Used to subscribe to publishers. */
    void OnEnable()
    {
        PlayerInventory.OnAddItem += NotifyQuests;
        NPC.OnNPCInteraction += NotifyQuests;
    }

    /* Used to unsubscribe to publishers. */
    void OnDisable()
    {
        PlayerInventory.OnAddItem -= NotifyQuests;
        NPC.OnNPCInteraction -= NotifyQuests;
    }

    /* VISUAL: Update the progress of the specified quest. */
    private void RefreshQuest(Quest quest)
    {
        for (int i = 0; i < questBlockGOs.Count; i++)
        {
            UIQuestBlock questBlock = questBlockGOs[i].GetComponent<UIQuestBlock>();
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
        GameObject NewUIQuestBlock = Instantiate(Resources.Load<GameObject>("Prefabs/QuestSystem/UIQuestBlock"), questContentArea);
        NewUIQuestBlock.GetComponent<UIQuestBlock>().Initialize(quest);

        questBlockGOs.Add(NewUIQuestBlock);
        currNumQuests++;
    }

    /* Removes the given quest. */
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < questBlockGOs.Count; i++)
        {
            GameObject q = questBlockGOs[i];
            if (q.GetComponent<Quest>().Equals(quest)) {
                questBlockGOs.RemoveAt(i);
                Destroy(q);

                Debug.Log("Successfully removed a quest.");
            }
        }
    }

    public void CompleteQuest(int questID)
    {

    }

    /// <summary>
    /// Returns a List of all currently tracked quests. If mustBeReady is true, the quests returned are all ready to complete.
    /// </summary>
    /// <param name="mustBeReady"></param>
    /// <returns> Returns a list of quests. </returns>
    public List<Quest> AllQuests(bool mustBeReady)
    {
        List<Quest> allQuests = new List<Quest>();
        foreach (GameObject go in questBlockGOs)
        {
            Quest quest = go.GetComponent<UIQuestBlock>().LinkedQuest;
            if (!mustBeReady || quest.CurrentState == (int)Quest.State.ready)
            {
                allQuests.Add(go.GetComponent<UIQuestBlock>().LinkedQuest);
            }
        }
        return allQuests;
    }

    /************************************
     'Notify' methods
    ************************************/

    /* Notify each quest that there is a potential change. */
    public void NotifyQuests(int itemID, int amount)
    {
        foreach (GameObject questBlockGO in questBlockGOs)
        {
            /* Notify the QuestsBlock that there is a potential change. */
            questBlockGO.GetComponent<UIQuestBlock>().NotifyChange(itemID, amount);
        }
    }

    /* Notify each quest that there is a potential change. */
    public void NotifyQuests(int NPC_ID)
    {
        foreach (GameObject questBlockGO in questBlockGOs)
        {
            /* Notify the QuestsBlock that there is a potential change. */
            questBlockGO.GetComponent<UIQuestBlock>().NotifyChange(NPC_ID);
        }
    }

    /****************************************************************************************************
     DEBUGGING
    ****************************************************************************************************/

    public void AddRandomQuest()
    {
        AddQuest(questDatabase.NPCIDToQuests[0][0]);
    }
}
