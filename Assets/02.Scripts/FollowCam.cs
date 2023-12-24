using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 따라가야할 오브젝트
    public Transform targetTr;
    private Transform camTr;
    
    // 따라갈 대상으로부터 떨어질 거리
    [Range(2.0f, 20.0f)] public float distance = 10.0f;

    // Y축으로 이동할 높이
    [Range(0.0f, 10.0f)] public float height = 2.0f;
    
    // 반응 속도
    public float damping = 10.0f;
    
    // SmoothDamp에 사용될 변수
    private Vector3 velocity = Vector3.zero;
    
    // 카메라 LookAt의 Offset 값
    public float targetOffset = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 targetPos = targetTr.position;
        
        // 대상의 뒤쪽 거리 = distance 만큼 이동
        // 대상의 높이 = height 만큼 이동
        Vector3 pos= targetPos + (-targetTr.forward * distance) + (Vector3.up * height);

        // 구면 선형 보간함수를 사용해 부드럽게 위치 변경
        // camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);
        
        // SmoothDamp를 이용한 위치 보간
        // 시작 위치, 목표 위치, 현재 속도, 목표 도달까지의 시간
        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);
        
        // 카메라를 피벗 좌표를 향해 회전
        camTr.LookAt(targetPos + targetTr.up * targetOffset);
    }
}
