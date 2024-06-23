using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;   //적 프리팹
    public GameObject[] itemPrefabs;    //드랍 아이템 프리팹
    private List<GameObject>[] pools;   //각 프리팹 종류에 따른 풀

    void Awake(){
        pools = new List<GameObject>[enemyPrefabs.Length + itemPrefabs.Length];
        for(int i = 0; i < pools.Length; i++){
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetPoolObj(int index){
        GameObject select = null;

        //선택된 풀의 비활성화 오브젝트 선택
        foreach(GameObject item in pools[index]){
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //모두 활성화되어 있으면 새로 생성
        if(!select){
            select = Instantiate(enemyPrefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
