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
    List<GameObject> questGOs;
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
        questGOs = new List<GameObject>();
        blockHeights = new List<float>();
    }

    /* Used to subscribe to publishers. */
    void OnEnable()
    {
        PlayerInventory.onAddItem += NotifyQuests;
    }

    /* Used to unsubscribe to publishers. */
    void OnDisable()
    {
        PlayerInventory.onAddItem -= NotifyQuests;
    }

    /* VISUAL: Update the progress of the specified quest. */
    private void RefreshQuest(Quest quest)
    {
        for (int i = 0; i < questGOs.Count; i++)
        {
            UIQuestBlock questBlock = questGOs[i].GetComponent<UIQuestBlock>();
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

        questGOs.Add(NewUIQuestBlock);
        currNumQuests++;
    }

    /* Removes the given quest. */
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < questGOs.Count; i++)
        {
            GameObject q = questGOs[i];
            if (q.GetComponent<Quest>().Equals(quest)) {
                questGOs.RemoveAt(i);
                Destroy(q);

                Debug.Log("Successfully removed a quest.");
            }
        }
    }

    /************************************
     'Notify' methods
    ************************************/

    /* Notify each quest that there is a potential change. */
    public void NotifyQuests(int itemID, int amount)
    {
        print("notiffied");
        foreach (GameObject questBlockGO in questGOs)
        {
            /* Notify the QuestsBlock that there is a potential change. */
            questBlockGO.GetComponent<UIQuestBlock>().NotifyChange(itemID, amount);
        }
    }

    /* Notify each quest that there is a potential change. */
    public void NotifyQuests(int NPC_ID)
    {
        foreach (GameObject questBlockGO in questGOs)
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
        AddQuest(questDatabase.questInfo[1]);
    }
}
