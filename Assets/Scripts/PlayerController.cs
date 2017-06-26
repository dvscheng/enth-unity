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
    public GameObject UIManagerObj;
    UIManager UIMan;
    public GameObject inventoryGameObj; // UNITY
    [HideInInspector] public PlayerInventory inventory; // could use singleton, but playercontroller is there on start and there will always be just one so it's fine
    public GameObject inputManagerObj;
    [HideInInspector] public Inputs inputs;

    [Header("Movement and Animation")]
    /* Movement and animtor params. */
    public float moveDistance = 175f;
    public float inputX; // UNITY
    public float inputY; // UNITY
    public bool isMoving; // UNITY
    public Vector2 lastFacing; // UNITY
    Vector2 movement = Vector2.zero;
    int animationState;

    public bool attacking = false; // UNITY
    public bool attackAnimDone = true;
    public bool attackDelayDone = true;
    public bool attackGraceDone = true;
    public int attacksInputted = 0;
    public int attacksDone = 0;
    public const int MAX_ATTACKS = 3;
    public IEnumerator gracePeriod;
    public IEnumerator attackAnim;

    public bool dashed = false;

    public int currentState;
    public const int STATE_IDLE = 0;
    public const int STATE_MOVING = 1;
    public const int STATE_ATTACKING = 2;
    public const int STATE_DASHING = 3;

    /* Game stats. */
    private int hp = 100;
    public int Hp
    {
        get { return hp; }
    }
    private int maxHp = 100;
    public int MaxHp {
        get
        {
            return maxHp;
        }
    }
    private int baseDamage = 10;
    public int BaseDamage
    {
        get { return baseDamage; }
    }
    private float mastery = .50f;
    public float Mastery
    {
        get { return mastery; }
    }


    /* Applies the damage and resets to 0 if hp goes below 0. */

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
        UIMan = UIManagerObj.GetComponent<UIManager>();
        inputs = inputManagerObj.GetComponent<Inputs>();
        animationState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        inventory = inventoryGameObj.GetComponent<PlayerInventory>();

        /* Initialize movement parameters. */
        currentState = STATE_IDLE;
        inputX = 0;
        inputY = 0;
        isMoving = false;
        lastFacing = Vector2.zero;
    }

    void OldControls()
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

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            default:
            case (STATE_IDLE):
                if (Inputs.Instance.dash_key_down)
                {
                    currentState = STATE_DASHING;
                    break;
                }
                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !EventSystem.current.IsPointerOverGameObject() && attackDelayDone)
                {
                    currentState = STATE_ATTACKING;
                    attacksInputted = 1;
                    break;
                }
                inputX = Input.GetAxisRaw("HorizontalAD");
                inputY = Input.GetAxisRaw("VerticalWS");
                if (inputX != 0 || inputY != 0)
                {
                    currentState = STATE_MOVING;
                    break;
                }

                /* Idle behaviour. */
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
                movement = Vector2.zero;

                break;

            case (STATE_MOVING):
                if (Inputs.Instance.dash_key_down)
                {
                    currentState = STATE_DASHING;
                    break;
                }
                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !EventSystem.current.IsPointerOverGameObject() && attackDelayDone)
                {
                    currentState = STATE_ATTACKING;
                    attacksInputted = 1;
                    break;
                }
                inputX = Input.GetAxisRaw("HorizontalAD");
                inputY = Input.GetAxisRaw("VerticalWS");
                if (inputX == 0 && inputY == 0)
                {
                    currentState = STATE_IDLE;
                    break;
                }

                /* Update movement parameters. */
                isMoving = true;
                lastFacing = new Vector2(inputX, inputY);
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

                break;

            case (STATE_ATTACKING):
                if ((!attackGraceDone && attackAnimDone) && (attacksInputted < MAX_ATTACKS)
                        && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !EventSystem.current.IsPointerOverGameObject())
                {
                    /* If the grace period between combo hits is NOT over AND the attack animation is over. */
                    attacksInputted++;

                    /* Stop the coroutine (set bool to false for safety), then restart the coroutine. */
                    StopCoroutine(gracePeriod);
                    attackGraceDone = false;
                    gracePeriod = AttackGraceTimer(0.7f);
                    StartCoroutine(gracePeriod);

                    /*
                    StopCoroutine(attackAnim);
                    attackAnimDone = false;
                    attackAnim = AttackAnimationDone(0.4f);
                    StartCoroutine(attackAnim);
                    */
                }
                if (attackAnimDone && attackGraceDone && attacksDone == attacksInputted)
                {
                    /* Change the state to idle and trigger the appropriate animation change. */
                    anim.SetTrigger("doneAttacking");
                    currentState = STATE_IDLE;

                    /* Set the timer for the delay between attacks so you can't attack endlessly. */
                    attackDelayDone = false;
                    StartCoroutine(AttackDelayTimer(0.5f));

                    /* Reset the attacks done and inputted variables. */
                    attacksDone = 0;
                    attacksInputted = 0;

                    break;
                }
                else if (attackAnimDone && attacksDone != attacksInputted)
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

                    /* Depending on the direction of the mouse click, create a hitbox at the appropriate position,
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
                    movement = Vector2.zero;

                    anim.SetTrigger(triggerName);
                    anim.SetFloat("LastFacingX", lastFacing.x);
                    anim.SetFloat("LastFacingY", lastFacing.y);

                    /* Start the coroutines for the hitbox spawntime and animation time. */
                    StartCoroutine(HitBoxTimer(0.1f, hitBox));

                    attackAnimDone = false;
                    attackAnim = AttackAnimationDone(0.4f);
                    StartCoroutine(attackAnim);
                    attackGraceDone = false;
                    gracePeriod = AttackGraceTimer(0.7f);
                    StartCoroutine(gracePeriod);

                    attacksDone++;

                    /* Play the appropriate sound. */
                    int sound = Random.Range(1, 3);
                    AudioManager.Instance.Play("Swing " + sound);
                }
                break;

            case (STATE_DASHING):
                // if () check when dashing is done, return to idle state;
                if (dashed)
                {
                    currentState = STATE_IDLE;
                    dashed = false;
                    break;
                }
                if (!dashed)
                {
                    inputX = Input.GetAxisRaw("HorizontalAD");
                    inputY = Input.GetAxisRaw("VerticalWS");
                    Vector2 force;
                    if (inputX == 0 && inputY == 0)
                        force = lastFacing;
                    else
                        force = new Vector2(inputX, inputY);
                    rb.AddForce(force * 1000f);
                    dashed = true;
                }
                break;
        }
    }

    /* Used to apply movement to the character. */
    void FixedUpdate()
    {
        rb.velocity = movement * Time.deltaTime;
    }

    /**/
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
            hp = 0;

        /* Notify the health bar of an update. */
        UIMan.healthBar.UpdateHealth();

        // Die()
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
        attackAnimDone = true;
        // change to moving?
    }

    /* Called to signal an end to the grace period between combo attacks. */
    public IEnumerator AttackGraceTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attackGraceDone = true;
    }

    /* Called to signal an end of the delay between combos. */
    public IEnumerator AttackDelayTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attackDelayDone = true;
    }
}
