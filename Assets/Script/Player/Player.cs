using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float mySpeed;
    public float jumpForce;
    public float dodgeForce;
    public GameObject attackCollider;
    public GameObject BladeEnergyPrefab;
    public int playerLife;
    //private float nextTransformTime = 0f; // Added for cooldown.
    //public float transformCooldown = 5f;  // Time in seconds after which another enemy can be transformed.

    Rigidbody2D myRigi;
    float energyDistance;
    
    SpriteRenderer mySr;

    [HideInInspector]
    public Animator myAnim;

    [HideInInspector]
    public bool isJumpPressed;

    [HideInInspector]
    public bool isDodgePressed;

    [HideInInspector]
    public bool canJump;

    bool isHurt;
    bool canBeHurt;
    bool isDead;
    bool canDodge;
    
    [HideInInspector] public int playerBlade;
    Canvas myCanvas;
    public void Awake()
    {
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySr = GetComponent<SpriteRenderer>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        isJumpPressed = false;
        isDodgePressed = false;
        canJump = true;
        isHurt = false;
        canBeHurt = true;
        isDead = false;
        canDodge = true;
        playerLife = PlayerPrefs.GetInt("PlayerLife"); 
        playerBlade = PlayerPrefs.GetInt("PlayerBlade");
        
    }

    public void Update()
    {
        // Jump operation
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            isJumpPressed = true;
            canJump = false;
        }

        // Dodge operation
        if(Input.GetKeyDown(KeyCode.L))
        {
            isDodgePressed = true;
        }

        // Attack Operation
        if (Input.GetKeyDown(KeyCode.J))
        {
            myAnim.SetTrigger("Attack");
        }

        // Skill Operation
        if (Input.GetKeyDown(KeyCode.K))
        {
            myAnim.SetTrigger("Skill");
            playerBlade--;
            PlayerPrefs.SetInt("PlayerBlade", playerBlade);
            myCanvas.BladeUpdate(); 
        }

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    EnemyMaleZombie[] allEnemies = FindObjectsOfType<EnemyMaleZombie>();
        //    EnemyMaleZombie closestEnemy = null;
        //    float closestDistance = float.MaxValue;

        //    foreach (EnemyMaleZombie enemy in allEnemies)
        //    {
        //        float distanceToPlayer = Vector3.Distance(transform.position, enemy.transform.position);
        //        if (distanceToPlayer < closestDistance)
        //        {
        //            closestDistance = distanceToPlayer;
        //            closestEnemy = enemy;
        //        }
        //    }

        //    if (closestEnemy != null)
        //    {
        //        closestEnemy.TransformToAlien();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnemyMaleZombie[] allEnemies = FindObjectsOfType<EnemyMaleZombie>();
            EnemyMaleZombie closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (EnemyMaleZombie enemy in allEnemies)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                closestEnemy.TransformToAlien();
            }
        }



    }

    // process Rigidbody
    private void FixedUpdate()
    {
        float a = Input.GetAxisRaw("Horizontal");

        if (isDead || isHurt)
        {
            a = 0;
        }

        // make player change directions
        if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (a < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // change player to Run animation 
        myAnim.SetFloat("Run", Mathf.Abs(a));


        // control jumping
        if (isJumpPressed)
        {
            myRigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false; // avoid player keeping jumping
            myAnim.SetBool("Jump", true);
        }

        // control dodging
        if (isDodgePressed && canDodge)
        {
            if(transform.localScale.x == 1.0f)
            {
                myRigi.AddForce(Vector2.right * dodgeForce, ForceMode2D.Impulse);
                
            }
            else if(transform.localScale.x == -1.0f)
            {
                myRigi.AddForce(Vector2.left * dodgeForce, ForceMode2D.Impulse);
            }

            isDodgePressed = false;
            myAnim.SetTrigger("Dodge");
            
        }


        // control moving
        if (!isHurt)
        {
            myRigi.velocity = new Vector2(a * mySpeed, myRigi.velocity.y);
        }
        
    
    }

    // Control Hurt Animation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttack" && !isHurt && canBeHurt) 
        {
            // decrease life points
            playerLife--;
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            myCanvas.LifeUpdate();
            // when player is alive
            if (playerLife >= 1) {
                isHurt = true;
                canBeHurt = false;
                // when player is hurt, it becomes transparent 
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f);
                // when player is hurt, it can not jump 
                canJump = false;
                myAnim.SetBool("Hurt", true);

                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 5.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 5.0f);
                }


                StartCoroutine("SetIsHurtFalse");
            }
            // when player is dead
            else
            {
                isHurt = true;
                isDead = true;
                canJump = false;
                myRigi.velocity = new Vector2(0f, 0f);
                myAnim.SetBool("Dead", true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttack" && !isHurt && canBeHurt)
        {
            // decrease life points
            playerLife--;
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            myCanvas.LifeUpdate();

            // when player is alive
            if (playerLife >= 1)
            {
                isHurt = true;
                canBeHurt = false;
                // when player is hurt, it becomes transparent 
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f);
                // when player is hurt, it can not jump 
                canJump = false;
                myAnim.SetBool("Hurt", true);

                if (transform.localScale.x == 1.0f)
                {
                    myRigi.velocity = new Vector2(-2.5f, 5.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 5.0f);
                }


                StartCoroutine("SetIsHurtFalse");
            }
            // when player is dead
            else
            {
                myAnim.SetBool("Dead", true);
                isHurt = true;
            }
        }
    }

    IEnumerator SetIsHurtFalse()
    {
        // Hurt animation lasts 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        // change to Idle Animation
        myAnim.SetBool("Hurt", false);
        isHurt = false;

        // give player 0.5 seconds to avoid when attacked
        yield return new WaitForSeconds(0.5f);
        canBeHurt = true;
        // change to original transparency
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 1.0f);
    }

    //IEnumerator Set

    public void ForIsHurtSetting()
    {
        myAnim.ResetTrigger("Attack");
        myAnim.ResetTrigger("Skill");
        attackCollider.SetActive(false);
    }

    // call the function in the last frame of attack animation
    // avoid attacking repeatly
    public void ResetAttack()
    {
        myAnim.ResetTrigger("Attack");

    }
    public void ResetSkill()
    {
        myAnim.ResetTrigger("Skill");
    }
 
    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }

    // generate skill(blade energy)
    public void EnergyInstantiate()
    {
        if (transform.localScale.x == 1.0f)
        {
            energyDistance = 1.0f;
        }
        else if (transform.localScale.x == -1.0f)
        {
            energyDistance = -1.0f;
        }
        Vector3 position = new Vector3(transform.position.x + energyDistance, transform.position.y, transform.position.z);
        Instantiate(BladeEnergyPrefab, position, Quaternion.identity);
    }
}
