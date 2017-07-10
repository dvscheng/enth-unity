using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {
    #region Singleton Behaviour
    private static ItemTooltip _instance;   // singleton behaviour
    public static ItemTooltip Instance
    {
        get { return _instance; }
    }
    #endregion

    public RectTransform rect;
    public GameObject itemName;
    public GameObject sprite;
    public GameObject flavorText;
    public GameObject equipmentStats;

    /* The original size of the tooltip. */
    Vector2 originalSize;
    /* The offset from the middle of the tooltip from the mouse position. */
    Vector3 offset;

    void Awake()
    {
        #region Singleton Behaviour
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        #endregion
        originalSize = rect.sizeDelta;
    }

    /* Set the information to-be displayed on the tooltip, snap the tooltip to position, THEN toggle it on. */
    public void Initialize(ItemObject item)
    {
        itemName.GetComponent<Text>().text = item.itemName;
        sprite.GetComponent<Image>().sprite = item.sprite;
        flavorText.GetComponent<Text>().text = item.flavorText;
        if (item is EquipItems)
        {
            rect.sizeDelta = originalSize + new Vector2(0, equipmentStats.GetComponent<RectTransform>().rect.height + 36);
            equipmentStats.GetComponent<Text>().text =
                  "Attack:          " + (item as EquipItems).attack + "\n"
                + "Defence:         " + (item as EquipItems).defence + "\n"
                + "Hp:              " + (item as EquipItems).hp + "\n"
                + "DefencePen:      " + (item as EquipItems).defencePen + "%" + "\n"
                + "Mastery:         " + (item as EquipItems).mastery + "%" + "\n";
            equipmentStats.SetActive(true);
        }
        else
        {
            rect.sizeDelta = originalSize;
            equipmentStats.GetComponent<Text>().text = "";
            equipmentStats.SetActive(false);
        }
        SnapToPosition();
        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.itemTooltip, true);
    }
    
    /* Trail the mouse. */
    void Update () {
        SnapToPosition();
    }

    /* Snap the tooltip to float near the bottom right of the mouse. */
    void SnapToPosition()
    {
        // Z = 40 is default for UI
        var mousePos = Input.mousePosition;
        mousePos.Set(mousePos.x, mousePos.y, 40);
        Vector3 newPos = mousePos + new Vector3((rect.sizeDelta.x / 2) + 15, (-rect.sizeDelta.y / 2) - 5, 0);
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(newPos);
    }
}
