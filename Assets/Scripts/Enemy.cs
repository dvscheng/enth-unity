using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    GameObject go;
    GameObject player;
    Rigidbody2D rb;
    public BoxCollider2D boxCollider; // UNITY
    public BoxCollider2D boxTrigger; // UNITY
    public CircleCollider2D sightRange; // UNITY
    [HideInInspector] public BoxCollider2D playerCollider;

    ItemDatabase itemDB;

    public int playerOnTriggerEnterCount = 0; // UNITY

    public int mobID; // UNITY

    private int hp = 15;
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    int baseDamage = 5;

    Vector2 movement = Vector2.zero;
    [Range(25f, 100f)] public float moveSpeed = 40f;

    int currentState = (int)State.STATE_IDLE;
    bool stateReady = true;
    bool wandering = false;

    private enum State : int
    {
        STATE_IDLE,
        STATE_WANDERING,
        STATE_CHASING
    }

    private void Start()
    {
        go = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        itemDB = ScriptableObject.CreateInstance<ItemDatabase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerOnTriggerEnterCount++;

            /* If the Player enters once, meaning it has entered the inner, body-hit collider. */
            if (playerOnTriggerEnterCount == 1)
            {
                stateReady = true;
                currentState = (int)State.STATE_CHASING;
            }
            else if (playerOnTriggerEnterCount == 2)
            {
                /* If the Player enters twice, meaning it has entered the inner, body-hit collider. */
                GameObject playerObj = collision.gameObject;

                /* Add force */
                float playerX = playerObj.transform.position.x;
                float playerY = playerObj.transform.position.y;
                float enemyX = gameObject.transform.position.x;
                float enemyY = gameObject.transform.position.y;

                float forceX = (enemyX - playerX > 0) ? -100 : 100;
                float forceY = (enemyY - playerY > 0) ? -100 : 100;
                Vector2 force = new Vector2(forceX, forceY);

                /* Apply damage. */
                // playerObj.GetComponent<Rigidbody2D>().AddForce(force);
                playerObj.GetComponent<PlayerController>().TakeDamage(baseDamage);

                movement = Vector2.zero;
                stateReady = false;
                StartCoroutine(Wait(1));

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerOnTriggerEnterCount--;
            if (playerOnTriggerEnterCount < 0)
            {
                Debug.Log("onTriggerEnterCount is < 0");
            } else if (playerOnTriggerEnterCount == 0)
            {
                movement = Vector2.zero;
                currentState = (int)State.STATE_IDLE;
                stateReady = false;
                StartCoroutine(Wait(2));
            }
        }
    }

    private void Update() {
        switch (currentState)
        {
            case (int)State.STATE_IDLE:
                if (stateReady)
                {
                    int roll = Random.Range(0, 100);
                    if (roll > 50)
                    {
                        currentState = (int)State.STATE_WANDERING;
                        return;
                    }
                    stateReady = false;
                    StartCoroutine(Wait(2));
                }
                break;

            case (int)State.STATE_WANDERING:
                if (stateReady)
                {
                    if (wandering)
                    {
                        movement = Vector2.zero;
                        wandering = false;

                        int roll = Random.Range(0, 100);
                        if (roll > 50)
                        {
                            currentState = (int)State.STATE_IDLE;
                            return;
                        }
                    }
                    else
                    {
                        // go to random pos
                        float dirX = Random.Range(-1f, 1f);
                        float dirY = Random.Range(-1f, 1f);
                        movement = new Vector2(dirX, dirY);

                        stateReady = false;
                        wandering = true;   
                        StartCoroutine(Wait(2));
                    }
                }
                break;
                
            case (int)State.STATE_CHASING:
                // chase
                if (stateReady)
                {
                    float playerX = player.transform.position.x;
                    float playerY = player.transform.position.y;
                    float enemyX = gameObject.transform.position.x;
                    float enemyY = gameObject.transform.position.y;

                    float forceX = (enemyX - playerX > 0) ? -1 : 1;
                    float forceY = (enemyY - playerY > 0) ? -1 : 1;
                    /* Removes jittering. */
                    if (System.Math.Abs(playerX - enemyX) < .01f)
                        forceX = 0;
                    if (System.Math.Abs(playerY - enemyY) < .01f)
                        forceY = 0;

                    movement = new Vector2(forceX, forceY);
                }
                break;
        }


        if (hp <= 0)
        {
            OnDeath();
        }
    }

    private void FixedUpdate()
    {
        //rb.MovePosition((Vector2)transform.position + movement * Time.deltaTime);
        rb.velocity = movement * moveSpeed * Time.deltaTime;
    }

    /* Roll for drops, then destroy the Game Object. */
    private void OnDeath()
    {
        if (!RollForDrops(itemDB.LEGENDARY))
        {
            if (!RollForDrops(itemDB.RARE))
            {
                if (!RollForDrops(itemDB.UNCOMMON))
                {
                    RollForDrops(itemDB.COMMON);
                }
            }
        }

        Destroy(gameObject);
    }

    /* Returns true IFF the roll succeeds. (Change later for diff tier drops) */
    private bool RollForDrops(int chances)
    {
        // instantiate an ItemOnGround prefab.
        // give the prefab an Item type depending on the monster's ID;

        int diceRoll = Random.Range(0, 100);
        if (diceRoll < chances)
        {
            // drop an item
            Vector2 dropPos = new Vector2(gameObject.transform.position.x + Random.Range(-1, 1), gameObject.transform.position.y + Random.Range(-1, 1));
            GameObject itemOnGround = (GameObject)Instantiate(Resources.Load("Prefabs/ItemOnGround"), dropPos, Quaternion.identity) as GameObject;

            int dropRate;
            if (chances == itemDB.LEGENDARY)
            {
                dropRate = (int)ItemDatabase.ItemDropRate.legendary;
            } else if (chances == itemDB.RARE)
            {
                dropRate = (int)ItemDatabase.ItemDropRate.rare;
            } else if (chances == itemDB.UNCOMMON)
            {
                dropRate = (int)ItemDatabase.ItemDropRate.uncommon;
            } else // common
            {
                dropRate = (int)ItemDatabase.ItemDropRate.common;
            }

            int itemID = itemDB.dropTable[mobID, dropRate];
            int itemType = itemDB.itemIDToType[itemID];
            switch (itemType)
            {
                case ((int)ItemDatabase.ItemType.equip):
                    itemOnGround.GetComponent<ItemOnGround>().SetItem(new EquipItems(itemID, 1));
                    break;

                case ((int)ItemDatabase.ItemType.use):
                    itemOnGround.GetComponent<ItemOnGround>().SetItem(new UseItems(itemID, 1));
                    break;

                case ((int)ItemDatabase.ItemType.mats):
                    itemOnGround.GetComponent<ItemOnGround>().SetItem(new MatItems(itemID, 1));
                    break;

                case ((int)ItemDatabase.ItemType.error):
                    Debug.Log("Item created in enemy RollForDrops has type error.");
                    break;
            }
            print(chances + "% roll success; dropped itemID: " + itemID);

            return true;
        }

        return false;
    }

    private bool PlayerWithinRange()
    {
        // generate direction;
        return false;
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stateReady = true;
    }
}
