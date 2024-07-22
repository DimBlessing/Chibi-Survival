using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public int id;  //아이템 ID
    public int prefabId;    //풀 프리팹 ID
    public int expAmount;
    private Collider2D collider;
    private Rigidbody2D rb;
    private Animator itemAnimator;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        itemAnimator = GetComponent<Animator>();
    }
    void OnDisable(){
        GameManager.instance.GetExp(expAmount);
        if(id == 10){
            //돈 추가
            GameManager.instance.money++;
        }
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
        
        StartCoroutine(AbsorbItem());
    }
    private IEnumerator AbsorbItem()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 itemPos = gameObject.transform.position;
        Vector3 targetDir = (playerPos - itemPos).normalized;
        float speed = 7f;

        Debug.Log("move item");
        while (Vector3.Distance(playerPos, itemPos) > 0.1f)
        {
            playerPos = GameManager.instance.player.transform.position;
            itemPos = Vector3.MoveTowards(itemPos, playerPos, speed * Time.deltaTime);
            rb.MovePosition(itemPos);
            yield return null;
        }

        // 플레이어와 겹쳐지면 이동 정지
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.SetActive(false);
        yield return null;
    }
}
