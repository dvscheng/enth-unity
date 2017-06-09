using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Inputs inputs;
    Rigidbody2D rb;
    Animator anim;
    public GameObject inventoryGameObj; // UNITY
    [HideInInspector] public PlayerInventory inventory;

    /* Movement params. */
    float moveDistance = 3.5f;
    public float horizontalAxis; // UNITY
    public float verticalAxis; // UNITY
    int upKey;
    int downKey;
    int leftKey;
    int rightKey;

    /* Animator params. */
    int animationState;

    /* Game stats. */
    private int hp = 100;
    public int Hp
    {
        get { return hp; }
    }
    public void takeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }
    }

    private int baseDamage = 10;
    public int BaseDamage
    {
        get { return baseDamage; }
    }

    public bool attacking; // UNITY

    Vector2 movement = Vector2.zero;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        inputs = gameObject.AddComponent<Inputs>();
        anim = GetComponent<Animator>();
        animationState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        inventory = inventoryGameObj.GetComponent<PlayerInventory>();

        /* Initialize movement parameters. */
        upKey = 0;
        downKey = 0;
        leftKey = 0;
        rightKey = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            /* Mouse clicks. */
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // check if on ui somehow

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

                /*
                print("PlayerPos.x:" + playerPos.x + ", PlayerLeft:" + playerLeft);
                print("MousePos.x: " + mousePos.x);
                print(below);
                */

                Vector3 hitBoxPos;
                float rotation;
                string triggerName;

                /* For animation control. */
                //AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

                if (above)
                {
                    hitBoxPos = new Vector2(playerPos.x, playerPos.y + playerColliderSize.y);
                    rotation = 90;
                    triggerName = "playerAttackUp";
                }
                else if (below)
                {
                    hitBoxPos = new Vector2(playerPos.x, playerPos.y - playerColliderSize.y);
                    rotation = -90;
                    triggerName = "playerAttackDown";
                }
                else if (left)
                {
                    hitBoxPos = new Vector2(playerPos.x - playerColliderSize.x, playerPos.y);
                    rotation = 180;
                    triggerName = "playerAttackLeft";
                }
                else // right
                {
                    hitBoxPos = new Vector2(playerPos.x + playerColliderSize.x, playerPos.y);
                    rotation = 0;
                    triggerName = "playerAttackRight";
                }

                GameObject hitBox = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerStrikeBox"), hitBoxPos, Quaternion.identity);
                hitBox.transform.Rotate(new Vector3(0, 0, rotation));
                anim.enabled = true;
                anim.SetTrigger(triggerName);

                attacking = true;

                movement = Vector2.zero;

                StartCoroutine(HitBoxTimer(0.1f, hitBox));
                StartCoroutine(AttackAnimationDone(0.5f));
            }
        }
        if (!attacking)
        {
            /* Update movement parameters. */
            upKey = (inputs.up_key) ? 1 : 0;
            downKey = (inputs.down_key) ? -1 : 0;
            leftKey = (inputs.left_key) ? -1 : 0;
            rightKey = (inputs.right_key) ? 1 : 0;
            horizontalAxis = leftKey + rightKey;
            verticalAxis = upKey + downKey;

            
            if (verticalAxis > 0)
            {
                anim.SetTrigger("playerMoveUp");
            }
            else if (verticalAxis < 0)
            {
                anim.SetTrigger("playerMoveDown");
            }
            else if (horizontalAxis > 0)
            {
                anim.SetTrigger("playerMoveRight");
            }
            else if (horizontalAxis < 0)
            {
                anim.SetTrigger("playerMoveLeft");
            }

            /* Diagonal movement. */
            if (horizontalAxis != 0 && verticalAxis != 0)
            {
                horizontalAxis = horizontalAxis / 1.5f;
                verticalAxis = verticalAxis / 1.5f;
            }
            if (horizontalAxis == 0 && verticalAxis == 0)
            {
                animationState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
                anim.Play(animationState, 0, 0.0f);
                anim.enabled = false;
            }
            else
            {
                anim.enabled = true;
            }

            movement = new Vector2(horizontalAxis * moveDistance, verticalAxis * moveDistance);

            if (hp <= 0)
            {
                //game over
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private IEnumerator HitBoxTimer(float waitTime, GameObject hitBox)
    {
        yield return new WaitForSeconds(waitTime);
        Object.Destroy(hitBox);
    }

    /* Called by the animator to signal an end to animation. */
    public IEnumerator AttackAnimationDone(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attacking = false;
        anim.SetTrigger("doneAttacking");
        // change to moving?
    }
}
