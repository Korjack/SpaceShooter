using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelCtrl : MonoBehaviour
{
    // 폭발 효과
    public GameObject expEffect;
    
    // 무작위 텍스쳐
    public Texture[] textures;
    
    //폭발 반경
    public float radius = 10.0f;
    
    // 반경내로 검출되는 Collider 값을 미리 선언
    // 메모리 Garbage를 발생을 줄이기 위함
    // 미리 최대 갯수를 알 수 있다면 이렇게 하는 것이 좋음
    private Collider[] colls = new Collider[10];

    private Transform tr;
    private Rigidbody rb;

    // 총알 맞은 횟수 기록
    private int hitCount = 0;

    private new MeshRenderer _renderer;

    private void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        _renderer = GetComponentInChildren<MeshRenderer>();

        // 난수 발생
        int idx = Random.Range(0, textures.Length);
        
        // 텍스쳐 지정
        _renderer.material.mainTexture = textures[idx];
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BULLET"))
        {
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        // 폭발 오프젝트 생성
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        
        // 폭발 오브젝트 삭제
        Destroy(exp, 0.5f);

        // // 무게를 가볍게 변경
        // rb.mass = 1.0f;
        //
        // // 위쪽 방향으로 힘 추가
        // rb.AddForce(Vector3.up * 1500.0f);

        // 폭발 전달
        IndirectDamage(tr.position);
        
        // 3초 후 드럼통 제거
        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        // 반경내의 드럼통 수집
        // 아래와 같이 진행하면, 메모리 Garbage가 발생한다.
        // 검출될 갯수가 명확하지 않다면 사용해야하기는 한다.
        // Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        // 메모리 Garbage가 일어나지 않고 사용하기 위한 방법
        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 3);
        
        foreach (var coll in colls)
        {
            // 10개가 감지 되지 않았을 경우, 배열안에 null이 있음
            // 이로 인해 드럼통이 삭제되지 않는 버그 있음.
            if (coll == null)
            {
                break;
            }
            
            // 드럼통 Rigidbody 추출
            rb = coll.GetComponent<Rigidbody>();
            
            // 무게를 가볍게 한다 
            rb.mass = 1.0f;
            
            // freezeRotation 제한값 해제
            rb.constraints = RigidbodyConstraints.None;
            
            // 폭발력 전달
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
