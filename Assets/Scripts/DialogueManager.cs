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
    int lineNumber = 0;

    // Use this for initialization
    void Awake () {
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
                NPCDialogueText.text = textLines[lineNumber];
                lineNumber++;
            }
        }
    }

    public void StartDialogue(int NPC_ID, bool isQuestGiver)
    {
        /* Get the NPC's dialogue values. */
        string[][] values = NPCData.valuesDictionary[NPC_ID];
        string name = values[0][0];
        string spriteLocation = values[1][0];
        textLines = values[2];

        /* Create and map values to the Dialogue GameObject's name and portait. */
        dialogue = Instantiate(Resources.Load<GameObject>("Prefabs/NPC Dialogue"), gameObject.transform);
        GameObject NPCName = dialogue.transform.GetChild(0).gameObject;
        GameObject NPCPortait = dialogue.transform.GetChild(1).gameObject;
        NPCName.GetComponent<Text>().text = name;
        NPCPortait.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteLocation);

        /* Handle displaying the text. */
        NPCDialogueText = dialogue.transform.GetChild(2).gameObject.GetComponent<Text>();

        ToggleOnOff();
        dialogueUIOn = true;
    }


    public void ResetDialogue()
    {
        if (dialogue != null)
        {
            Object.Destroy(dialogue);
            lineNumber = 0;
        }
    }

    public void ToggleOnOff()
    {
        dialogueUIOn = !dialogueUIOn;
        gameObject.SetActive(dialogueUIOn);
    }
}
