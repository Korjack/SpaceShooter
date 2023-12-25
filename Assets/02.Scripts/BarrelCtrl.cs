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

        // 무게를 가볍게 변경
        rb.mass = 1.0f;
        
        // 위쪽 방향으로 힘 추가
        rb.AddForce(Vector3.up * 1500.0f);
        
        // 3초 후 드럼통 제거
        Destroy(gameObject, 3.0f);
    }
}
