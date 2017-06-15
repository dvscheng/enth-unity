using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Singleton Behaviour
    private static PlayerController _instance;   // singleton behaviour
    public static PlayerController Instance
    {
        get { return _instance; }
    }
    #endregion

    Rigidbody2D rb;
    Animator anim;
    public GameObject inventoryGameObj; // UNITY
    [HideInInspector] public PlayerInventory inventory; // could use singleton, but playercontroller is there on start and there will always be just one so it's fine

    /* Movement params. */
    float moveDistance = 3.5f;
    public float inputX; // UNITY
    public float inputY; // UNITY
    public bool isMoving; // UNITY
    public Vector2 lastFacing; // UNITY

    /* Animator params. */
    int animationState;

    /* Game stats. */
    private int hp = 100;
    public int Hp
    {
        get { return hp; }
    }


    private int baseDamage = 10;
    public int BaseDamage
    {
        get { return baseDamage; }
    }

    public bool attacking; // UNITY

    Vector2 movement = Vector2.zero;

    /* Applies the damage and resets to 0 if hp goes below 0. */
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
            hp = 0;
    }

    // Use this for initialization
    void Awake() {
        #region Singleton Behaviour
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animationState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        inventory = inventoryGameObj.GetComponent<PlayerInventory>();

        /* Initialize movement parameters. */
        inputX = 0;
        inputY = 0;
        isMoving = false;
        lastFacing = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            /* Attacking */
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                /* Gets the mouse's world position, not screen position. */
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                /*        |  above  |
                 *        |         |
                 *        |---------|
                 * left   | Player  |   right
                 *        |---------|
                 *        |         |
                 *        |  below  |
                 **/
                BoxCollider2D playerCollider = gameObject.GetComponent<BoxCollider2D>();
                Vector2 playerPos = gameObject.transform.position;
                Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;
                float playerRight = playerPos.x + playerColliderSize.x / 2;
                float playerLeft = playerPos.x - playerColliderSize.x / 2;

                bool above = ((mousePos.y > playerPos.y) && ((playerLeft < mousePos.x) && (mousePos.x < playerRight))) ? true : false;
                bool below = ((mousePos.y < playerPos.y) && ((playerLeft < mousePos.x) && (mousePos.x < playerRight))) ? true : false;
                bool left = (mousePos.x < playerLeft) ? true : false;
                bool right = (playerRight < mousePos.x) ? true : false;

                /* Depending on the direction of the mouse click, creates a hitbox at the appropriate position,
                 * and setup the animation triggers and idle position for after the animation ends. */
                Vector3 hitBoxPos;
                float rotation;
                string triggerName;
                if (above)
                {
                    hitBoxPos = new Vector2(playerPos.x, playerPos.y + playerColliderSize.y);
                    rotation = 90;
                    triggerName = "playerAttackUp";
                    lastFacing = new Vector2(0, 1);
                }
                else if (below)
                {
                    hitBoxPos = new Vector2(playerPos.x, playerPos.y - playerColliderSize.y);
                    rotation = -90;
                    triggerName = "playerAttackDown";
                    lastFacing = new Vector2(0, -1);

                }
                else if (left)
                {
                    hitBoxPos = new Vector2(playerPos.x - playerColliderSize.x, playerPos.y);
                    rotation = 180;
                    triggerName = "playerAttackLeft";
                    lastFacing = new Vector2(-1, 0);

                }
                else // right
                {
                    hitBoxPos = new Vector2(playerPos.x + playerColliderSize.x, playerPos.y);
                    rotation = 0;
                    triggerName = "playerAttackRight";
                    lastFacing = new Vector2(1, 0);

                }
                GameObject hitBox = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerStrikeBox"), hitBoxPos, Quaternion.identity);
                hitBox.transform.Rotate(new Vector3(0, 0, rotation));
                attacking = true;
                movement = Vector2.zero;

                anim.SetTrigger(triggerName);
                anim.SetFloat("LastFacingX", lastFacing.x);
                anim.SetFloat("LastFacingY", lastFacing.y);

                /* Start the coroutines for the hitbox spawntime and animation time. */
                StartCoroutine(HitBoxTimer(0.1f, hitBox));
                StartCoroutine(AttackAnimationDone(0.5f));

                AudioManager.Instance.Play("Swing 1");
            }
        }
        if (!attacking)
        {
            /* Update movement parameters. */
            inputX = Input.GetAxisRaw("HorizontalAD");
            inputY = Input.GetAxisRaw("VerticalWS");
            if (inputX != 0 || inputY != 0)
            {
                lastFacing = new Vector2(inputX, inputY);
                isMoving = true;
            }
            else
                isMoving = false;
            

            anim.SetFloat("Horizontal", inputX);
            anim.SetFloat("Vertical", inputY);
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("LastFacingX", lastFacing.x);
            anim.SetFloat("LastFacingY", lastFacing.y);

            /* Diagonal speed for rigidbody calcs. */
            if (inputX != 0 && inputY != 0)
            {
                inputX = inputX / 1.5f;
                inputY = inputY / 1.5f;
            }
            movement = new Vector2(inputX * moveDistance, inputY * moveDistance);



            /* Checks when the player dies. */
            if (hp <= 0)
            {
                //game over
            }
        }
    }

    /* Used to apply movement to the character. */
    void FixedUpdate()
    {
        rb.velocity = movement;
    }
    
    /* 
     * After the given wait time, destroys the strike's hitbox.
     * */
    private IEnumerator HitBoxTimer(float waitTime, GameObject hitBox)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(hitBox);
    }

    /* Called to signal an end to the attack animation. */
    public IEnumerator AttackAnimationDone(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attacking = false;
        anim.SetTrigger("doneAttacking");
        // change to moving?
    }
}
