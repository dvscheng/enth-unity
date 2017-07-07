using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
    From https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects?playlist=17117
     */
public class ItemDatabaseEditor : EditorWindow {

    public ItemDatabaseSO itemDatabase;
    int viewIndex = 1;

    [MenuItem("Window/Inventory Item Editor")]
    static void Init()
    {
        GetWindow(typeof(ItemDatabaseEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            itemDatabase = AssetDatabase.LoadAssetAtPath(objectPath, typeof(ItemDatabaseSO)) as ItemDatabaseSO;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);
        if (itemDatabase != null)
        {
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = itemDatabase;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = itemDatabase;
        }
        GUILayout.EndHorizontal();

        if (itemDatabase == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (itemDatabase != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < itemDatabase.itemList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (itemDatabase.itemList == null)
                Debug.Log("wtf");
            if (itemDatabase.itemList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, itemDatabase.itemList.Count);
                //Mathf.Clamp (viewIndex, 1, itemDatabase.itemList.Count);
                EditorGUILayout.LabelField("of   " + itemDatabase.itemList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                itemDatabase.itemList[viewIndex - 1].id = EditorGUILayout.IntField("ID", itemDatabase.itemList[viewIndex - 1].id);
                itemDatabase.itemList[viewIndex - 1].itemName = EditorGUILayout.TextField("Name", itemDatabase.itemList[viewIndex - 1].itemName as string);
                itemDatabase.itemList[viewIndex - 1].sprite = EditorGUILayout.ObjectField("Sprite", itemDatabase.itemList[viewIndex - 1].sprite, typeof(Sprite), false) as Sprite;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                itemDatabase.itemList[viewIndex - 1].isStackable = (bool)EditorGUILayout.Toggle("Stackable?", itemDatabase.itemList[viewIndex - 1].isStackable, GUILayout.ExpandWidth(false));
                itemDatabase.itemList[viewIndex - 1].isQuestItem = (bool)EditorGUILayout.Toggle("Quest item?", itemDatabase.itemList[viewIndex - 1].isQuestItem, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                itemDatabase.itemList[viewIndex - 1].flavorText = EditorGUILayout.TextField("Flavor text", itemDatabase.itemList[viewIndex - 1].flavorText as string);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

            }
            else
            {
                GUILayout.Label("This Inventory List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(itemDatabase);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        itemDatabase = Create();
        if (itemDatabase)
        {
            itemDatabase.itemList = new List<ItemObject>();
            string relPath = AssetDatabase.GetAssetPath(itemDatabase);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Item Database", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            itemDatabase = AssetDatabase.LoadAssetAtPath(relPath, typeof(ItemDatabaseSO)) as ItemDatabaseSO;
            if (itemDatabase.itemList == null)
                itemDatabase.itemList = new List<ItemObject>();
            if (itemDatabase)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        ItemObject newItem = CreateInstance<ItemObject>();
        newItem.itemName = "New Item";
        newItem.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(newItem, itemDatabase);
        itemDatabase.itemList.Add(newItem);
        viewIndex = itemDatabase.itemList.Count;
    }

    void DeleteItem(int index)
    {
        itemDatabase.itemList.RemoveAt(index);
    }

    public static ItemDatabaseSO Create()
    {
        ItemDatabaseSO asset = CreateInstance<ItemDatabaseSO>();

        AssetDatabase.CreateAsset(asset, "Assets/ItemDatabase.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
