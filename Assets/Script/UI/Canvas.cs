using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    // Start is called before the first frame update
    public Text lifeText, stoneText, bladeText;

    public void Awake()
    {
        LifeUpdate();
        StoneUpdate();
        BladeUpdate();
        FirstTimePlayState();
    }

    public void LifeUpdate()
    {
        lifeText.text = "X" + PlayerPrefs.GetInt("PlayerLife").ToString();
    }

    public void StoneUpdate()
    {
        stoneText.text = "X" + PlayerPrefs.GetInt("PlayerStone").ToString();
    }
    public void BladeUpdate()
    {
        bladeText.text = "X" + PlayerPrefs.GetInt("PlayerBlade").ToString();
    }

    public void FirstTimePlayState()
    {
        
            PlayerPrefs.SetInt("PlayerBlade", 0);
            PlayerPrefs.SetInt("PlayerLife", 0);
            PlayerPrefs.SetInt("PlayerStone", 0);

        
    }
}
