using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : ScriptableObject {
    public enum NPC_ID
    {
        yellowHood, // indexed starting at 1
        chief
    }

    ItemDatabaseSO itemDatabase;
	public Dictionary<int, Quest> questInfo;

    public void OnEnable()
    {
        itemDatabase = CreateInstance<ItemDatabaseSO>();
        questInfo = new Dictionary<int, Quest>();

        QuestObjective objective1 = new ItemQuestObjective( "Collect mushrooms",
            (int)ItemDatabaseSO.ItemID.mushroom, 3);
        QuestObjective objective2 = new ItemQuestObjective("Collect rocks",
            (int)ItemDatabaseSO.ItemID.rock, 3);
        questInfo[(int)NPC_ID.yellowHood] = new Quest((int)NPC_ID.yellowHood, new QuestObjective[] { objective1, objective2 });

        QuestObjective objective3 = new ItemQuestObjective("Collect mushrooms",
            (int)ItemDatabaseSO.ItemID.mushroom, 2);
        QuestObjective objective4 = new ItemQuestObjective("Collect rocks",
            (int)ItemDatabaseSO.ItemID.rock, 2);
        questInfo[(int)NPC_ID.chief] = new Quest((int)NPC_ID.chief, new QuestObjective[] { objective1, objective2 });
    }
}
