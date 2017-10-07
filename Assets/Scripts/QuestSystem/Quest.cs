using System.Collections.Generic;
using UnityEngine;

public class Quest {
    public enum State
    {
        unqualified,                                            // The user has not yet met the requirements for this quest
        qualified,                                              // The quest hasn't been handed out yet, but the user has met the requirements
        incomplete,                                             // The quest has been handed out but not completed yet
        ready,                                                  // The quest is ready to be completed/handed-in
        completed                                               // The quest is completed
    }
    private int id;                                             // The quest number/id; assigned in QuestDatabase
    public int ID
    {
        get
        {
            return id;
        }
    }
    private int currentState;                                   // Whether or not the quest is completed
    public int CurrentState
    {
        get
        {
            return currentState;
        }
    }
    private int startNPC;                                       // The NPC that gave the quest
    public int StartNPC
    {
        get
        {
            return startNPC;
        }
    }
    private int endNPC;                                         // The NPC that completes the quest if the quest is complete
    public int EndNPC
    {
        get
        {
            return endNPC;
        }
    }
    private string questTitle;                                  // The title of the quest
    public string QuestTitle
    {
        get
        {
            return questTitle;
        }
    }
    private string[] startDialogue;                             // The dialogue shown when starting the quest
    public string[] StartDialogue
    {
        get
        {
            return startDialogue;
        }
    }
    private string[] completeDialogue;                          // The dialogue shown when completing the quest
    public string[] CompleteDialogue
    {
        get
        {
            return completeDialogue;
        }
    }

    private List<int> requirements;                             // Quests (their ids) that are required to start this quest 
    public List<int> Requirements
    {
        get
        {
            return requirements;
        }
    }                            // (less complex than having a Requirement datatype and supporting all types of requirements (kill count, level, item, exploration, etc.), and is more appropriate given the scope of this game

    private List<int> proceedingQuests;                         // Quests that have this quest as a requirement
    public List<int> ProceedingQuests
    {
        get
        {
            return proceedingQuests;
        }
    }

    private List<QuestObjective> questObjectives;               // UIQuestBlock requires that the objectives be in order as they are in the ui block
    public List<QuestObjective> QuestObjectives
    {
        get
        {
            return questObjectives;
        }
    }

    /* Notifies each QuestObjective of a change. If a QuestObjective is changed as a result of this,
        return true, otherwise return false. */
    public bool NotifyChange(int itemID, int amount)
    {
        int amountChanged = 0;
        foreach (QuestObjective questObjective in questObjectives)
        {
            questObjective.NotifyChange(itemID, amount);
            if (questObjective.IsCompleted)
            {
                amountChanged++;
            }
        }

        // if completed:
        if (amountChanged == questObjectives.Count)
        {
            currentState = (int)State.ready;
        }

        return amountChanged > 0;
    }

    /* Notifies each QuestObjective of a change. If a QuestObjective is changed as a result of this,
        return true, otherwise return false. */
    public bool NotifyChange(int NPC_ID)
    {
        // TODO REPEATED CODE
        int amountChanged = 0;
        foreach (QuestObjective questObjective in questObjectives)
        {
            questObjective.NotifyChange(NPC_ID);
            if (questObjective.IsCompleted)
            {
                amountChanged++;
            }
        }

        // if completed:
        if (amountChanged == questObjectives.Count)
        {
            currentState = (int)State.ready;
        }

        return amountChanged > 0;
    }

    public void OnQuestComplete()
    {
        if (currentState != (int)State.ready)
            Debug.Log("A quest was to be completed but it wasn't in the ready state.");
        else
        {
            currentState = (int)State.completed;
            NotifyProceedingQuests();
            UIManager.Instance.questTracker.RemoveQuest(this);

            // default rewards TODO: specialized rewards to-bo introduced on quest creation
            PlayerController.Instance.GainEXP((int)(PlayerController.Instance.MaxExp*.75));
        }
    }

    public void OnQuestAccept()
    {
        if (currentState != (int)State.qualified)
            Debug.Log("Tried to accept a quest that wasn't in the 'qualified' state.");
        else
            currentState = (int)State.incomplete;
    }

    /* Notifies each proceeding quest that this quest has been completed. */
    private void NotifyProceedingQuests()
    {
        foreach (int quest_id in proceedingQuests)
        {
            //questData.QuestDictionary[quest_id].CheckRequirements();
        }
    }

    /* Checks whether the requirements of this quest have been fulfilled; if they have been, then change the state to 'qualified'. */
    private void CheckRequirements()
    {
        foreach (int quest_id in requirements)
        {
            //if (questData.QuestDictionary[quest_id].currentState != (int)State.completed)
            //{
            //    return;
            //}
        }
        /* Reach here only if all requirements are in the completed state */
        if (currentState == (int)State.unqualified)
            currentState = (int)State.qualified;
    }

    /************************************
      Used in NPCDatabase ONLY
     ************************************/
    public Quest(int quest_ID, int state, int[] requiredQuestIDs, int[] proceedingQuestIDs, int startNPC, int endNPC, string questTitle,
        string[] startDialogue, string[] completeDialogue, QuestObjective[] objectives)
    {
        id = quest_ID;
        currentState = state;
        proceedingQuests = new List<int>(proceedingQuestIDs);
        this.startNPC = startNPC;
        this.endNPC = endNPC;
        this.questTitle = questTitle;
        this.startDialogue = startDialogue;
        this.completeDialogue = completeDialogue;
        questObjectives = new List<QuestObjective>();
        questObjectives.AddRange(objectives);
        requirements = new List<int>();
        requirements.AddRange(requiredQuestIDs);
    }
}
