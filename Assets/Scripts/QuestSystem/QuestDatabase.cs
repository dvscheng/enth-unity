using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : ScriptableObject {

    /* (NPC_ID, Quest). */
	public Dictionary<int, List<Quest>> npcIDToQuests;

    public void OnEnable()
    {
        npcIDToQuests = new Dictionary<int, List<Quest>>();

        npcIDToQuests[(int)NPCDatabase.ID.example] = new List<Quest>(new Quest[]
            {
            new Quest(
                0,
                new int[] { },                                                                                                          // The quests (their ids) that are required for this quest to start
                (int)NPCDatabase.ID.example,                                                                                            // The NPC that gives the quest
                (int)NPCDatabase.ID.example,                                                                                            // The NPC that completes the quest
                new string[] { "Start dialogue" },                                                                                      // Start dialogue
                new string[] { "End dialogue" },                                                                                        // End dialogue 
                new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),         // (Objective description, item ID, amount to be collected)
                                       new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }
                ),
            new Quest(
                1,
                new int[] { },
                (int)NPCDatabase.ID.example,
                (int)NPCDatabase.ID.example,
                new string[] { "Start dialogue" },
                new string[] { "End dialogue" },
                new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                       new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }
                ),
            }
        );

        npcIDToQuests[(int)NPCDatabase.ID.desert_master] = new List<Quest>(new Quest[]
            {
            new Quest(
                2,
                new int[] { },
                (int)NPCDatabase.ID.desert_master,
                (int)NPCDatabase.ID.desert_master,
                new string[] { "Start dialogue" },
                new string[] { "End dialogue" },
                new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                       new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }
                ),
            }
        );
    }
}
