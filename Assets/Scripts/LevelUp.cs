using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    Item[] items;
    void Start()
    {
        items = GetComponentsInChildren<Item>(true);
    }

    public void Select(int index){
        items[index].OnClick();
    }

    public void NextLevel(){   //레벨업
        //모든 아이템UI 비활성화
        foreach(Item item in items){
            item.gameObject.SetActive(false);
        }
        //랜덤 3개 아이템 활성화
        int[] rand = new int[3];
        while(true){
            rand[0] = Random.Range(0, items.Length);
            rand[1] = Random.Range(0, items.Length);
            rand[2] = Random.Range(0, items.Length);
            if(rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2]){
                break;
            }
        }

        for(int i = 0; i < rand.Length; i++){
            Item randomItem = items[rand[i]];
            randomItem.gameObject.SetActive(true);

            if(randomItem.level ==  randomItem.itemData.damages.Length){    //만렙인 경우
                randomItem.gameObject.SetActive(false);
                items[items.Length - 1].gameObject.SetActive(true); //임시로 포션 활성화
            }
        }

        //만렙 무기,방어구인 경우... 일단 제외
    }
}
