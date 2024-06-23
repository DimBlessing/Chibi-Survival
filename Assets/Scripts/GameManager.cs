using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Object")]
    public PoolManager pool;
    public PlayerController player;

    [Header("Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    //플레이어 게임 진행상태
    [Header("Player Info")]
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {3, 5, 10, 100, 150, 210, 280, 360, 450, 600}; //레벨 별 필요 경험치

    void Awake(){
        instance = this;
    }

    void Update(){
        gameTime += Time.deltaTime;
        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
        }
    }

    public void GetExp(int expAmount){
        exp += expAmount;
        if(exp == nextExp[level]){
            level++;
            exp = 0;
        }
    }
}
