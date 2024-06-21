using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int id;  //무기 ID
    public int prefabId;    //풀 프리팹 ID
    public float damage;
    public int count;   //무기 배치 갯수
    public float speed; //근접 회전 속도
    public int penetrate;   //관통(-1 이면 무한관통)
    public float attackInterval; //원거리 공격 속도

    private float timer;

    private PlayerController player;

    void Awake(){
        player = GetComponentInParent<PlayerController>();
    }
    void Start(){
        Init();
    }

    public void Init(){
        switch(id){
            case 0: //근접
                speed = 150;
                MeleeBatch();
                break;
            case 1: //원거리
                attackInterval = 0.3f;
                break;
            default:
                
                break;
        }
    }

    public void LevelUp(float damage, int count, int penetrate){
        this.damage = damage;
        this.count += count;

        if(id == 0){
            MeleeBatch();
        }
        else if(id == 1){
            this.penetrate += penetrate;            
        }
    }

    void Update(){
        switch(id){
            case 0: //근접
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1: //원거리
                timer += Time.deltaTime;
                if(timer > attackInterval){
                    timer = 0f;
                    Fire();
                }
                break;
            default:

                break;
        }

        //Test
        if(Input.GetButtonDown("Jump")){
            LevelUp(10, 1, 1);   
        }
    }

    private void MeleeBatch(){
        for(int i = 0; i < count; i++){
            Transform weapon;
            if(i < transform.childCount){   //기존 무기 재활용
                weapon = transform.GetChild(i);
            }
            else{   //부족하면 오브젝트 풀링
                weapon = GameManager.instance.pool.GetPoolObj(prefabId).transform;
                weapon.parent = transform;
            }

            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector3 rotVec = Vector3.forward * 360f * i / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 1.5f, Space.World);
            weapon.GetComponent<Weapon>().Init(id, damage, -1, Vector3.zero);
        }
    }

    private void Fire(){
        if(!player.enemyScanner.nearestTarget){
            return;
        }
        Debug.Log("FIre");
        Vector3 targetPos = player.enemyScanner.nearestTarget.position;
        Vector3 targetDir = targetPos - transform.position;
        targetDir = targetDir.normalized;

        Transform bullet = GameManager.instance.pool.GetPoolObj(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, targetDir);
        bullet.GetComponent<Weapon>().Init(id, damage, penetrate, targetDir);
    }
}
