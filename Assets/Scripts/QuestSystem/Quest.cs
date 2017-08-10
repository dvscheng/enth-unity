using System.Collections.Generic;

public class Quest {
    public enum State
    {
        notGiven,                                               // The quest hasn't been handed out yet
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
        bool changed = false;
        foreach (QuestObjective questObjective in questObjectives)
        {
            questObjective.NotifyChange(itemID, amount);
            if (changed == false && questObjective.IsCompleted)
            {
                changed = true;
            }
        }
        return changed;
    }

    /* Notifies each QuestObjective of a change. If a QuestObjective is changed as a result of this,
        return true, otherwise return false. */
    public bool NotifyChange(int NPC_ID)
    {
        // TODO REPEATED CODE
        bool changed = false;
        foreach (QuestObjective questObjective in questObjectives)
        {
            questObjective.NotifyChange(NPC_ID);
            if (changed == false && questObjective.IsCompleted)
            {
                changed = true;
            }
        }
        return changed;
    }


    /************************************
      Used in NPCDatabase ONLY
     ************************************/
    public Quest(int quest_ID, int[] requiredQuestIDs, int startNPC, int endNPC, string[] startDialogue, string[] completeDialogue, QuestObjective[] objectives)
    {
        id = quest_ID;
        currentState = (int)State.notGiven;
        this.startNPC = startNPC;
        this.endNPC = endNPC;
        this.startDialogue = startDialogue;
        this.completeDialogue = completeDialogue;
        questObjectives = new List<QuestObjective>();
        questObjectives.AddRange(objectives);
        requirements = new List<int>();
        requirements.AddRange(requiredQuestIDs);
    }
}
