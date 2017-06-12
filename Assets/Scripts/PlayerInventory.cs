using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    private static PlayerInventory _instance;   // singleton behaviour
    public static PlayerInventory Instance
    {
        get { return _instance; }
    }


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

    /* Should be referenced in Unity editor. */
    [HideInInspector] public Sprite[] buttonOnSprites;
    [HideInInspector] public Sprite[] buttonOffSprites;
    public Button equipTabButton;
    public Button useTabButton;
    public Button matsTabButton;


    public bool inventoryUIOn;
    public int tabInFocus;  // 0 = equips, 1 = use, 2 = mats


    public void Awake()
    {
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


        inventoryUIOn = true;
        tabInFocus = (int)ItemDatabase.ItemType.mats;
    }

    /* Gets the correct grid depending on the item's type; returns true IFF item is added successfully. */
    public bool AddToInventory(Item item)
    {
        int type = item.Type;
        int itemID = item.ItemID;
        /* Depending on the item's type, update the appropriate grid and add the item to the new slot/increase the count (assumes accessing array in dictionary
         *  is not a reference) . */
        switch (type)
        {
            case (int)ItemDatabase.ItemType.error:
                Debug.Log("Item of type error passed into AddToInventory.");
                break;

            case (int)ItemDatabase.ItemType.equip:
                return AddItemToSpecificGrid(equipmentGridLocation, equipmentGrid, ref equipNextAvailableSlot, ref numEquips, item);

            case (int)ItemDatabase.ItemType.use:
                return AddItemToSpecificGrid(useGridLocation, useGrid, ref useNextAvailableSlot, ref numUse, item);

            case (int)ItemDatabase.ItemType.mats:
                return AddItemToSpecificGrid(materialsGridLocation, materialsGrid, ref matsNextAvailableSlot, ref numMats, item);
        }
        return false;
    }

    //TODO check ref on numOfType in unity editor
    private bool AddItemToSpecificGrid(Dictionary<int, int[]> gridLocation, ItemSlot[,] grid, ref int[] nextAvailableSlot, ref int numOfType, Item item)
    {
        int itemID = item.ItemID;
        if (gridLocation.ContainsKey(itemID))
        {
            int[] itemSlotLocation = gridLocation[itemID];
            grid[itemSlotLocation[0], itemSlotLocation[1]].AddToExistingItem(item.Amount);
            return true;
        }
        else if (FindNextAvailableSlot(grid, ref nextAvailableSlot))
        {
            grid[nextAvailableSlot[1], nextAvailableSlot[0]].SetItemAndSprite(item);
            gridLocation.Add(item.ItemID, new int[2] { nextAvailableSlot[1], nextAvailableSlot[0] });
            numOfType++;
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

    /* Toggles this UI's display on or off in Unity. */
    public void ToggleOnOff()
    {
        inventoryUIOn = !inventoryUIOn;
        gameObject.SetActive(inventoryUIOn);
    }
}
