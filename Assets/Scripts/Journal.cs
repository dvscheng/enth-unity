using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour {

    /* Unity editor */
    public GameObject contentPage;

    Dictionary<int, int> enemyKillCount = new Dictionary<int, int>();
    Dictionary<int, Text> idToTextComponent = new Dictionary<int, Text>();

    void Awake()
    {
        Enemy.onDeath += OnEnemyDeath;

        // TODO: read from database/save later      shortcut: multiple line comment = ctrl + k + c
        for (int i = 0; i < Enum.GetNames(typeof(ItemDatabaseSO.MobID)).Length; i++)
        {
            GameObject killCountGO = Instantiate(Resources.Load<GameObject>("Prefabs/Kill Count Object"), Vector2.zero, Quaternion.identity, contentPage.transform);
            killCountGO.GetComponent<Text>().text = Enum.GetName(typeof(ItemDatabaseSO.MobID), i);      // gets the MobID.ToString()

            enemyKillCount.Add(i, 0);       // (mobid, killcount)
            idToTextComponent.Add(i, killCountGO.transform.GetChild(0).gameObject.GetComponent<Text>());        // get the text GO (child 0) then get its text component
        }
    }

    void OnDestroy()
    {
        Enemy.onDeath -= OnEnemyDeath;
    }

    void OnEnemyDeath(int mobID)
    {
        enemyKillCount[mobID] += 1;
        UpdateJournal(mobID);
    }

    void UpdateJournal(int mobID)
    {
        idToTextComponent[mobID].text = "" + enemyKillCount[mobID];
    }
}
