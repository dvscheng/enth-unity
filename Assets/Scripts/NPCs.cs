using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    public int ID;
    public bool isQuestGiver;
    public CircleCollider2D interactRange;
    public Rigidbody2D rb;
    public GameObject DialogueUIObj;
    DialogueManager DialogueMan;
    [HideInInspector] Inputs inputs;

    // Use this for initialization
    void Awake()
    {
        inputs = gameObject.AddComponent<Inputs>();
        DialogueMan = DialogueUIObj.GetComponent<DialogueManager>();
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
            if (!DialogueMan.dialogueUIOn && inputs.interaction_key_down)
            {
                DialogueMan.StartDialogue(ID, isQuestGiver);
                
                // second "page" will have buttons (green check red x) to click on to accept or not
            }
        }
    }

    /* Reduces the physics checks of this trigger by allowing it to sleep. */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            if(DialogueMan.dialogueUIOn)
            {
                DialogueMan.ResetDialogue();
            }
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }
}
