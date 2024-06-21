using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    ///가장 가까운 적에 대한 원거리 공격 클래스

    public float scanRange; //스캔 범위
    public LayerMask targetLayer;   //적 타겟 레이어
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate(){
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearestTarget();
    }

    Transform GetNearestTarget(){   //가장 가까운 적 인식
        Transform result = null;
        float diff = 100f;

        foreach(RaycastHit2D target in targets){
            Vector3 playerPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(playerPos, targetPos);

            if(curDiff < diff){
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
