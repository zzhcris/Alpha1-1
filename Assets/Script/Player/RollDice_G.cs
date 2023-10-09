using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice_G : MonoBehaviour
{
    private Player_G player;
    private void Start()
    {
        player = GetComponent<Player_G>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RollDice();
        }
    }

    void RollDice()
    {
        int diceResult = Random.Range(1, 7);
        Debug.Log("Dice Result: " + diceResult);

        switch (diceResult)
        {
            case 1:
                player.GainAbility("SpeedBoost");
                break;
            case 2:
                player.GainAbility("JumpBoost");
                break;
            case 3:
                player.GainAbility("Invisibility");
                break;
            case 4:
                player.GainAbility("Shield");
                break;
            case 5:
                player.GainAbility("DoubleDamage");
                break;
            case 6:
                player.GainAbility("Heal");
                break;
            default:
                Debug.LogError("Invalid dice result!");
                break;
        }
    }

}

public class Player_G : MonoBehaviour
{
    private float speed = 5.0f;
    private float jumpForce = 5.0f;
    private bool isInvisible = false;
    private bool isShielded = false;
    private float normalDamage = 10f;
    private float damage = 10f;
    private float health = 100f;
    private Renderer rend;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 9.8f;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Basic movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ) * speed;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            moveDirection.y = jumpForce;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

        // Attack logic
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
            {
                var enemy = hit.collider.GetComponent<Enemy_G>();
                if (enemy)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    public void GainAbility(string ability)
    {
        StopAllCoroutines(); // Stop ongoing effects
        switch (ability)
        {
            case "SpeedBoost":
                speed *= 1.5f;
                StartCoroutine(ResetAbilityAfterSeconds("Speed", 10f)); // Ability lasts for 10 seconds
                break;
            case "JumpBoost":
                jumpForce *= 1.5f;
                StartCoroutine(ResetAbilityAfterSeconds("Jump", 10f));
                break;
            case "Invisibility":
                rend.material.color = new Color(1, 1, 1, 0.5f); // 50% transparent
                break;
            case "Shield":
                isShielded = true;
                break;
            case "DoubleDamage":
                damage *= 2;
                StartCoroutine(ResetAbilityAfterSeconds("Damage", 10f));
                break;
            case "Heal":
                health = Mathf.Min(health + 20, 100); // Assuming max health is 100
                break;
        }
    }

    private System.Collections.IEnumerator ResetAbilityAfterSeconds(string ability, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        switch (ability)
        {
            case "Speed":
                speed /= 1.5f;
                break;
            case "Jump":
                jumpForce /= 1.5f;
                break;
            case "Damage":
                damage = normalDamage;
                break;
        }
    }
}

public class Enemy_G : MonoBehaviour
{
    public float health = 50;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // Perhaps play a death animation or spawn a pickup item
    }

    private void Update()
    {
        // Simple AI: Follow the player
        var player = FindObjectOfType<Player>();
        if (player)
        {
            var direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * 3f * Time.deltaTime; // Assuming enemy speed is 3
        }
    }
}


