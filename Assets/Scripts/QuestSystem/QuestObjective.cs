using UnityEngine;

public abstract class QuestObjective {
    protected bool isCompleted;
    public bool IsCompleted {
        get
        {
            return isCompleted;
        }
    }
    protected string description;
    public string Description
    {
        get
        {
            return description;
        }
    }
    protected int objectiveNeeded;
    public int ObjectiveNeeded
    {
        get
        {
            return objectiveNeeded;
        }
    }
    protected int amountNeeded;
    public int AmountNeeded
    {
        get
        {
            return amountNeeded;
        }
    }
    protected int amountCompleted;
    public int AmountCompleted
    {
        get
        {
            return amountCompleted;
        }
    }
    protected Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    /* Do anything that the objective needs to check when the quest is accepted. */
    public virtual void OnObjectiveStart()
    {
        /* Do nothing.
         * Allow children to modify if needed.*/
    }

    /* Notify objectives that there has been a change. */
    public virtual void NotifyChange(int itemID, int amount)
    {
        /* Do nothing.
         * Allow children to modify if needed.*/
    }
    public virtual void NotifyChange(int NPC_ID)
    {
        /* Do nothing.
         * Allow children to modify if needed.*/
    }
}
