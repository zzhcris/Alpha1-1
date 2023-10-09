using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZombie : MonoBehaviour
{
    public Vector3 targetPosition;
    public float mySpeed;
    public GameObject attackCollider;
    public int enemyLife;

    Animator myAnim;
    Vector3 originPosition;
    Vector3 turnPoint;
    bool isFirstTimeIdle;
    bool isDead;
    private bool isTransformed = false;
    //private float nextTransformTime = 0f;
    //public float transformCooldown = 5f;

    GameObject myPlayer;

    private void Awake()
    {
        myPlayer = GameObject.Find("Player");
        myAnim = GetComponent<Animator>();
        originPosition = new Vector3(transform.position.x, transform.position.y,
            transform.position.z);

        isFirstTimeIdle = true;
        isDead = false;
    }


    void Update()
    {
        if (isTransformed)
        {
            transform.position = myPlayer.transform.position - new Vector3(1.5f, 0, 0);
            return; // Return to prevent other update actions
        }

        if (Vector3.Distance(transform.position, myPlayer.transform.position) <= 1.5f && !isDead)
        {
            if (myPlayer.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackWait"))
            {
                return;
            }
            myAnim.SetTrigger("Attack");
            return;
        }
        else
        {
            if (turnPoint == targetPosition)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else if (turnPoint == originPosition)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }

        if (transform.position.x == targetPosition.x)
        {
            myAnim.SetTrigger("Idle");
            turnPoint = originPosition;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            isFirstTimeIdle = false;
        }
        else if (transform.position.x == originPosition.x)
        {
            if (!isFirstTimeIdle)
            {
                myAnim.SetTrigger("Idle");
            }

            turnPoint = targetPosition;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                turnPoint, mySpeed * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        HandleCollisionWhenTransformed(collision);
        if (collision.tag == "PlayerAttack" || collision.tag == "PlayerSkill")
        {
            if(collision.tag == "PlayerAttack")
            {
                enemyLife--;
            }
            else
            {
                enemyLife = enemyLife - 2; ;
            }
            
            if(enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
            }
            else
            {
                myAnim.SetTrigger("Dead");
                isDead = true;
                StartCoroutine("AfterDie");
            }
        }
    }

    public void TransformToAlien()
    {
        if (!isTransformed)
        {
            // Change the enemy's position behind the player
            transform.position = myPlayer.transform.position - new Vector3(1.5f, 0, 0); // Adjust the value as needed

            // Refill the enemy's life
            enemyLife = 2;

            // Set the flag
            isTransformed = true;

        }
    }


    void HandleCollisionWhenTransformed(Collider2D collision)
    {
        if (isTransformed && (collision.tag == "Enemy" || collision.tag == "Zombie"))
        {
            enemyLife--;

            if (enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
            }
            else
            {
                myAnim.SetTrigger("Dead");
                isTransformed = false; // Reset the transformation
                StartCoroutine("AfterDie");
            }
        }
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(3.0f);
        
        Destroy(this.gameObject);
        
    }
}
