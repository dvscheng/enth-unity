using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject UIObj;
    GameObject player;
    Spawner spawner;
    Rigidbody2D rb;
    EnemyHealthBar healthBar;
    public SpriteRenderer spriteRenderer; // UNITY
    public BoxCollider2D boxCollider; // UNITY
    public BoxCollider2D boxTrigger; // UNITY
    public CircleCollider2D sightRange; // UNITY
    [HideInInspector]
    public BoxCollider2D playerCollider;
    public GameObject healthBarObj;

    const int STATE_IDLE = 0;
    const int STATE_WANDERING = 1;
    const int STATE_CHASING = 2;
    int currentState = 0;
    bool stateReady = true;
    bool readyToAttack = true;
    bool wandering = false;
    const float KNOCKBACK_STUN_TIME = 0.8f;
    bool knockbacked = false;
    Vector2 movement = Vector2.zero;
    [Range(25f, 100f)]
    public float moveSpeed = 40f;
    public int playerOnTriggerEnterCount = 0; // UNITY

    public int mobID; // UNITY

    public delegate void EnemyDelegate(int mobID);
    public static event EnemyDelegate onDeath; 

    /* Game stats. */
    public int Level { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int BaseAtt { get; set; }
    public int AdditionalAtt { get; set; }
    public int Defence { get; set; }
    public int DefencePen { get; set; }
    public float Mastery { get; set; }

    public void SetSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }

    void Start()
    {
        // TODO regions
        Level = 1;
        Hp = 10;
        MaxHp = 10;
        BaseAtt = 5;
        AdditionalAtt = 0;
        Defence = 0;
        DefencePen = 0;
        Mastery = 0.90f;

        UIObj = GameObject.Find("UI");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        healthBar = healthBarObj.GetComponent<EnemyHealthBar>();
        playerCollider = player.GetComponent<BoxCollider2D>();
    }

    /* Chases the player when inside the sight range, attacks player when inside attack range. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerOnTriggerEnterCount++;

            /* If the Player enters once, meaning it has entered the inner, body-hit collider. */
            if (playerOnTriggerEnterCount == 1)
            {
                stateReady = true;
                currentState = STATE_CHASING;
            }
            else if (playerOnTriggerEnterCount == 2 && readyToAttack)
            {
                Damage(collision);

                movement = Vector2.zero;
                stateReady = false;
                StartCoroutine(Wait(3));
                currentState = STATE_IDLE;
            }
        }
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            currentState = STATE_CHASING;
            if (playerOnTriggerEnterCount == 2 && readyToAttack)
            {
                Damage(collision);

                movement = Vector2.zero;
                stateReady = false;
                StartCoroutine(Wait(3));
                currentState = STATE_IDLE;
            }
        }
    }

    /* Enemy stops chasing the player when exiting the sight range. */
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerOnTriggerEnterCount--;
            if (playerOnTriggerEnterCount < 0)
            {
                Debug.Log("onTriggerEnterCount is < 0; it should never be < 0.");
            } else if (playerOnTriggerEnterCount == 0)
            {
                movement = Vector2.zero;
                currentState = STATE_IDLE;
                stateReady = false;
                StartCoroutine(Wait(2));
            }
        }
    }

    private void Damage(Collider2D collision)
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
        // add defence too
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        int playerDefence = playerController.Defence + playerController.BonusDef;
        int damageDealt = (BaseAtt * BaseAtt) / (BaseAtt + (playerDefence - (int)(playerDefence * DefencePen)));
        playerController.TakeDamage(damageDealt);

        readyToAttack = false;
    }


    /* FSM. */
    void Update() {
        switch (currentState)
        {
            case (STATE_IDLE):
                if (stateReady)
                {
                    int roll = Random.Range(0, 100);
                    if (roll > 50)
                    {
                        currentState = STATE_WANDERING;
                        return;
                    }
                    stateReady = false;
                    StartCoroutine(Wait(2));
                }
                break;

            case STATE_WANDERING:
                if (stateReady)
                {
                    if (wandering)
                    {
                        movement = Vector2.zero;
                        wandering = false;

                        int roll = Random.Range(0, 100);
                        if (roll > 50)
                        {
                            currentState = STATE_IDLE;
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
                
            case STATE_CHASING:
                // chase
                if (stateReady)
                {
                    if (knockbacked)
                    {
                        movement = Vector2.zero;
                    }
                    else
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
                }
                break;
        }

        if (Hp <= 0)
        {
            OnDeath();
        }
    }

    /* Move the enemy. */
    void FixedUpdate()
    {
        //rb.MovePosition((Vector2)transform.position + movement * Time.deltaTime);
        rb.velocity = movement * moveSpeed * Time.deltaTime;

        /* If going left, flip x, otherwise don't. Sprite defult looks right. */
        spriteRenderer.flipX = (movement.x < 0) ? true : false;
    }

    public void TakeDamage(int damage)
    {
        /* Update the health and healthBar. */
        Hp -= damage;
        if (Hp < 0)
            Hp = 0;

        healthBar.UpdateBar();

        /* Show the damage text. */
        GameObject damageText = Instantiate(Resources.Load<GameObject>("Prefabs/DamageText"), UIObj.transform);
        damageText.GetComponent<DamageText>().Initialize(gameObject, damage);
        // consider keeping a list of dmg texts and shove some out if it gets too big

        knockbacked = true;
        StartCoroutine(KnockBackDelay(KNOCKBACK_STUN_TIME));
    }

    /* Roll for drops, then destroy the Game Object. */
    void OnDeath()
    {
        if (!RollForDrops(ItemDatabaseSO.LEGENDARY_DROPRATE))
        {
            if (!RollForDrops(ItemDatabaseSO.RARE_DROPRATE))
            {
                if (!RollForDrops(ItemDatabaseSO.UNCOMMON_DROPRATE))
                {
                    RollForDrops(ItemDatabaseSO.COMMON_DROPRATE);
                }
            }
        }
        PlayerController.Instance.GainEXP(30);

        spawner.OnEnemyDeath();

        if (onDeath != null)
            onDeath(mobID);     // delegate callback

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
            Vector2 enemyPos = gameObject.transform.position;
            Vector2 dropPos = new Vector2(enemyPos.x, enemyPos.y);

            int dropRate;
            if (chances == ItemDatabaseSO.LEGENDARY_DROPRATE)
            {
                dropRate = (int)ItemDatabaseSO.DropRateType.legendary;
            } else if (chances == ItemDatabaseSO.RARE_DROPRATE)
            {
                dropRate = (int)ItemDatabaseSO.DropRateType.rare;
            } else if (chances == ItemDatabaseSO.UNCOMMON_DROPRATE)
            {
                dropRate = (int)ItemDatabaseSO.DropRateType.uncommon;
            } else // common
            {
                dropRate = (int)ItemDatabaseSO.DropRateType.common;
            }

            int itemID = ItemDatabaseSO.dropTable[mobID, dropRate];
            if (ItemDatabaseSO.itemList.ContainsKey(itemID))
            {
                GameObject itemOnGround = Instantiate(Resources.Load<GameObject>("Prefabs/ItemOnGround"), dropPos, Quaternion.identity);
                itemOnGround.GetComponent<ItemOnGround>().Initialize(itemID, 5);
            }
            else
            {
                return false;
            }


            //print(chances + "% roll success; Item: " + ((ItemDatabase.ItemID)itemID).ToString() + " of ID: " + itemID);

            return true;
        }

        return false;
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stateReady = true;
        readyToAttack = true;
    }

    private IEnumerator KnockBackDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        knockbacked = false;
    }
}
