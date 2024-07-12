using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public ItemData itemData;
    public int level;
    public WeaponManager weaponManager;
    public Gear gear;

    public Image icon;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;

    void Awake(){
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = itemData.itemIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = itemData.itemName;
        textDesc.text = itemData.itemDesc;
    }

    void OnEnable(){
        textLevel.text = "Lv." + (level + 1);
    }

    void Init(){
        //플레이어 기본무기 정보 로드
        weaponManager = GameManager.instance.weaponManagers[0];

    }

    public void OnClick(){
        switch(itemData.itemType){
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0){
                    GameObject newWeapon = new GameObject();
                    weaponManager = newWeapon.AddComponent<WeaponManager>();
                    weaponManager.Init(itemData);
                }
                else{
                    float nextDamage = itemData.baseDamage;
                    int nextCount = 0;
                    int nextPenetrate = 0;
                    float nextAttackInterval = 0f;

                    nextDamage += itemData.baseDamage * itemData.damages[level];
                    nextCount += itemData.counts[level];
                    nextPenetrate += itemData.basePenetrate + itemData.penetrates[level];
                    //nextAttackInterval -= itemData.baseAttackInterval - itemData.attackIntervals[level];

                    if(itemData.itemType == ItemData.ItemType.Melee){
                        nextPenetrate = -1;
                        nextAttackInterval = 0;
                    }
                    weaponManager.LevelUp(nextDamage, nextCount, nextPenetrate, nextAttackInterval);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if(level == 0){
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else{
                    float nextRate = itemData.damages[level];
                    Debug.Log("rate: " + nextRate);
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Potion:
                GameManager.instance.health = GameManager.instance.maxHealth;   //힐량 조정 필요
                break;
        }
        if(level == itemData.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
