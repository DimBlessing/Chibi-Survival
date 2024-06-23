using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int id;  //아이템 ID
    public int prefabId;    //풀 프리팹 ID
    public int expAmount;
    private Collider2D collider;
    private Animator itemAnimator;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        itemAnimator = GetComponent<Animator>();
    }

    public void Init(int id){
        this.id = id;
        switch(this.id){
            case 0: //동
                expAmount = 1;
                itemAnimator.SetTrigger("Bronze");
                break;
            case 1: //은
                expAmount = 2;
                itemAnimator.SetTrigger("Silver");
                break;
            case 2: //금
                expAmount = 3;
                itemAnimator.SetTrigger("Gold");
                break;
            //////////////////
            default:
                expAmount = 0;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(!collision.CompareTag("Player"))
            return;
        
        GameManager.instance.GetExp(expAmount);
        gameObject.SetActive(false);
    }
}
