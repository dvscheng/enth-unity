using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    [HideInInspector] Inputs inputs;
    NPCDatabase NPCData;

    /* Dialogue prefab. */
    GameObject dialogue;

    /* If dialogueUIOn, then the text should exist, else it'll be null. */
    public bool dialogueUIOn;
    Text NPCDialogueText;
    string[] textLines;
    int lineNumber = 1;
    bool isQuestGiver;

    GameObject questDialogueObj;

    // Use this for initialization
    void Awake () {
        questDialogueObj = null;
        inputs = gameObject.AddComponent<Inputs>();
        NPCData = ScriptableObject.CreateInstance<NPCDatabase>();
        dialogueUIOn = true;
	}

    private void Update()
    {
        /* Handle displaying the text. */
        if (inputs.interaction_key_down)
        {
            if (dialogueUIOn && NPCDialogueText != null)
            {
                /* If we reach the end of the dialogue. */
                if (lineNumber == textLines.Length)
                {
                    if (isQuestGiver && questDialogueObj == null)
                    {
                        questDialogueObj = Instantiate(Resources.Load<GameObject>("Prefabs/QuestDialogue"), gameObject.transform);
                        QuestDialogue questDialogue = questDialogueObj.GetComponent<QuestDialogue>();
                        questDialogue.dialogueMan = this;
                        questDialogue.Initialize();
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
    }

    /* Creates and displays the dialogue box. Assumes that there is text to be displayed. */
    public void StartDialogue(int NPC_ID, bool isQuestGiver)
    {
        /* Get the NPC's dialogue values. */
        string[][] values = NPCData.valuesDictionary[NPC_ID];
        string name = values[0][0];
        string spriteLocation = values[1][0];
        textLines = values[2];
        this.isQuestGiver = isQuestGiver;

        /* Create and map values to the Dialogue GameObject's name and portait. */
        dialogue = Instantiate(Resources.Load<GameObject>("Prefabs/NPC Dialogue"), gameObject.transform);
        GameObject NPCName = dialogue.transform.GetChild(0).gameObject;
        GameObject NPCPortait = dialogue.transform.GetChild(1).gameObject;
        NPCName.GetComponent<Text>().text = name;
        NPCPortait.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteLocation);

        /* Handle displaying the text. */
        NPCDialogueText = dialogue.transform.GetChild(2).gameObject.GetComponent<Text>();
        NPCDialogueText.text = textLines[0];

        ToggleOnOff();
        dialogueUIOn = true;
    }

    /* Destroys and resets the dialogue manager, including the quest dialogue if there is one. */
    public void ResetDialogue()
    {
        if (dialogue != null)
        {
            Destroy(dialogue);
            lineNumber = 1;
            ToggleOnOff();
        }

        /* If a quest dialogue exists, destroy it too. */
        if (questDialogueObj != null)
        {
            Destroy(questDialogueObj);
        }
    }

    /* Toggles this UI's display on or off in Unity. */
    public void ToggleOnOff()
    {
        dialogueUIOn = !dialogueUIOn;
        gameObject.SetActive(dialogueUIOn);
    }
}
