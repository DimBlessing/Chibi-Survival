using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("# HUD UI")]
    public GameObject hudUI;
    public Slider expSlider;
    public Slider healthSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeScaleText;
    public Button timeScaleUp;

    private float curExp = 0;
    private float maxExp = 0;
    private float curHealth = 0;
    private float maxHealth = 0;
    private float remainTime = 0;

    [Header("# LevelUp UI")]
    public RectTransform rect;

    [Header("# Main UI")]
    public GameObject mainUI;
    public GameObject selectUI;

    [Header("# End UI")]
    public GameObject endUI;
    public GameObject[] endTitles;

    void Awake(){
        hudUI.SetActive(false);
        mainUI.SetActive(true);
        selectUI.SetActive(false);
        endUI.SetActive(false);
    }

    void Update()
    {
        //expText.text = "Exp: " + curExp;
    }

    void LateUpdate(){
        curExp = GameManager.instance.exp;
        maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
        curHealth = GameManager.instance.health;
        maxHealth = GameManager.instance.maxHealth;
        expSlider.value = curExp / maxExp;
        healthSlider.value = curHealth / maxHealth;
        levelText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
        killText.text = string.Format("{0:F0}", GameManager.instance.kill);
        timeScaleText.text = string.Format("{0}x", Time.timeScale);
        SetTimer();
    }
    void SetTimer(){
        remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

    public void OnClickTimeSpeed(){
        if(Time.timeScale == 0f)
            return;

        float currentScale = Time.timeScale;
        if(currentScale == 1f){
            Time.timeScale = 1.5f;
        }
        else if(currentScale == 1.5f){
            Time.timeScale = 2f;
        }
        else if(currentScale == 2f){
            Time.timeScale = 1f;
        }

        GameManager.instance.currenTimeScale = Time.timeScale;
    }

    //레벨업 UI
    public void ShowLevelUI(){
        rect.GetComponent<LevelUp>().NextLevel();
        rect.localScale = Vector3.one;
        GameManager.instance.PauseGame();
    }
    public void HideLevelUI(){
        rect.localScale = Vector3.zero;
        GameManager.instance.ResumeGame();
    }

    //결과 UI
    public void Lose(){
        endTitles[0].SetActive(true);
    }
    public void Victory(){
        endTitles[1].SetActive(true);
    }
}
