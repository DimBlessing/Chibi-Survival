using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour   //장비 제어 스크립트
{
    public ItemData.ItemType type;
    public float rate;  //레벨 별 데이터

    public void Init(ItemData data){
        //Basic Setting
        name = "Gear " + data.ItemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //Property Setting
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate){
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear(){
        switch(type){
            case ItemData.ItemType.Glove:   //장갑 -> 공속
                RateUp();
                break;
            case ItemData.ItemType.Shoe:    //신발 -> 이속
                SpeedUp();
                break;
        }
    }

    void RateUp(){  //공격속도
        WeaponManager[] weapons = transform.parent.GetComponentsInChildren<WeaponManager>();
        foreach(WeaponManager weapon in weapons){
            switch(weapon.id){
                case 0: //장검 등 근접무기
                Debug.Log("hear");
                    weapon.rpm = 150 + (150 * rate);
                    break;
                default:    //원거리
                    weapon.rpm = 0f;
                    //weapon.speed = 0.5f * (1f - rate);
                    weapon.attackInterval *= 0.8f;
                    break;
            }
        }
    }

    void SpeedUp(){ //플레이어 이동속도
        float speed = 4f;
        GameManager.instance.player.speed = speed + (speed * rate);
    }
}
