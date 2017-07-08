using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour {

    public Text slimeKC;
    public Text blueSlimeKC;

    Dictionary<int, int> enemyKillCount = new Dictionary<int, int>();
    Dictionary<int, Text> idToGo = new Dictionary<int, Text>();

    void Awake()
    {
        Enemy.onDeath += OnEnemyDeath;

        //read from database/save later
        enemyKillCount.Add((int)ItemDatabaseSO.MobID.Slime, 0);
        enemyKillCount.Add((int)ItemDatabaseSO.MobID.BlueSlime, 0);

        idToGo.Add((int)ItemDatabaseSO.MobID.Slime, slimeKC);
        idToGo.Add((int)ItemDatabaseSO.MobID.BlueSlime, blueSlimeKC);
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
        idToGo[mobID].text = "" + enemyKillCount[mobID];
    }
}
