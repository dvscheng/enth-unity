using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestDialogue : MonoBehaviour {
    
    ItemDatabase itemDB;
    GameObject newQuest;
    [HideInInspector] public DialogueManager dialogueMan;


    public GameObject dialogue;
    public GameObject firstImage;
    public GameObject firstText;
    public GameObject secondImage;
    public GameObject secondText;
    public int firstItemID;
    public int firstAmount;
    public int secondItemID;
    public int secondAmount;

    // Use this for initialization
    void Awake () {
        newQuest = null;
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();
	}

    public void Initialize()
    {
        /* Generate the items, get their sprites, and get the amounts-to-collect. */
        int numItems = Enum.GetValues(typeof(ItemDatabase.ItemID)).Length;
        firstItemID = UnityEngine.Random.Range(1, numItems);
        secondItemID = UnityEngine.Random.Range(1, numItems);
        while (firstItemID == secondItemID)
        {
            secondItemID = UnityEngine.Random.Range(1, numItems);
        }
        firstAmount = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);
        secondAmount = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);

        /* Sets the sprites and amounts for the two items. */
        firstImage.GetComponent<Image>().sprite = itemDB.itemDictionary[firstItemID];
        secondImage.GetComponent<Image>().sprite = itemDB.itemDictionary[secondItemID];
        firstText.GetComponent<Text>().text = "" + firstAmount;
        secondText.GetComponent<Text>().text = "" + secondAmount;
    }

    public void Accept()
    {
        if (newQuest == null)
            Debug.Log("Item was not created in QuestDialogue; check that Initialize() was called.");
        else
        {
            /* Initialize the prefab for the quest UI item. */
            newQuest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestItem"), QuestTrackerUI.Instance.questObjectArea.transform);
            newQuest.GetComponent<CollectQuest>().Initialize(firstItemID, secondItemID, firstAmount, secondAmount);
            Close();
        }
    }

    /* Close the quest dialogue and reset the dialogue manager. */
    public void Close()
    {
        // delete the quest
        dialogueMan.ResetDialogue();
        Destroy(gameObject);
    }
}
