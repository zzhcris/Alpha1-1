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
    // Define player attributes
    private float speed = 5.0f;
    private float jumpForce = 5.0f;
    private bool isShielded = false;
    private bool isInvisible = false;
    private float normalDamage = 10f;
    private float damage = 10f;

    public void GainAbility(string ability)
    {
        switch (ability)
        {
            case "SpeedBoost":
                speed *= 1.5f; // Increase speed by 50%
                break;

            case "JumpBoost":
                jumpForce *= 1.5f; // Increase jump force by 50%
                break;

            case "Invisibility":
                isInvisible = true;
                // Implement invisibility effect, e.g., change material transparency
                GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
                break;

            case "Shield":
                isShielded = true;
                // Here, you can add a visual shield or other effects
                break;

            case "DoubleDamage":
                damage = normalDamage * 2; // Double the damage
                break;

            case "Heal":
                // Assuming there's a health attribute
                // health += 10; 
                // Limit the maximum health value, e.g.:
                // health = Mathf.Min(health, maxHealth);
                break;
        }
    }
}


