using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    private NPCDatabase NPCData;
    private QuestDatabase questData;
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
        IsQuestGiver = true;

        NPCData = ScriptableObject.CreateInstance<NPCDatabase>();
        questData = ScriptableObject.CreateInstance<QuestDatabase>();

        if (NPCData.idToDialogues.ContainsKey(ID))
        {
            string[][] values = NPCData.idToDialogues[ID];
            string name = values[0][0];
            string characterSpriteDirectory = values[1][0];
            string dialogueSpriteDirectory = values[2][0];

            characterName = name;
            characterSprite = Resources.Load<Sprite>(characterSpriteDirectory);
            dialogueSprite = Resources.Load<Sprite>(dialogueSpriteDirectory);
        }
        if (questData.npcIDToQuests.ContainsKey(ID))
        {
            quests = questData.npcIDToQuests[ID];                                       // TODO: decide whether you need reference or copy of the list
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
            //UIManager.Instance.dialogue.NPCInteraction(this, Quest);
            UIManager.Instance.dialogue.NPCInteraction(this);
            if (OnNPCInteraction != null)
            {
                OnNPCInteraction(ID);
            }
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
