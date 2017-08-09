using UnityEngine;

public class NPCQuestObjective : QuestObjective {
	public NPCQuestObjective(int NPC)
    {

    }

    public override void NotifyChange(int NPC_id)
    {
        if (NPC_id == objectiveNeeded)
        {
            isCompleted = true;
        }
    }

    /************************************
     Used in NPCDatabase ONLY
    ************************************/
    public NPCQuestObjective(string description, int NPC_ID, int amount)
    {
        this.description = description;
        objectiveNeeded = NPC_ID;
        amountNeeded = amount;
        amountCompleted = 0;
    }
}
