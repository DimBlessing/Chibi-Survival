using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] battlePrefabs;   //전투 관련 프리팹(적, 무기)
    public GameObject[] itemPrefabs;    //드랍 아이템 프리팹
    private List<GameObject>[] battlePools;   //각  종류에 따른 풀
    private List<GameObject>[] itemPools;     //아이템 풀

    void Awake(){
        battlePools = new List<GameObject>[battlePrefabs.Length];
        itemPools = new List<GameObject>[itemPrefabs.Length];
        for(int i = 0; i < battlePools.Length; i++){
            battlePools[i] = new List<GameObject>();
        }
        for(int i = 0; i < itemPools.Length; i++){
            itemPools[i] = new List<GameObject>();
        }
    }

    public GameObject GetPoolObj(int index){
        GameObject select = null;

        //선택된 풀의 비활성화 오브젝트 선택
        foreach(GameObject item in battlePools[index]){
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //모두 활성화되어 있으면 새로 생성
        if(!select){
            select = Instantiate(battlePrefabs[index], transform);
            battlePools[index].Add(select);
        }

        return select;
    }
    public GameObject DropItemPool(int index){
        GameObject select = null;
        //선택된 풀의 비활성화 오브젝트 선택
        foreach(GameObject item in itemPools[index]){
            if(!item.activeSelf){
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //모두 활성화되어 있으면 새로 생성
        if(!select){
            select = Instantiate(itemPrefabs[index], transform);
            itemPools[index].Add(select);
        }

        return select;
    }
}
