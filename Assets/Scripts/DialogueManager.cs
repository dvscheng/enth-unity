using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    NPCDatabase NPCData;
    GameObject questDialogueObj;
    public NPCs NPC {
        get { return _NPC; }
    }

    /* Dialogue prefab. */
    GameObject newDialogue;
    NPCs _NPC;
    Text NPCDialogueText;
    string[] textLines;
    int lineNumber = 1;
    bool resetNextIteration = false;

    

    // Use this for initialization
    void Awake () {
        NPCData = ScriptableObject.CreateInstance<NPCDatabase>();
	}

    void Update()
    {
        //questDialogueObj = Instantiate(Resources.Load<GameObject>("Prefabs/QuestDialogue"), gameObject.transform);
        //questDialogueObj.GetComponent<QuestDialogue>().Initialize(_NPC);
        
    }

    /* Notify that an NPC has been interacted with. */
    public void NPCInteraction(NPCs NPC)
    {
        GameObject newDialogue = Instantiate(Resources.Load<GameObject>("Prefabs/NPC Dialogue"), gameObject.transform);
        newDialogue.transform.GetChild(0).GetComponent<Text>().text = NPC.CharacterName;
        newDialogue.transform.GetChild(1).GetComponent<Image>().sprite = NPC.DialogueSprite;

        // check if a quest is finished
        // go through every quest, of the ones completed, is the end npc this one?
        // if so then complete it
        List<Quest> allReadyQuests = UIQuestTracker.Instance.AllQuests(true);
        foreach (Quest quest in allReadyQuests)
        {
            if (quest.EndNPC == NPC.ID)
            {
                // complete the quest and show the appropriate dialogue
                // change quest state to completed
                // notify proceeding quests
                quest.OnQuestComplete();
                return;
            }
        }

        // then check if a quest is to be given
        foreach (Quest quest in NPC.Quests)
        {
            if (quest.CurrentState == (int)Quest.State.qualified)
            {
                // start the quest
                // change quest state to incompleted
                return;
            }
        }

        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);

    }

    /* Destroys and resets the dialogue manager, including the quest dialogue if there is one. */
    public void ResetDialogue()
    {
        if (newDialogue != null)
        {
            Destroy(newDialogue);
            lineNumber = 1;
            UIManager.Instance.dialogueUIOn = false;
            gameObject.SetActive(false);
        }

        /* If a quest dialogue exists, destroy it too. */
        if (questDialogueObj != null)
            Destroy(questDialogueObj);
    }
}
