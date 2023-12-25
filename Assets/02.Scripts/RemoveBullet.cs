using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BULLET"))
        {
            // 첫번쨰 충돌한 지점의 정보 추출
            ContactPoint cp = other.GetContact(0);
            
            // 충돌한 총알의 법선 벡터를 쿼터니언으로 변경
            Quaternion rot = Quaternion.LookRotation(-cp.normal);
            
            // 총알 스파크 효과 생성
            GameObject spark = Instantiate(sparkEffect, other.transform.position, rot);
            
            // 일정 시간이 지나면 스파크 삭제
            Destroy(spark, 0.5f);
            
            // 총알 파괴
            Destroy(other.gameObject);
        }
    }
}
