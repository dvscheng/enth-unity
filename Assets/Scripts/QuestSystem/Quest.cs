using System.Collections.Generic;

public class Quest {
    private int npcID;
    public int NPC_ID
    {
        get { return NPC_ID;  }
    }

    private List<QuestObjective> questObjectives;
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
        foreach (QuestObjective questObjective in questObjectives)
        {
            if (questObjective is ItemQuestObjective)
            {
                (questObjective as ItemQuestObjective).NotifyChange(itemID, amount);
                if (questObjective.IsCompleted)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
