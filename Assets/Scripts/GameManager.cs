using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Object")]
    public PoolManager pool;
    public PlayerController player;
    public UIManager uIManager;
    public GameObject enemyCleaner;

    [Header("Game Control")]
    public bool isLive = true; //게임 진행여부
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    //플레이어 게임 진행상태
    [Header("Player Info")]
    public int playerId;
    public GameObject[] playerPrefabs;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public List<WeaponManager> weaponManagers = new List<WeaponManager>();  //플레이어 보유 무기
    public int[] nextExp = {3, 5, 10, 100, 150, 210, 280, 360, 450, 600}; //레벨 별 필요 경험치

    void Awake(){
        instance = this;
        uIManager.mainUI.SetActive(true);
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SelectPlayer(int id){
        player = Instantiate(playerPrefabs[id]).GetComponent<PlayerController>();
        playerId = id;
    }
    public void GameStart(){
        //playerId = id;
        health = maxHealth;
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //player = playerPrefabs[playerId].GetComponent<PlayerController>();
        player.gameObject.SetActive(true);
        ResumeGame();
        CreateBaseWeapon(player.baseWeapon);
    }

    //게임오버
    public void GameOver(){
        StartCoroutine(GameOverRoutine());
    }
    private IEnumerator GameOverRoutine(){
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uIManager.endUI.SetActive(true);
        uIManager.Lose();
        PauseGame();
    }

    //게임 클리어
    public void GameVictory(){
        StartCoroutine(GameVictoryRoutine());
    }
    private IEnumerator GameVictoryRoutine(){
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uIManager.endUI.SetActive(true);
        uIManager.Victory();
        PauseGame();
    }

    public void GameRetry(){
        SceneManager.LoadScene(0);
    }

    void Update(){
        if(!isLive){
            return;
        }
        gameTime += Time.deltaTime;
        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
            //제한시간 버티면 승리
            GameVictory();
        }
    }

    public void CreateBaseWeapon(ItemData data){ //기본 or 새 무기 생성
        //GameObject newWeapon = new GameObject();
        //weaponManagers.Add(newWeapon.AddComponent<WeaponManager>());
        //PlayerController 인스펙터에 itemData 매개변수로 전달받아 Init
        //newWeapon.GetComponent<WeaponManager>().Init(data);
        uIManager.rect.GetComponent<LevelUp>().Select(data.ItemId);
    }

    public void GetExp(int expAmount){
        if(!isLive){
            return;
        }
        exp += expAmount;
        if(exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)]){
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
