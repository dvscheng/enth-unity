using UnityEngine;
using UnityEngine.UI;

/*
Quest Pop-up prefab:
    Quest Pop-up
        Quest Objectives
        Quest Title
        NPC Icon BG
            NPC Icon Sprite
        Accept and Decline Buttons
            Accept
            Decline
*/
public class QuestPopup : MonoBehaviour {
    private Quest quest;
    public Quest Quest
    {
        get
        {
            return quest;
        }
    }

    /* Unity Editor. */
    [SerializeField] GameObject questObjectiveAreaGO;
    [SerializeField] GameObject questTitleGO;
    [SerializeField] GameObject questIconGO;

    /* Must be called; links a quest to this popup. */
    public void Initialize(Quest quest, NPCs startingNPC)
    {
        this.quest = quest;

        /* Initialize the QuestObjectiveUI GameObject. */
        foreach (QuestObjective objective in quest.QuestObjectives)
        {
            GameObject objectiveUI = Instantiate(Resources.Load<GameObject>("Prefabs/QuestSystem/Quest Pop-up/Quest Objective"), questObjectiveAreaGO.transform);
            objectiveUI.transform.GetChild(0).GetComponent<Image>().sprite = objective.Sprite;
            objectiveUI.transform.GetChild(1).GetComponent<Text>().text = objective.AmountCompleted + "/" + objective.AmountNeeded;
            objectiveUI.transform.GetChild(2).GetComponent<Text>().text = objective.Description;
        }
        questTitleGO.GetComponent<Text>().text = quest.QuestTitle;
        questIconGO.GetComponent<Image>().sprite = startingNPC.DialogueSprite;
    }

    /* Linked to a button. */
    public void AcceptQuest()
    {
        UIQuestTracker.Instance.AddQuest(quest);
        quest.OnQuestAccept();
    }

    /* Linked to a button. */
    public void DeclineQuest()
    {
        UIManager.Instance.dialogue.ResetDialogue();
        Destroy(gameObject);
    }
}
