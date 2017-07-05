using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int enemyID;     // change in inspector to change the mob to be spawned
    public float timer;      // time til respawn, in seconds
    public float respawnTime = 60f;

    Vector3 location;
    GameObject enemy;
    bool enemyDead;
    bool readyToSpawn;
    bool startCounting;

	/* Spawn the enemy on start. */
	void Start () {
        timer = respawnTime;
        location = gameObject.transform.position;
        enemyDead = true;
        readyToSpawn = true;

        SpawnEnemy();
    }
	
	/* Updates the timer. */
	void Update () {
        if (startCounting)
        {
            if (timer >= 0)
            {
                timer -= 1 * Time.deltaTime;
            }
            else
            {
                timer = respawnTime;
                startCounting = false;
                readyToSpawn = true;
            }
        }
	}

    /* Returns true if spawning the enemy was successful. */
    public bool SpawnEnemy()
    {
        if (readyToSpawn && enemyDead && enemy == null)
        {
            string name = ((ItemDatabase.MobID)enemyID).ToString();
            enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Mobs/" + name),
                                    location, Quaternion.identity, gameObject.transform.parent.parent);     //parent.parent is the Mobs GameObject
            enemy.GetComponent<Enemy>().SetSpawner(this);
            enemyDead = false;
            readyToSpawn = false;

            return true;
        }
        return false;
    }

    /* Notifies the Spawner that its enemy has died, starting the countdown in Update(). */
    public void OnEnemyDeath()
    {
        enemy = null;
        enemyDead = true;
        startCounting = true;
    }
}
