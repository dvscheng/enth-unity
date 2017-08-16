using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestDialogue : MonoBehaviour {

    public NPC _NPC;
    public GameObject dialogue;
    public GameObject firstImage;
    public GameObject firstText;
    public GameObject secondImage;
    public GameObject secondText;
    public int firstItemID;
    public int firstAmount;
    public int secondItemID;
    public int secondAmount;
    bool initialized;

    // Use this for initialization
    void Awake () {
        initialized = false;
	}

    public void Initialize(NPC NPC)
    {
        /* Generate the items, get their sprites, and get the amounts-to-collect. */
        int numItems = Enum.GetValues(typeof(ItemDatabaseSO.ItemID)).Length-1;
        firstItemID = UnityEngine.Random.Range(0, numItems);
        secondItemID = UnityEngine.Random.Range(0, numItems);

        int tries = 3;
        while (firstItemID == secondItemID && tries > 0)
        {
            secondItemID = UnityEngine.Random.Range(1, numItems);
            tries--;
        }

        // FIX
        //firstAmount = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);
        //secondAmount = UnityEngine.Random.Range(1, CollectQuest.MAX_COLLECT_AMOUNT);

        /* Sets the sprites and amounts for the two items. */
        firstImage.GetComponent<Image>().sprite = ItemDatabaseSO.itemList[firstItemID].sprite;
        secondImage.GetComponent<Image>().sprite = ItemDatabaseSO.itemList[secondItemID].sprite;
        firstText.GetComponent<Text>().text = "" + firstAmount;
        secondText.GetComponent<Text>().text = "" + secondAmount;

        _NPC = NPC;

        initialized = true;
    }

    public void Accept()
    {
        if (!initialized)
        {
            Debug.Log("Item was not created in QuestDialogue; check that Initialize() was called.");
        }
        else
        {
            /* Initialize the prefab for the quest UI item. */
            //GameObject newQuest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestItem"), QuestTrackerUI.Instance.questObjectArea.transform);
            //CollectQuest quest = newQuest.GetComponent<CollectQuest>();
            //quest.Initialize(_NPC, firstItemID, secondItemID, firstAmount, secondAmount);
            //UIManager.Instance.dialogue.NPC.givenQuest = true;

            Close();
        }
    }

    /* Close the quest dialogue and reset the dialogue manager. */
    public void Close()
    {
        // delete the quest
        UIManager.Instance.dialogue.ResetDialogue();
        Destroy(gameObject);
    }
}
