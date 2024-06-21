using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    }

    void OnMove(InputValue value){
        inputVec = value.Get<Vector2>();
    }

    void FixedUpdate(){
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }
    
    void LateUpdate(){
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
}
