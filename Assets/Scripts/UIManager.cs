using System.Collections.Generic;
using UnityEngine.EventSystems;
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
    Vector2 hotSpot;

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

    public GameObject insigniaObj;
    [HideInInspector]
    public InsigniaPanel insigniaPanel;
    bool insigniaOn;

    public GameObject journalObj;
    [HideInInspector]
    public Journal journal;
    bool journalOn;

    public GameObject healthBarObj;
    [HideInInspector]
    public HealthBar healthBar;
    bool healthBarOn;

    public GameObject expBarObj;
    [HideInInspector]
    public EXPBar expBar;
    bool expBarOn;

    public GameObject itemTooltipObj;
    [HideInInspector]
    public ItemTooltip itemTooltip;
    bool itemTooltipOn;


    public int playerHp;
    public Text hpText;

    public enum UI_Type
    {
        inventory,
        questTracker,
        dialogue,
        insigniaPanel,
        journal,
        itemTooltip
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
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);   // middle of the crossheir

        inventoryUIOn = false;
        questTrackerOn = false;
        dialogueUIOn = false;
        insigniaOn = false;
        journalOn = false;
        healthBarOn = true;
        expBarOn = true;

        playerController = player.GetComponent<PlayerController>();
        playerInventory = inventory.GetComponent<PlayerInventory>();
        dialogue = dialogueObj.GetComponent<DialogueManager>();
        insigniaPanel = insigniaObj.GetComponent<InsigniaPanel>();
        healthBar = healthBarObj.GetComponent<HealthBar>();
        journal = journalObj.GetComponent<Journal>();
        expBar = expBarObj.GetComponent<EXPBar>();
        itemTooltip = itemTooltipObj.GetComponent<ItemTooltip>();
        playerHp = playerController.Hp;

        /* Turn on (initialize) the UI, then turn it back off (assumes they're hidden in Unity editor at start). */
        inventory.SetActive(true);
        inventory.SetActive(false);
        questTracker.SetActive(true);
        questTracker.SetActive(false);
        dialogueObj.SetActive(true);
        dialogueObj.SetActive(false);
        insigniaObj.SetActive(true);
        insigniaObj.SetActive(false);
        journalObj.SetActive(true);
        journalObj.SetActive(false);
        itemTooltipObj.SetActive(true);
        itemTooltipObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursor();

        /* Check for UI toggles. */
        if (Inputs.Instance.inventory_key_down)
        {
            TurnOnOffUI(UI_Type.inventory, !inventoryUIOn);
        }
        if (Inputs.Instance.quest_tracker_collapse_down)
        {
            TurnOnOffUI(UI_Type.questTracker, !questTrackerOn);
        }
        if (Inputs.Instance.insignia_key_down)
        {
            TurnOnOffUI(UI_Type.insigniaPanel, !insigniaOn);
        }
        if (Inputs.Instance.journal_key_down)
        {
            TurnOnOffUI(UI_Type.journal, !journalOn);
        }

        /* Show HP. (move elsewhere later) */
        playerHp = playerController.Hp;
        hpText.text = "HP: " + playerHp;
    }

    void UpdateCursor()
    {
        // OnMouseEnter changes cursor would be better, but find a way to change it for ALL UI elements at the same time
        /* Cursor; if the cursor is over a UI element, reset the cursor. Else set the custom one. */
        if (EventSystem.current.IsPointerOverGameObject())
        {
            /*
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach (var obj in raycastResults)
                {
                    if (obj.gameObject.CompareTag("Damage Text"))
                    {
                        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                    }
                }
            }
            */
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        else
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    /* Given the UI type to manipualte, turn it OnOff. */
    public void TurnOnOffUI(UI_Type type, bool OnOff)
    {
        switch (type)
        {
            case (UI_Type.inventory):
                inventoryUIOn = OnOff;
                inventory.SetActive(OnOff);
                if (!OnOff)
                    TurnOnOffUI(UI_Type.itemTooltip, OnOff);            // turn off the item tooltip as well
                break;

            case (UI_Type.questTracker):
                questTrackerOn = OnOff;
                questTracker.SetActive(OnOff);
                break;

            case (UI_Type.dialogue):
                dialogueUIOn = OnOff;
                dialogueObj.SetActive(OnOff);
                break;

            case (UI_Type.insigniaPanel):
                insigniaOn = OnOff;
                insigniaObj.SetActive(OnOff);
                break;

            case (UI_Type.journal):
                journalOn = OnOff;
                journalObj.SetActive(OnOff);
                break;

            case (UI_Type.itemTooltip):
                itemTooltipOn = OnOff;
                itemTooltipObj.SetActive(OnOff);
                break;
        }
    }

    /* Called by PlayerController on level up. */
    public void RefreshStats()
    {
        healthBar.UpdateHealth();
        insigniaPanel.Refresh();
        expBar.UpdateExp();
    }
}
