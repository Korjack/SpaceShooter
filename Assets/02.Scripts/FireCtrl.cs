using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 반드시 필요한 컴포넌트를 명시해 해당 컴포넌트가 삭제되는 것을 방지하는 속성
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // 총알
    public GameObject bullet;

    // 총알 발사 위치
    public Transform firePos;
    
    // 총소리에 사용될 오디오
    public AudioClip fireSfx;
    
    // AudioSource 컴포넌트를 저장할 변수
    private new AudioSource audio;
    
    // Muzzle Flash의 MeshRenderer 컴포넌트
    private MeshRenderer muzzleFlash;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        
        // FirePos 하위에 있는 MuzzleFlash 추출
        muzzleFlash = GetComponentInChildren<MeshRenderer>();
        
        // 처음 시작시 비활성화
        muzzleFlash.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // 총알 생성
        Instantiate(bullet, firePos.position, firePos.rotation);
        
        // 총소리 발생
        audio.PlayOneShot(fireSfx, 1.0f);
    }
}
