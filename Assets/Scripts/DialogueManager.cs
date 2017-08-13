using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    /* Unity Editor */
    [SerializeField] Text NPCName;
    [SerializeField] Image NPCDialogueSprite;
    [SerializeField] Text text;

    private Queue<string> currentDialogue;

    private bool questIsUp;
    private bool toGiveQuest;

    private Quest questToGive;
    private QuestPopup questPopup;

    private NPCs interactingNPC;
    

    // Use this for initialization
    void Awake () {
        currentDialogue = new Queue<string>();
        questIsUp = false;
        toGiveQuest = false;
        questToGive = null;
        interactingNPC = null;
    }

    void Update()
    {
        if (Inputs.Instance.interaction_key_down && !questIsUp)
        {
            ProceedDialogue();
        }
    }

    /* Notify that an NPC has been interacted with. */
    public void NPCInteraction(NPCs NPC)
    {
        NPCName.text = NPC.CharacterName;
        NPCDialogueSprite.sprite = NPC.DialogueSprite;

        // check if a quest is finished
        // go through every quest, of the ones completed, is the end npc this one?
        // if so then complete it
        List<Quest> allReadyQuests = UIQuestTracker.Instance.AllQuests(true);
        foreach (Quest quest in allReadyQuests)
        {
            if (quest.EndNPC == NPC.ID)
            {
                quest.OnQuestComplete();
                StartDialogue(quest.CompleteDialogue, false);
                return;
            }
        }

        // then check if a quest is to be given
        foreach (Quest quest in NPC.Quests)
        {
            if (quest.CurrentState == (int)Quest.State.qualified)
            {
                questToGive = quest;
                interactingNPC = NPC;
                StartDialogue(quest.StartDialogue, true);
                return;
            }
        }

        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);

    }

    public void ProceedDialogue()
    {
        if (currentDialogue.Count > 0)
        {
            text.text = currentDialogue.Dequeue();
        }
        else if (toGiveQuest)
        {
            if (questToGive != null && interactingNPC != null)
                ShowQuestPopup(questToGive, interactingNPC);
            else
                Debug.Log("Tried to show quest, but quest or npc was null.");
        }
        else
        {
            ResetDialogue();
        }
    }

    /* Load the dialogue into the currentDialogue queue for display. */
    private void StartDialogue(string[] lines, bool isGivingQuest)
    {
        // could initialize it
        foreach (string line in lines)
            currentDialogue.Enqueue(line);

        toGiveQuest = isGivingQuest;

        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);
        ProceedDialogue();
    }

    /* Shows the QuestPopup window. */
    private void ShowQuestPopup(Quest quest, NPCs startingNPC)
    {
        GameObject questPopupObj = Instantiate(Resources.Load<GameObject>("Prefabs/QuestSystem/Quest Pop-up/Quest Pop-up"), gameObject.transform);
        questPopup = questPopupObj.GetComponent<QuestPopup>();
        questPopup.Initialize(quest, startingNPC);
        questIsUp = true;
    }

    /* Destroys and resets the dialogue manager, including the quest dialogue if there is one. */
    public void ResetDialogue()
    {
        if (questPopup != null)
            questPopup.ResetDialogue();
        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, false);
        currentDialogue.Clear();
        toGiveQuest = false;
        questIsUp = false;
        questToGive = null;
        interactingNPC = null;
    }
}
