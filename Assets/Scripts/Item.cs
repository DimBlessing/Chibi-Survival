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

    public Image icon;
    public TextMeshProUGUI textLevel;

    void Awake(){
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = itemData.itemIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
    }

    void LateUpdate(){
        textLevel.text = "Lv." + (level + 1);
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
                    int nextPenetrate = -1;
                    //count, penetrate 변수 통합 사용

                    nextDamage += itemData.baseDamage * itemData.damages[level];
                    nextCount += itemData.counts[level];
                    nextPenetrate += itemData.penetrates[level];

                    if(itemData.itemType == ItemData.ItemType.Melee){
                        nextPenetrate = -1;
                    }
                    weaponManager.LevelUp(nextDamage, nextCount, nextPenetrate);
                }
                break;
            case ItemData.ItemType.Glove:
                
                break;
            case ItemData.ItemType.Shoe:

                break;
            case ItemData.ItemType.Potion:

                break;
        }
        level++;
        if(level == itemData.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
