using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public float health;
    public float maxHealth;
    public float speed;
    public int itemId;  //ItemManager의 id로 전달
    
    private bool isLive;

    public Rigidbody2D target;  //타겟 플레이어
    private Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public Collider2D collider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }
    void OnEnable(){
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isLive = true;
        collider.enabled = true;
        rb.simulated = true;
        spriteRenderer.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;
    }
    public void Init(SpawnData data){
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        itemId = data.expId;
    }

    void FixedUpdate(){
        if(!GameManager.instance.isLive){
            return;
        }
        if(!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        
        Vector2 dirVec = target.position - rb.position; //타겟 방향
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //목표 거리
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    void LateUpdate(){
        if(!GameManager.instance.isLive){
            return;
        }
        spriteRenderer.flipX = target.position.x < rb.position.x;
    }

    void OnTriggerExit2D(Collider2D collision){
        if(!collision.CompareTag("Area") || !isLive){
            return;
        }
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 playerDir = GameManager.instance.player.inputVec;
        if(collider.tag == "Enemy"){
            if(collider.enabled){
                transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(!collider.CompareTag("Weapon") || !isLive)
            return;
        
        health -= collider.GetComponent<Weapon>().damage;
        StartCoroutine(KnockBack());
        if(health > 0){
            animator.SetTrigger("Hit");
        }
        else{
            isLive = false;
            this.collider.enabled = false;
            rb.simulated = false;
            spriteRenderer.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            //GameManager.instance.GetExp();
        }
    }
    private void DropItem(int itemId){
        GameObject item = GameManager.instance.pool.DropItemPool(0);  //경험치 아이템 생성
        item.transform.position = gameObject.transform.position;    //enemy 사망 지점으로 위치 조정
        item.GetComponent<DropItemManager>().Init(itemId);


        //특수 아이템 랜덤 생성 기능 추가
        //일정 확률로 힐링포션, 자석, 스킬부스트 아이템 드랍
        
        //돈
        int moneyRand = Random.Range(0, 2);
        if(moneyRand == 0){
            GameObject money = GameManager.instance.pool.DropItemPool(1);
            money.transform.position = gameObject.transform.position;
            money.GetComponent<DropItemManager>().Init(10);
        }
    }
    void OnCollisionEnter2D(Collision2D collision){ //0802 시작
        if(collision.gameObject.tag != "Obstacle")
            return;

    }


    private IEnumerator KnockBack(){
        yield return new WaitForFixedUpdate();
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rb.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
    }

    private void Dead(){
        gameObject.SetActive(false);
        DropItem(itemId);
    }
}
