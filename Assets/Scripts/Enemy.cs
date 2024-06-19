using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;  //타겟 플레이어
    private bool isLive = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }
    void OnEnable(){
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        if(!isLive)
            return;
        
        Vector2 dirVec = target.position - rb.position; //타겟 방향
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //목표 거리
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    void LateUpdate(){
        spriteRenderer.flipX = target.position.x < rb.position.x;
    }

    void OnTriggerExit2D(Collider2D collision){
        if(!collision.CompareTag("Area")){
            return;
        }
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 playerDir = GameManager.instance.player.inputVec;
        if(collider.tag == "Enemy"){
            if(collider.enabled){
                transform.Translate(playerDir * 15 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
            }
        }
    }
}
