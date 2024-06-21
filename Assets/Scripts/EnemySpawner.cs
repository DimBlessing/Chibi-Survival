using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    private int level;
    private float timer;
    

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
        Debug.Log("level: " + level);
        if(timer > spawnData[level].spawnTime){
            Spawn();
            timer = 0f;
        }
    }

    void Spawn(){
        GameObject enemy = GameManager.instance.pool.GetPoolObj(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
