using System.Collections.Generic;

public class Quest {
    private int npcID;
    public int NPC_ID
    {
        get { return NPC_ID;  }
    }

    private List<QuestObjective> questObjectives;               // UIQuestBlock requires that the objectives been in order as they are in the ui block
    public List<QuestObjective> QuestObjectives
    {
        get
        {
            return questObjectives;
        }
    }

	public Quest(int NPC_ID, QuestObjective[] objectives)
    {
        npcID = NPC_ID;
        questObjectives = new List<QuestObjective>();
        questObjectives.AddRange(objectives);
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
}
