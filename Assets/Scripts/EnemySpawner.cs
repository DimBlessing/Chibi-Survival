using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    private float timer;

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.5f){
            Spawn();
            timer = 0f;
        }
    }

    void Spawn(){
        GameObject enemy = GameManager.instance.pool.GetPoolObj(Random.Range(0, 3));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
