using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    #region Singleton Behaviour
    private static PlayerInventory _instance;   // singleton behaviour
    public static PlayerInventory Instance
    {
        get { return _instance; }
    }
    #endregion

    ItemDatabaseSO itemData;

    private readonly int WIDTH = 4;
    private readonly int HEIGHT = 6;
    public GameObject equipmentGridGameObj;
    public GameObject useGridGameObj;
    public GameObject materialsGridGameObj;
    public ItemSlot[,] equipmentGrid;
    public ItemSlot[,] useGrid;
    public ItemSlot[,] materialsGrid;
    /* ItemID, [y, x] */
    public Dictionary<int, int[]> equipmentGridLocation;
    public Dictionary<int, int[]> useGridLocation;
    public Dictionary<int, int[]> materialsGridLocation;
    /* Actually is last used slot. */
    public int[] equipNextAvailableSlot;
    public int[] useNextAvailableSlot;
    public int[] matsNextAvailableSlot;
    public int numEquips = 0;
    public int numUse = 0;
    public int numMats = 0;
    /* Used to notify subscribers that an item has been added. */
    public delegate void NotifyAddedItem(int itemID, int amount);
    public static event NotifyAddedItem OnAddItem;

    /* Should be referenced in Unity editor. */
    [HideInInspector] public Sprite[] buttonOnSprites;
    [HideInInspector] public Sprite[] buttonOffSprites;
    public Button equipTabButton;
    public Button useTabButton;
    public Button matsTabButton;

    public int tabInFocus;  // 0 = equips, 1 = use, 2 = mats


    public void Awake()
    {
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
        #endregion
        //DontDestroyOnLoad(gameObject); already in UI

        /* Load in the inventory tab buttons. */
        buttonOnSprites = Resources.LoadAll<Sprite>("Sprites/spr_inventory_tab_focused");
        buttonOffSprites = Resources.LoadAll<Sprite>("Sprites/spr_inventory_tab_not_focused");

        /* Initialize the grid prefabs for Unity, initialize the grid arrays, then adjust the ItemSlots. */
        equipmentGridGameObj = (GameObject)Instantiate(Resources.Load("Prefabs/InventoryGrid Multiple 1"), gameObject.transform);
        useGridGameObj = (GameObject)Instantiate(Resources.Load("Prefabs/InventoryGrid Multiple 1"), gameObject.transform);
        materialsGridGameObj = (GameObject)Instantiate(Resources.Load("Prefabs/InventoryGrid Multiple 1"), gameObject.transform);
        equipmentGridGameObj.name = "Equipment Grid";
        useGridGameObj.name = "Useables Grid";
        materialsGridGameObj.name = "Materials Grid";
        equipmentGrid = new ItemSlot[HEIGHT, WIDTH];
        useGrid = new ItemSlot[HEIGHT, WIDTH];
        materialsGrid = new ItemSlot[HEIGHT, WIDTH];
        /* Add the ItemSlots to the array that PlayerInventory will control. */
        GameObject equipItemSlot;
        GameObject useItemSlot;
        GameObject matsItemSlot;
        for (int i = 0; i < HEIGHT*WIDTH; i++)
        {
            int x = i % WIDTH;
            int y = i / WIDTH;

            equipItemSlot = equipmentGridGameObj.transform.GetChild(y).gameObject;
            useItemSlot = useGridGameObj.transform.GetChild(y).gameObject;
            matsItemSlot = materialsGridGameObj.transform.GetChild(y).gameObject;

            equipmentGrid[y, x] = equipItemSlot.GetComponent<ItemSlot>();
            useGrid[y, x] = useItemSlot.GetComponent<ItemSlot>();
            materialsGrid[y, x] = matsItemSlot.GetComponent<ItemSlot>();
        }

        /* Initialize the count grids. */
        equipmentGridLocation = new Dictionary<int, int[]>();
        useGridLocation = new Dictionary<int, int[]>();
        materialsGridLocation = new Dictionary<int, int[]>();


        /* Default grid on start is Mats. */
        equipmentGridGameObj.SetActive(false);
        useGridGameObj.SetActive(false);

        /* (x, y) */
        equipNextAvailableSlot = new int[2];
        useNextAvailableSlot = new int[2];
        matsNextAvailableSlot = new int[2];

        tabInFocus = (int)ItemDatabaseSO.ItemType.mats;
    }

    /* Gets the correct grid depending on the item's type; returns true IFF item is added successfully. */
    public bool AddToInventory(ItemObject item, int amount)
    {
        bool addedSuccessfully = false;
        /* Depending on the item's type, update the appropriate grid and add the item to the new slot/increase the count (assumes accessing array in dictionary
         *  is not a reference) . */
        switch (item.type)
        {
            case (int)ItemDatabaseSO.ItemType.equip:
                addedSuccessfully = AddItemToSpecificGrid(equipmentGridLocation, equipmentGrid, ref equipNextAvailableSlot, ref numEquips, item, amount);
                break;

            case (int)ItemDatabaseSO.ItemType.use:
                addedSuccessfully = AddItemToSpecificGrid(useGridLocation, useGrid, ref useNextAvailableSlot, ref numUse, item, amount);
                break;

            case (int)ItemDatabaseSO.ItemType.mats:
                addedSuccessfully = AddItemToSpecificGrid(materialsGridLocation, materialsGrid, ref matsNextAvailableSlot, ref numMats, item, amount);
                break;
        }

        if (addedSuccessfully && OnAddItem != null)
        {
            OnAddItem(item.id, amount);
        }

        return addedSuccessfully;
    }

    //TODO check ref on numOfType in unity editor
    private bool AddItemToSpecificGrid(Dictionary<int, int[]> gridLocation, ItemSlot[,] grid, ref int[] nextAvailableSlot, ref int numOfType, ItemObject item, int amount)
    {
        int itemID = item.id;
        if (gridLocation.ContainsKey(itemID))
        {
            int[] itemSlotLocation = gridLocation[itemID];
            grid[itemSlotLocation[0], itemSlotLocation[1]].AddToExistingItem(amount);

            AudioManager.Instance.Play("Item Pickup");
            return true;
        }
        else if (FindNextAvailableSlot(grid, ref nextAvailableSlot))
        {
            grid[nextAvailableSlot[1], nextAvailableSlot[0]].SetItemAndSprite(item, amount);
            gridLocation.Add(item.id, new int[2] { nextAvailableSlot[1], nextAvailableSlot[0] });
            numOfType++;

            AudioManager.Instance.Play("Item Pickup");
            return true;
        }

        return false;
    }

    /* Remove an item from the inventory. */
    public bool RemoveFromInventory(int itemID, int itemType, int amount)
    {
        switch (itemType)
        {
            case (int)ItemDatabaseSO.ItemType.equip:
                return RemoveItemFromSpecificGrid(equipmentGridLocation, equipmentGrid, ref equipNextAvailableSlot, ref numEquips, itemID, amount);

            case (int)ItemDatabaseSO.ItemType.use:
                return RemoveItemFromSpecificGrid(useGridLocation, useGrid, ref useNextAvailableSlot, ref numUse, itemID, amount);

            case (int)ItemDatabaseSO.ItemType.mats:
                return RemoveItemFromSpecificGrid(materialsGridLocation, materialsGrid, ref matsNextAvailableSlot, ref numMats, itemID, amount);
        }
        return false;
    }

    //TODO check ref on numOfType in unity editor
    private bool RemoveItemFromSpecificGrid(Dictionary<int, int[]> gridLocation, ItemSlot[,] grid, ref int[] nextAvailableSlot, ref int numOfType, int itemID, int amount)
    {
        if (gridLocation.ContainsKey(itemID))
        {
            int[] itemSlotLocation = gridLocation[itemID];
            grid[itemSlotLocation[0], itemSlotLocation[1]].RemoveFromExistingItem(amount);
            FindNextAvailableSlot(grid, ref nextAvailableSlot);
            return true;
        }
        return false;
    }

    /* Returns true IFF there is an available slot in the given grid. */
    private bool FindNextAvailableSlot(ItemSlot[,] inventoryGrid, ref int[] whichAvailableSlot)
    {
        ItemSlot itemSlot;
        for (int y = 0; y < inventoryGrid.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryGrid.GetLength(1); x++)
            {
                itemSlot = inventoryGrid[y, x];
                if (!itemSlot.HasItem)
                {
                    whichAvailableSlot[0] = x;
                    whichAvailableSlot[1] = y;
                    return true;
                }
            }
        }
        return false;
    }

    /* Returns the amount of the item in the inventory, else returns -1. */
    public int FindInInventory(int itemID)
    {
        int type = itemData.itemList[itemID].type;
        ItemSlot[,] grid;

        switch (type)
        {
            case (int)ItemDatabaseSO.ItemType.equip:
                grid = equipmentGrid;
                break;

            case (int)ItemDatabaseSO.ItemType.use:
                grid = useGrid;
                break;

            default:
            case (int)ItemDatabaseSO.ItemType.mats:
                grid = materialsGrid;
                break;
        }

        ItemSlot itemSlot;
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                itemSlot = grid[y, x];
                if (itemSlot.HasItem && itemSlot.item.id == itemID)
                {
                    return itemSlot.amount;
                }
            }
        }
        return -1;
    }

    /* Sets a tab to be in focus, changing the tab sprite and grid displayed. */
    public void SetTabInFocus(int tabNumber)
    {
        tabInFocus = tabNumber;

        if (tabNumber == 0)
        {
            equipmentGridGameObj.SetActive(true);
            equipTabButton.image.overrideSprite = buttonOnSprites[0];

            useGridGameObj.SetActive(false);
            materialsGridGameObj.SetActive(false);
            useTabButton.image.overrideSprite = buttonOffSprites[1];
            matsTabButton.image.overrideSprite = buttonOffSprites[2];
        } else if (tabNumber == 1)
        {
            useGridGameObj.SetActive(true);
            useTabButton.image.overrideSprite = buttonOnSprites[1];

            equipmentGridGameObj.SetActive(false);
            materialsGridGameObj.SetActive(false);
            equipTabButton.image.overrideSprite = buttonOffSprites[0];
            matsTabButton.image.overrideSprite = buttonOffSprites[2];
        } else if (tabNumber == 2)
        {
            materialsGridGameObj.SetActive(true);
            matsTabButton.image.overrideSprite = buttonOnSprites[2];

            equipmentGridGameObj.SetActive(false);
            useGridGameObj.SetActive(false);
            equipTabButton.image.overrideSprite = buttonOffSprites[0];
            useTabButton.image.overrideSprite = buttonOffSprites[1];
        } else
        {
            Debug.Log("Incorrect tabNumber:" + tabNumber + " was passed into SetTabInFocus in PlayerInventory.");
        }
    }
}
