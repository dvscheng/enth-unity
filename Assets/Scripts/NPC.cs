using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    /* Unity editor */
    public int ID;

    private string characterName;
    public string CharacterName
    {
        get
        {
            return characterName;
        }
    }

    private Sprite characterSprite;
    public Sprite CharacterSprite
    {
        get
        {
            return characterSprite;
        }
    }

    private Sprite dialogueSprite;
    public Sprite DialogueSprite
    {
        get
        {
            return dialogueSprite;
        }
    }

    private List<Quest> quests;
    public List<Quest> Quests
    {
        get
        {
            return quests;
        }
    }

    /* Used to notify subscribers that an NPC has been interacted with. */
    public delegate void NotifyNPCInteration(int NPC_ID);
    public static event NotifyNPCInteration OnNPCInteraction;

    public bool IsQuestGiver { get; set; }
    public bool givenQuest = false;
    public CircleCollider2D interactRange;
    public Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        /* If the NPC was already initialized, take the data from NPCDatabase. */
        if (NPCDatabase.npcObjects.ContainsKey(ID))                                         // temporary fix for unity creating another NPC on scene load.
        {                                                                                   // references the NPCDatabase NPC object for info.
            NPC originalNPC = NPCDatabase.npcObjects[ID];                                   // TODO: find a way to switch references to NPC script on the GameObject
            characterName = originalNPC.characterName;
            characterSprite = originalNPC.characterSprite;
            dialogueSprite = originalNPC.dialogueSprite;
            quests = originalNPC.quests;
        }
        else
        {
            if (NPCDatabase.idToInfo.ContainsKey(ID))
            {
                string[][] info = NPCDatabase.idToInfo[ID];
                string name = info[0][0];
                string characterSpriteDirectory = info[1][0];
                string dialogueSpriteDirectory = info[2][0];

                characterName = name;
                characterSprite = Resources.Load<Sprite>(characterSpriteDirectory);
                dialogueSprite = Resources.Load<Sprite>(dialogueSpriteDirectory);
            }
            if (QuestDatabase.NPCIDToQuests.ContainsKey(ID))
            {
                IsQuestGiver = true;
                quests = QuestDatabase.NPCIDToQuests[ID];                                       // TODO: decide whether you need reference or copy of the list
            }
            else
            {
                IsQuestGiver = false;
                quests = new List<Quest>();                                                     // just create an empty list of quests for no bug when checking for dialogue
            }

            /* Temporary fix, look above for info. */
            NPCDatabase.InitializeNPC(ID, this);
        }
    }

    /* Keeps the rigidbody awake to check for player interaction. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player") && Inputs.Instance.interaction_key_down && !UIManager.Instance.dialogueUIOn)         // consider changing the check for whether user is talking
        {
            if (OnNPCInteraction != null)
            {
                OnNPCInteraction(ID);
            }
            UIManager.Instance.dialogue.NPCInteraction(this);
        }
    }

    /* Reduces the physics checks of this trigger by allowing it to sleep. */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            if (UIManager.Instance.dialogueUIOn)
            {
                UIManager.Instance.dialogue.ResetDialogue();
            }
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }
}
