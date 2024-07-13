using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public ItemData baseWeapon;
    public Transform playerRoot;
    public Rigidbody2D rb;
    public Animator playerAnimator;
    //public SpriteRenderer spriteRenderer;
    public EnemyScanner enemyScanner;
    public float speed = 3f;
    public Vector2 inputVec;

    private Vector3 currentRotation;
    void Start()
    {
        playerRoot = gameObject.transform.GetChild(0).GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        enemyScanner = GetComponent<EnemyScanner>();
        //GameManager.instance.CreateBaseWeapon(baseWeapon);
    }

    void OnMove(InputValue value){
        if(!GameManager.instance.isLive){
            return;
        }
        inputVec = value.Get<Vector2>();
    }

    void FixedUpdate(){
        if(!GameManager.instance.isLive){
            return;
        }
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }
    
    void LateUpdate(){
        if(!GameManager.instance.isLive){
            return;
        }
        playerAnimator.SetFloat("Speed", inputVec.magnitude);
        currentRotation = playerRoot.localEulerAngles;
        if(inputVec.x != 0){
            //spriteRenderer.flipX = inputVec.x < 0;
            if(inputVec.x > 0){
                currentRotation.y = 180f;
                playerRoot.localEulerAngles = currentRotation;
            }
            else{
                currentRotation.y = 0f;
                playerRoot.localEulerAngles = currentRotation;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision){
        if(!GameManager.instance.isLive)
            return;
        
        GameManager.instance.health -= Time.deltaTime * 10f;    //적마다 다른 데미지로 향후 수정

        if(GameManager.instance.health <= 0){
            for(int i = 2; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
            playerAnimator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
