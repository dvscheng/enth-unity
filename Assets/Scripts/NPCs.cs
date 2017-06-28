using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    public int ID;
    public bool IsQuestGiver { get; set; }
    public bool givenQuest = false;
    public CollectQuest Quest { get; set; }
    public CircleCollider2D interactRange;
    public Rigidbody2D rb;

    // Use this for initialization
    void Awake()
    {
        IsQuestGiver = true;
        Quest = null;
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
        if (collision.gameObject.name.Equals("Player"))
        {
            if (!UIManager.Instance.dialogueUIOn && Inputs.Instance.interaction_key_down)
                UIManager.Instance.dialogue.NPCInteraction(this, Quest);
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
