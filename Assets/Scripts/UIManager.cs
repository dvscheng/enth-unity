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

    public GameObject healthBarObj;
    [HideInInspector]
    public HealthBar healthBar;
    bool healthBarOn;

    public GameObject expBarObj;
    [HideInInspector]
    public EXPBar expBar;
    bool expBarOn;


    public int playerHp;
    public Text hpText;

    public enum UI_Type
    {
        inventory,
        questTracker,
        dialogue,
        insigniaPanel
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

        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);   // middle of the crossheir

        inventoryUIOn = false;
        questTrackerOn = false;
        dialogueUIOn = false;
        insigniaOn = false;
        healthBarOn = true;
        expBarOn = true;

        playerController = player.GetComponent<PlayerController>();
        playerInventory = inventory.GetComponent<PlayerInventory>();
        dialogue = dialogueObj.GetComponent<DialogueManager>();
        insigniaPanel = insigniaObj.GetComponent<InsigniaPanel>();
        healthBar = healthBarObj.GetComponent<HealthBar>();
        expBar = expBarObj.GetComponent<EXPBar>();
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
    }

    // Update is called once per frame
    void Update()
    {
        // OnMouseEnter changes cursor would be better, but find a way to change it for ALL UI elements at the same time
        /* Cursor; if the cursor is over a UI element, reset the cursor. Else set the custom one. */
        if (EventSystem.current.IsPointerOverGameObject())
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        else
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

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

            case (UI_Type.insigniaPanel):
                insigniaOn = OnOff;
                insigniaObj.SetActive(OnOff);
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
