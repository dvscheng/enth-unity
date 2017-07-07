using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    #region Singleton Behaviour
    private static SpawnManager _instance;   // singleton behaviour
    public static SpawnManager Instance
    {
        get { return _instance; }
    }
    #endregion

    Dictionary<string, Spawner[]> sceneToSpawners = new Dictionary<string, Spawner[]>();
    Dictionary<string, bool> sceneHasLoaded = new Dictionary<string, bool>();

    Spawner[] spawnersInFocus;

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
        DontDestroyOnLoad(gameObject);

        /* Load all the scene names coupled with false. */
        sceneToSpawners.Add("Desert Town", new Spawner[0]);
        sceneToSpawners.Add("Player House", new Spawner[0]);
        sceneHasLoaded.Add("Desert Town", false);
        sceneHasLoaded.Add("Player House", false);
    }

    void Update()
    {
        
    }

    /* When enabled, subscribe to SceneManager's sceneLoaded. */
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    /* When disabled, unsubscribe to SceneManager's sceneLoaded. */
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    /* When the scene changes, loads the list of spawners of the given scene if it hasn't
     * been loaded before, and spawns the enemies when possible. */
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        /* Load spawn locations. */
        if (!sceneHasLoaded[scene.name])
        {
            FindAndSaveSpawners(scene.name);
        }

        /* Used in Update() to countdown. */
        spawnersInFocus = sceneToSpawners[scene.name];

        /* Check if the monster needs to respawn. */
        Spawner[] spawners = sceneToSpawners[scene.name];
        foreach (Spawner s in spawners)
            s.SpawnEnemy();     // returns true/false for debugging
    }

    /* Finds and adds all the spawners to the sceneToSpawner dictionary for later reference. */
    void FindAndSaveSpawners(string sceneName)
    {
        GameObject spawners = GameObject.Find("Mobs");
        if (spawners != null)
        {
            Transform spawnersTrans = spawners.transform.GetChild(0);

            int numSpawners = spawnersTrans.childCount;
            Spawner[] listOfSpawners = new Spawner[numSpawners];
            for (int i = 0; i < numSpawners; i++)
            {
                listOfSpawners[i] = spawnersTrans.GetChild(i).GetComponent<Spawner>();
            }
            sceneToSpawners[sceneName] = listOfSpawners;
        }
    }
}
