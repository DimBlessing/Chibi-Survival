using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType{Melee, Range, Glove, Shoe, Potion}

    [Header("# Main Info")]
    public ItemType itemType;
    public int ItemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public int basePenetrate;
    public float[] damages;
    public int[] counts;
    public int[] penetrates;

    [Header("# Weapon Data")]
    public GameObject projectile;   //투사체
    
}
