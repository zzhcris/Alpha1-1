using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool isAlive, isIdle, jumpAttack, isJumpUp, slideAttack, isHurt;
    public int bossLife;
    public GameObject attackCollider;

    GameObject player;
    Animator myAnim;
    Vector3 slideTargetPosition;
    BoxCollider2D myCollider;

    private void Awake()
    {
        player = GameObject.Find("Player");
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();

        isAlive = true;
        isIdle = true;
        jumpAttack = false;
        isJumpUp = true;
        slideAttack = false;
        isHurt = false;

        bossLife = 10;
    }

    private void Update()
    {
        if (isAlive)
        {
            if (isIdle)
            {
                LookAtPlayer();
                if (Vector3.Distance(player.transform.position, transform.position) <= 4.0f)
                {
                    // Boss slide attack
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttack");
                }
                else if (Vector3.Distance(player.transform.position, transform.position) >= 4.0f &&
                    Vector3.Distance(player.transform.position, transform.position) <= 15.0f)
                {
                    // Boss jump attack
                    isIdle = false;
                    StartCoroutine("IdleToJumpAttack");
                }

            }
            else if (jumpAttack)
            {
                LookAtPlayer();
                if (isJumpUp)
                {
                    Vector3 myTarget = new Vector3(player.transform.position.x, 3.5f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, 15.0f * Time.deltaTime);
                    myAnim.SetBool("JumpUp", true);
                }
                else
                {
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", true);

                    Vector3 myTarget = new Vector3(transform.position.x, -2.3f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, 15.0f * Time.deltaTime);
                }

                if (transform.position.y == 3.5f)
                {
                    isJumpUp = false;

                }
                else if (transform.position.y == -2.3f)
                {
                    jumpAttack = false;
                    StartCoroutine("JumpDownToIdle");
                }
            }
            else if (slideAttack)
            {
                myAnim.SetBool("Slide", true);

                transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, 8.0f * Time.deltaTime);

                if (transform.position == slideTargetPosition)
                {
                    myAnim.SetBool("Slide", false);
                    slideAttack = false;
                    isIdle = true;
                }
            }
            else if (isHurt)
            {
                Vector3 myTargetPosition = new Vector3(transform.position.x, -2.3f, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, 10.0f * Time.deltaTime);

            }
        }
    }

    void LookAtPlayer()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator IdleToSlideAttack()
    {
        yield return new WaitForSeconds(1.0f);
        LookAtPlayer();
        slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        slideAttack = true;
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttack = true;
    }

    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
    }

    IEnumerator SetAnimHurtToFalse()
    {
        yield return new WaitForSeconds(0.5f);

        myAnim.SetBool("Hurt", false);
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
        myAnim.SetBool("Slide", false);
        isHurt = false;
        isIdle = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" || collision.tag == "PlayerSkill")
        {
            if(collision.tag == "PlayerAttack")
            {
                bossLife--;
            }
            else
            {
                bossLife = bossLife - 2;
            }

            // if boss is alive
            if(bossLife >= 1)
            {
                isIdle = false;
                jumpAttack = false;
                slideAttack = false;
                isHurt = true;

                StopCoroutine("IdleToSlideAttack");
                StopCoroutine("IdleToJumpAttack");
                StopCoroutine("JumpDownToIdle");
                myAnim.SetBool("Hurt", true);
                StartCoroutine("SetAnimHurtToFalse");
            }
            // if boss is dead
            else
            {
                isAlive = false;
                myCollider.enabled = false;
                StopAllCoroutines();
                myAnim.SetBool("Dead", true);
            }
        }
    }

    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }
}
