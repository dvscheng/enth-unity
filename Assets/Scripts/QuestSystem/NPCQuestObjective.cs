using UnityEngine;

public class NPCQuestObjective : QuestObjective {

    public override void OnObjectiveStart()
    {
        
    }
    
    public override void NotifyChange(int NPC_id)
    {
        if (NPC_id == objectiveNeeded)
        {
            isCompleted = true;
            amountCompleted = amountNeeded;
        }
    }

    /************************************
     Used in QuestDatabase ONLY
    ************************************/
    public NPCQuestObjective(string description, int NPC_ID, int amount)
    {
        this.description = description;
        sprite = Resources.Load<Sprite>(NPCDatabase.idToInfo[NPC_ID][(int)NPCDatabase.Slot.dialogueSprite][0]);                 // sprite directory
        objectiveNeeded = NPC_ID;
        amountNeeded = amount;
        amountCompleted = 0;
    }
}
