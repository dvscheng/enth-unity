using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestDialogue : MonoBehaviour {
    
    ItemDatabase itemDB;
    CollectQuest newQuest;
    public DialogueManager dialogueMan;
    public GameObject dialogue;
    public GameObject firstImage;
    public int firstID;
    public GameObject firstText;
    public GameObject secondImage;
    public int secondID;
    public GameObject secondText;

    // Use this for initialization
    void Awake () {
        newQuest = null;
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();
	}

    public void Initialize()
    {
        /* Generate the items, get their sprites, and get the amounts-to-collect. */
        int numItems = Enum.GetValues(typeof(ItemDatabase.ItemID)).Length;
        int firstItemID = UnityEngine.Random.Range(1, numItems);
        int secondItemID = UnityEngine.Random.Range(1, numItems);
        while (firstItemID == secondItemID)
        {
            secondItemID = UnityEngine.Random.Range(1, numItems);
        }
        firstID = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);
        secondID = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);
        newQuest = new CollectQuest(firstItemID, secondItemID, firstID, secondID);

        /* Sets the sprites and amounts for the two items. */
        firstImage.GetComponent<Image>().sprite = itemDB.itemDictionary[firstItemID];
        secondImage.GetComponent<Image>().sprite = itemDB.itemDictionary[secondItemID];
        firstText.GetComponent<Text>().text = "" + firstID;
        secondText.GetComponent<Text>().text = "" + secondID;
    }

    public void Accept()
    {
        if (newQuest == null)
        {
            Debug.Log("Item was not created in QuestDialogue; check that Initialize() was called.");
        }
        else
        {
            newQuest.NotifyQuestTrackerUI();
            dialogueMan.ResetDialogue();
            Destroy(gameObject);
        }
    }

    public void Decline()
    {
        // delete the quest
        dialogueMan.ResetDialogue();
        Destroy(gameObject);
    }
}
