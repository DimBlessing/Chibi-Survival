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
    public float rpm; //근접 회전 속도
    public float speed; //원거리 탄속
    public int penetrate;   //관통(-1 이면 무한관통)
    public float attackInterval; //원거리 공격 속도

    private float timer;

    private PlayerController player;

    void Awake(){
        player = GameManager.instance.player;
    }
    
    public void Init(ItemData data){
        //Basic Setting
        name = "Weapon" + data.ItemId;
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.6f, 0);
        //Property Setting
        id = data.ItemId;
        damage = data.baseDamage;
        count = data.baseCount;
        for(int i = 0; i < GameManager.instance.pool.battlePrefabs.Length; i++){
            if(data.projectile == GameManager.instance.pool.battlePrefabs[i]){
                prefabId = i;
                break;
            }
        }
        switch(id){
            case 0: //근접
                rpm = 150;
                speed = 0;
                MeleeBatch();
                break;
            case 1: //원거리 마법구
                attackInterval = 0.3f;
                speed = 10f;
                break;
            case 2: //화살
                attackInterval = 0.2f;
                speed = 5f;
                break;
            default:
                
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count, int penetrate, float attackInterval){
        this.damage = damage;
        this.count += count;

        if(id == 0 && penetrate == -1){
            MeleeBatch();
        }
        else if(id == 1){
            this.penetrate += penetrate;            
            //동시 발사 갯수 증가
            //this.attackInterval += attackInterval;
        }
        else if(id == 2){
            this.penetrate += penetrate;
            //동시 발사 갯수 증가
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Update(){
        if(!GameManager.instance.isLive){
            return;
        }
        switch(id){
            case 0: //근접
                transform.Rotate(Vector3.back * rpm * Time.deltaTime);
                break;
            case 1: //원거리 마법구
                timer += Time.deltaTime;
                if(timer > attackInterval){
                    timer = 0f;
                    // /Fire();
                    StartCoroutine(CoFire(count));
                }
                break;
            case 2: //화살
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
            //LevelUp(10, 1, 1);   
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
            weapon.GetComponent<Weapon>().Init(id, damage, speed, -1, Vector3.zero);
        }
    }

    private IEnumerator CoFire(int count){
        while(count > 0){
            Fire();
            yield return new WaitForSeconds(0.05f);
            count--;
        }
    }
    private void Fire(){
        Vector3 targetPos = new Vector3(0,0,0);
        if(!player.enemyScanner.nearestTarget){
            //return;
            //근처 적 없을 땐 무작위 방향으로 발사하도록 추가
            targetPos = player.GetComponentInChildren<EnemySpawner>().spawnPoint[UnityEngine.Random.Range(0, 18)].position;           
        }
        else{
            targetPos = player.enemyScanner.nearestTarget.position;
        }

        Vector3 targetDir = (targetPos - transform.position).normalized;
        //targetDir = targetDir.normalized;
        Debug.Log("Fire");
    
        Transform bullet = GameManager.instance.pool.GetPoolObj(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, targetDir);
        bullet.GetComponent<Weapon>().Init(id, damage, speed, penetrate, targetDir);
    }
}
