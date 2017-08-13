using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : ScriptableObject {

    private Dictionary<int, Quest> questDictionary;                                          // (id, Quest) Quest IDs mapped to the respective quest
    public Dictionary<int, Quest> QuestDictionary
    {
        get
        {
            return questDictionary;
        }
    }

    private Dictionary<int, List<Quest>> npcIDToQuests;                                      // (npc_id, List<Quest>) An NPC's ID mapped to a List of all their quests
    public Dictionary<int, List<Quest>> NPCIDToQuests
    {
        get
        {
            return npcIDToQuests;
        }
    }

    public void OnEnable()
    {
        npcIDToQuests = new Dictionary<int, List<Quest>>();
        questDictionary = new Dictionary<int, Quest>
        {
            {
                0,
                /* Example quests. */
                new Quest(
                    0,                                                                                                                      // The quest's id
                    (int)Quest.State.qualified,                                                                                             // The quest's state; refer to Quest.cs for the enum
                    new int[] { },                                                                                                          // The quests (their ids) that are required for this quest to start
                    new int[] { },                                                                                                          // The quests that have this quest as a requirement.
                    (int)NPCDatabase.ID.example,                                                                                            // The NPC that gives the quest
                    (int)NPCDatabase.ID.example,                                                                                            // The NPC that completes the quest
                    "Quest Title",
                    new string[] { "Start dialogue" },                                                                                      // Start dialogue
                    new string[] { "End dialogue" },                                                                                        // End dialogue 
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),         // (Objective description, sprite, item ID, amount to be collected)
                                           new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) })
            },
            {
                1,
                new Quest(
                    1,
                    (int)Quest.State.unqualified,
                    new int[] { },
                    new int[] { },
                    (int)NPCDatabase.ID.example,
                    (int)NPCDatabase.ID.example,
                    "Quest Title",
                    new string[] { "Start dialogue" },
                    new string[] { "End dialogue" },
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                            new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) })
            },

            /* desert_master quests. */
            {
                2,
                new Quest(
                    2,
                    (int)Quest.State.qualified,
                    new int[] { },
                    new int[] { },
                    (int)NPCDatabase.ID.desert_yellowHood,
                    (int)NPCDatabase.ID.desert_yellowHood,
                    "Test Quest",
                    new string[] { "Start dialogue Start dialogue Start dialogue Start dialogue Start dialogue.Start dialogue Start dialogue Start dialogue Start dialogue Start dialogue.", "Hi there Hi there Hi there Hi there." },
                    new string[] { "End dialogue" },
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                           new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) })
            }
        };
        CreateIDToQuestDictionary();
    }

    /* Used once to create an (id, quest) dictionary. */
    private void CreateIDToQuestDictionary()
    {
        int[] keys = new int[questDictionary.Count];
        questDictionary.Keys.CopyTo(keys, 0);
        foreach (int key in keys)
        {
            Quest quest = questDictionary[key];
            if (npcIDToQuests.ContainsKey(quest.StartNPC))
                npcIDToQuests[quest.StartNPC].Add(quest);                                                       // Adds a quest to the existing List<Quest>
            else
                npcIDToQuests.Add(quest.StartNPC, new List<Quest>(new Quest[] { quest }));                      // Creates a new List<Quest>
        }
    }
}
