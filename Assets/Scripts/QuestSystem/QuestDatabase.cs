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
                    "New Beginnings",
                    new string[] { "Good morning, young one. You come at a splendid time. I have something to discuss with you.",
                                    "I'm sure you recall how strongly I've been against you venturing off out of Elios, but it's time I told you why.",
                                    "It's been a while since the last time we've talked about the world outside of Elios, and that's for good reason. The last thing our people need is our curious young ones running to their death.",
                                    "But as you've grown, I see that you've grown mature. More importantly, I see in your eyes the curiosity and adventurousness of your father, and through your actions I see your mother's ambitious and loving heart. I believe you are ready to discover the world your father and mother once faught to defend.",
                                    "As you may already know, our Elios is a safe haven in this mysterious and dangerous world. But you need to know this: Elios is not the only safe place. We are one of few safe havens scattered across this land, meaning there are other villages like ours.",
                                    "Your mother and father befriended all of the villages, and in turn they accumulated a power rivaled by none. But to their demise, your mother ... expand on story.",
                                    "Their goal, and now your goal, is it gain the favor of all the villages of this land. With the power of all the villages, you will face the great evil that has flooded our world.",
                                    "But enough intensity for now. You must take small steps. The first part of your journey will to gain the favor and power of our beloved village, Elios. Talk to everyone in the village, and show them this. (You gain an odd looking artifact)",
                                    "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?"},
                    new string[] { "You've returned. Splendid! How did it go?" },
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
