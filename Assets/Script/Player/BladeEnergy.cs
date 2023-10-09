using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeEnergy : MonoBehaviour
{
    GameObject player;
    Rigidbody2D myRigi;

    public float energySpeed;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        myRigi = GetComponent<Rigidbody2D>();

        if(player.transform.localScale.x == 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myRigi.AddForce(Vector2.right * energySpeed, ForceMode2D.Impulse);
        }
        else if(player.transform.localScale.x == -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            myRigi.AddForce(Vector2.left * energySpeed, ForceMode2D.Impulse);
        }

        // Blade Engergy disappear after 0.5 seconds
        Destroy(this.gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        */

        if(collision.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
