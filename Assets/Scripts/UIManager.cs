using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    #region Singleton Behaviour
    private static UIManager _instance;   // singleton behaviour.
    public static UIManager Instance
    {
        get { return _instance; }
    }
    #endregion

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;

    public GameObject inventory;
    [HideInInspector]
    public PlayerInventory playerInventory;
    bool inventoryUIOn;

    public GameObject dialogueObj;
    [HideInInspector]
    public DialogueManager dialogue;
    public bool dialogueUIOn;

    public GameObject questTracker;
    bool questTrackerOn;


    public int playerHp;
    public Text hpText;

    public enum UI_Type
    {
        inventory,
        questTracker,
        dialogue
    }

    // Use this for initialization
    void Awake () {
        #region Singleton Behaviour
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
        #endregion

        inventoryUIOn = false;
        questTrackerOn = false;
        dialogueUIOn = false;

        playerController = player.GetComponent<PlayerController>();
        playerInventory = inventory.GetComponent<PlayerInventory>();
        dialogue = dialogueObj.GetComponent<DialogueManager>();
        playerHp = playerController.Hp;

        /* Turn on (initialize) the UI, then turn it back off (assumes they're hidden in Unity editor at start). */
        inventory.SetActive(true);
        inventory.SetActive(false);
        questTracker.SetActive(true);
        questTracker.SetActive(false);
        dialogueObj.SetActive(true);
        dialogueObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* Check for UI toggles. */
        if (Inputs.Instance.inventory_key_down)
        {
            TurnOnOffUI(UI_Type.inventory, !inventoryUIOn);
        }
        if (Inputs.Instance.quest_tracker_collapse_down)
        {
            TurnOnOffUI(UI_Type.questTracker, !questTrackerOn);
        }

        /* Show HP. (move elsewhere later) */
        playerHp = playerController.Hp;
        hpText.text = "HP: " + playerHp;
    }

    /* Given the UI type to manipualte, turn it OnOff. */
    public void TurnOnOffUI(UI_Type type, bool OnOff)
    {
        switch (type)
        {
            case (UI_Type.inventory):
                inventoryUIOn = OnOff;
                inventory.SetActive(OnOff);
                break;

            case (UI_Type.questTracker):
                questTrackerOn = OnOff;
                questTracker.SetActive(OnOff);
                break;

            case (UI_Type.dialogue):
                dialogueUIOn = OnOff;
                dialogueObj.SetActive(OnOff);
                break;
        }
    }
}
