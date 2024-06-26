using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public Slider expSlider;
    public Slider healthSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI timeText;

    private float curExp = 0;
    private float maxExp = 0;
    private float curHealth = 0;
    private float maxHealth = 0;
    private float remainTime = 0;

    void Awake(){

    }

    void Update()
    {
        //expText.text = "Exp: " + curExp;
    }

    void LateUpdate(){
        curExp = GameManager.instance.exp;
        maxExp = GameManager.instance.nextExp[GameManager.instance.level];
        curHealth = GameManager.instance.health;
        maxHealth = GameManager.instance.maxHealth;
        expSlider.value = curExp / maxExp;
        healthSlider.value = curHealth / maxHealth;
        levelText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
        killText.text = string.Format("{0:F0}", GameManager.instance.kill);
        SetTimer();
    }
    void SetTimer(){
        remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }
}
