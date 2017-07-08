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

    /* StartDialogue should ALWAYS be called first. */
    private void Update()
    {
        /* If users presses space, the dialogue UI is on and, just in case, StartDialogue has been called... */
        if (Inputs.Instance.interaction_key_down && UIManager.Instance.dialogueUIOn && NPC != null)
        {
            /* If we reach the end of the dialogue... */
            if (lineNumber >= textLines.Length)
            {
                if (NPC.IsQuestGiver)
                {
                    /* If the NPC is a quest giver AND this NPC's quest has been accepted... */
                    if (NPC.givenQuest)
                    {
                        /* If the quest is complete, give the rewards to the player. */
                        if (NPC.Quest.IsComplete)
                        {
                            NPC.Quest.Complete();
                        }
                        ResetDialogue();
                    }
                    else
                    {
                        /* If there isn't already a quest dialogue up, create one. */
                        if (questDialogueObj == null)
                        {
                            questDialogueObj = Instantiate(Resources.Load<GameObject>("Prefabs/QuestDialogue"), gameObject.transform);
                            questDialogueObj.GetComponent<QuestDialogue>().Initialize(_NPC);
                        }
                    }
                }
            }
            else
            {
                NPCDialogueText.text = textLines[lineNumber];
                lineNumber++;
            }
        }
    }

    public void NPCInteraction(NPCs NPC_ID, CollectQuest quest)
    {
        /* Get the NPC's dialogue values. */
        string[][] values = NPCData.valuesDictionary[NPC_ID.ID];
        string name = values[0][0];
        string spriteLocation = values[1][0];
        textLines = values[2];

        /* Create and map values to the Dialogue GameObject's name and portait. */
        newDialogue = Instantiate(Resources.Load<GameObject>("Prefabs/NPC Dialogue"), gameObject.transform);
        GameObject NPCName = newDialogue.transform.GetChild(0).gameObject;
        GameObject NPCPortait = newDialogue.transform.GetChild(1).gameObject;
        NPCName.GetComponent<Text>().text = name;
        NPCPortait.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteLocation);

        /* Handle displaying the text. */
        if (NPC_ID.IsQuestGiver && NPC_ID.givenQuest)
        {
            if (quest.IsComplete)
                textLines = values[3];
            else
                textLines = NPCData.QuestNotCompleteString;
        } else if (!NPC_ID.IsQuestGiver && NPC_ID.givenQuest)
        {
            /* NPC is not a questgiver but has given a quest. */
            textLines = values[4];
        }

        NPCDialogueText = newDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        NPCDialogueText.text = textLines[0];


        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);

        /* Indicates that initialization was a success. */
        _NPC = NPC_ID;
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
