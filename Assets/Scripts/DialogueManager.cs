using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    NPCDatabase NPCData;

    /* Dialogue prefab. */
    GameObject newDialogue;
    NPCs _NPC;
    Text NPCDialogueText;
    string[] textLines;
    int lineNumber = 1;
    bool questCompleted = false;

    GameObject questDialogueObj;

    public NPCs NPC {
        get { return _NPC; }
    }

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
                /* If the NPC is a quest giver AND there isn't already a quest window, then create one. */
                if (questCompleted)
                {
                    questCompleted = false;
                    ResetDialogue();
                }
                if (NPC.isQuestGiver && NPC.Quest == null && questDialogueObj == null)
                {
                    questDialogueObj = Instantiate(Resources.Load<GameObject>("Prefabs/QuestDialogue"), gameObject.transform);
                    QuestDialogue questDialogue = questDialogueObj.GetComponent<QuestDialogue>();
                    questDialogue.Initialize(_NPC);
                }
                else if (NPC.isQuestGiver && NPC.Quest != null && NPC.Quest.IsComplete)
                {
                    /* Complete the quest and give the rewards to the player. */
                    questCompleted = true;
                    NPC.Quest.Complete();
                }
                else if (questDialogueObj == null)
                {
                    ResetDialogue();
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
        if (NPC_ID.isQuestGiver && NPC_ID.givenQuest && quest != null)
        {
            if (quest.IsComplete)
            {
                print("textline true");
                textLines = values[3];
            }
            else
            {
                textLines = NPCData.QuestNotCompleteString;

            }
        }
        NPCDialogueText = newDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        NPCDialogueText.text = textLines[0];


        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);

        /* Indicates that initialization was a success. */
        _NPC = NPC_ID;
    }

    /* Creates and displays the dialogue box. Assumes that there is text to be displayed. */
    public void StartDialogue(NPCs NPC_ID, bool isQuestGiver)
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
        NPCDialogueText = newDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        NPCDialogueText.text = textLines[0];

        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.dialogue, true);

        /* Indicates that initialization was a success. */
        _NPC = NPC_ID;
    }

    public void CompleteQuestDialogue(NPCs NPC, CollectQuest quest)
    {
        /* Get the NPC's dialogue values. */
        string[][] values = NPCData.valuesDictionary[NPC.ID];
        string name = values[0][0];
        string spriteLocation = values[1][0];
        textLines = values[3];

        /* Create and map values to the Dialogue GameObject's name and portait. */
        newDialogue = Instantiate(Resources.Load<GameObject>("Prefabs/NPC Dialogue"), gameObject.transform);
        GameObject NPCName = newDialogue.transform.GetChild(0).gameObject;
        GameObject NPCPortait = newDialogue.transform.GetChild(1).gameObject;
        NPCName.GetComponent<Text>().text = name;
        NPCPortait.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteLocation);

        /* Handle displaying the text. */
        NPCDialogueText = newDialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        NPCDialogueText.text = textLines[0];
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
