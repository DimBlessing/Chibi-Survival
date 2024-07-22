using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponId;
    public float damage;    //공격력
    public float speed;     //탄속
    public int penetrate;   //관통(-1이면 근접무기) 가능 횟수
    private Rigidbody2D rb;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(int weaponId, float damage, float speed, int penetrate, Vector3 dir){
        this.weaponId = weaponId;
        this.damage = damage;
        this.speed = speed;
        this.penetrate = penetrate;
        if(penetrate > -1){
            Debug.Log("move bullet");
            rb.velocity = dir * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){ //관통
        if(!collider.CompareTag("Enemy") ||  penetrate == -1)
            return;
        

        Debug.Log("enemy hitted");
        penetrate--;
        if(weaponId != 0 && penetrate == -1){
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void ReturnPool(){
        gameObject.SetActive(false);
    }
}
