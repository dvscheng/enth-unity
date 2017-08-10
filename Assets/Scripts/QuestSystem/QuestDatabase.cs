using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : ScriptableObject {

    /* (NPC_ID, Quest). */
	public Dictionary<int, List<Quest>> npcIDToQuests;
    public Dictionary<int, Quest> questDictionary;

    public void OnEnable()
    {
        npcIDToQuests = new Dictionary<int, List<Quest>>();
        questDictionary = new Dictionary<int, Quest>();

        questDictionary.Add( 0,
            /* Example quests. */
            new Quest(
                0,
                (int)Quest.State.unqualified,                                                                                           // The quest's state; refer to Quest.cs for the enum
                new int[] { },                                                                                                          // The quests (their ids) that are required for this quest to start
                new int[] { },                                                                                                          // The quests that have this quest as a requirement.
                (int)NPCDatabase.ID.example,                                                                                            // The NPC that gives the quest
                (int)NPCDatabase.ID.example,                                                                                            // The NPC that completes the quest
                new string[] { "Start dialogue" },                                                                                      // Start dialogue
                new string[] { "End dialogue" },                                                                                        // End dialogue 
                new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),         // (Objective description, item ID, amount to be collected)
                                        new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }));
        questDictionary.Add( 1,
            new Quest(
                1,
                (int)Quest.State.unqualified,
                new int[] { },
                new int[] { },
                (int)NPCDatabase.ID.example,
                (int)NPCDatabase.ID.example,
                new string[] { "Start dialogue" },
                new string[] { "End dialogue" },
                new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                        new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }));

        /* desert_master quests. */
        questDictionary.Add(2,
            new Quest(
                    2,
                    (int)Quest.State.qualified,
                    new int[] { },
                    new int[] { },
                    (int)NPCDatabase.ID.desert_master,
                    (int)NPCDatabase.ID.desert_master,
                    new string[] { "Start dialogue" },
                    new string[] { "End dialogue" },
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                           new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) }));

        CreateIDToQuestDictionary();
    }

    private void CreateIDToQuestDictionary()
    {
        foreach (Quest quest in questDictionary)
        {
            if (npcIDToQuests.ContainsKey(quest.StartNPC))
                npcIDToQuests[quest.StartNPC].Add(quest);                                                           // Adds a quest to the Quest[]
            else
                npcIDToQuests.Add(quest.StartNPC, new List<Quest>( new Quest[] { quest } ));                         // Couples a Quest[] to the NPC ID key 
        }
    }
}
