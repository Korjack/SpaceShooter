using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // 컴포넌트 캐시 처리를 위한 변수
    private CharacterController _controller;
    private new Transform transform;
    private Animator _animator;
    private new Camera camera;
    
    // 가상의 Plane에 레이캐스팅 하기 위한 변수
    private Plane _plane;
    private Ray _ray;
    private Vector3 hitPoint;

    public float moveSpeed = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        camera = Camera.main;
        
        // 가상의 바닥을 주인공의 위치를 기준으로 생성
        _plane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
    }
    
    //키보드 입력 값
    private float h => Input.GetAxis("Horizontal");
    private float v => Input.GetAxis("Vertical");

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;
        
        // 이동할 방향 벡터 계산
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);
        
        // 주인공 캐릭터 이동 처리
        _controller.SimpleMove(moveDir * moveSpeed);
        
        // 주인공 캐릭터의 애니메이션 처리
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        
        _animator.SetFloat("Forward", forward);
        _animator.SetFloat("Strafe", strafe);
    }
    
    void Turn()
    {
        // 마우스의 2차원 좌표값을 이용해 3차원 광선을 생성
        _ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;
        
        // 가상의 바닥에 레이를 발사해 충돌한 지점의 거리를 enter 변수로 반환
        _plane.Raycast(_ray, out enter);
        // 가상의 바닥에 레이가 충돌한 좌표값 추출
        hitPoint = _ray.GetPoint(enter);
        
        // 회전해야 할 방향의 벡터를 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;
        // 주인공 캐릭터의 회전값 지정
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }
}
