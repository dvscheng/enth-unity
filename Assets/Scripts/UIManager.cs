using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject player;
    [HideInInspector] public PlayerController playerController;
    public GameObject inventory;
    [HideInInspector] public PlayerInventory playerInventory;
    Inputs inputs;
    public int playerHp;
    public Text hpText;

    enum Children
    {
        inventory,
        hpText
    }

    // Use this for initialization
    void Awake () {
        inputs = gameObject.AddComponent<Inputs>();
        playerController = player.GetComponent<PlayerController>();
        playerInventory = inventory.GetComponent<PlayerInventory>();
        playerHp = playerController.Hp;

        /* Turn on (initialize) the UI, then turn it back off. */
        playerInventory.ToggleOnOff();
        playerInventory.ToggleOnOff();
    }

    // Update is called once per frame
    void Update()
    {
        /* Check for UI toggles. */
        if (inputs.inventory_key_down)
        {
            playerInventory.ToggleOnOff();
        }

        /* Show HP. */
        playerHp = playerController.Hp;
        hpText.text = "HP: " + playerHp;
    }
}
