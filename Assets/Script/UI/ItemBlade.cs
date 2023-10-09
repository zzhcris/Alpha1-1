using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlade : MonoBehaviour
{
    Player myPlayer;
    Canvas myCanvas;

    private void Awake()
    {
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
           
            int Blade = PlayerPrefs.GetInt("PlayerBlade") + 1;
            PlayerPrefs.SetInt("PlayerBlade", Blade);
            
            myPlayer.playerBlade = Blade;
            myCanvas.BladeUpdate();

            
            Destroy(this.gameObject);
        }
    }
}
