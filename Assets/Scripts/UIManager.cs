using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private static UIManager _instance;   // singleton behaviour.
    public static UIManager Instance
    {
        get { return _instance; }
    }

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject player;
    [HideInInspector] public PlayerController playerController;

    public GameObject inventory;
    [HideInInspector] public PlayerInventory playerInventory;
    bool inventoryUIOn;

    public GameObject questTracker;
    bool questTrackerOn;

    public GameObject dialogue;
    [HideInInspector] public DialogueManager dialogueMan;
    public bool dialogueUIOn;

    public int playerHp;
    public Text hpText;

    enum Children
    {
        inventory,
        hpText
    }

    // Use this for initialization
    void Awake () {
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        inventoryUIOn = false;
        questTrackerOn = false;
        dialogueUIOn = false;

        playerController = player.GetComponent<PlayerController>();
        playerInventory = inventory.GetComponent<PlayerInventory>();
        dialogueMan = dialogue.GetComponent<DialogueManager>();
        playerHp = playerController.Hp;

        /* Turn on (initialize) the UI, then turn it back off (assumes they're hidden in Unity editor at start). */
        inventory.SetActive(true);
        inventory.SetActive(false);
        questTracker.SetActive(true);
        questTracker.SetActive(false);
        dialogue.SetActive(true);
        dialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* Check for UI toggles. */
        if (Inputs.Instance.inventory_key_down)
        {
            inventoryUIOn = !inventoryUIOn;
            inventory.SetActive(inventoryUIOn);
        }
        if (Inputs.Instance.quest_tracker_collapse_down)
        {
            questTrackerOn = !questTrackerOn;
            questTracker.SetActive(questTrackerOn);
        }


        /* Show HP. */
        playerHp = playerController.Hp;
        hpText.text = "HP: " + playerHp;
    }
}
