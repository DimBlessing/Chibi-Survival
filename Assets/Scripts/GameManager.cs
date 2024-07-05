using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Object")]
    public PoolManager pool;
    public PlayerController player;
    public UIManager uIManager;

    [Header("Game Control")]
    public bool isLive = true; //게임 진행여부
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    //플레이어 게임 진행상태
    [Header("Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public List<WeaponManager> weaponManagers = new List<WeaponManager>();  //플레이어 보유 무기
    public int[] nextExp = {3, 5, 10, 100, 150, 210, 280, 360, 450, 600}; //레벨 별 필요 경험치

    void Awake(){
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start(){
        health = maxHealth;
    }

    void Update(){
        if(!isLive){
            return;
        }
        gameTime += Time.deltaTime;
        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
        }
    }

    public void CreateBaseWeapon(ItemData data){ //기본 or 새 무기 생성
        GameObject newWeapon = new GameObject();
        weaponManagers.Add(newWeapon.AddComponent<WeaponManager>());
        //PlayerController 인스펙터에 itemData 매개변수로 전달받아 Init
        newWeapon.GetComponent<WeaponManager>().Init(data);
    }

    public void GetExp(int expAmount){
        exp += expAmount;
        if(exp == nextExp[level]){
            level++;
            exp = 0;
            uIManager.ShowLevelUI();
        }
    }

    public void PauseGame(){
        isLive = false;
        Time.timeScale = 0f;
    }
    public void ResumeGame(){
        isLive = true;
        Time.timeScale = 1f;
    }
}
