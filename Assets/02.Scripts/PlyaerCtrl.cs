using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlyaerCtrl : MonoBehaviour
{
    [SerializeField]
    private Transform tr;

    private Animation anim;
    
    public float moveSpeed = 10.0f; // 움직이는 속도 정의
    public float turnSpeed = 80.0f; // 회전속도 정의

    // 초기 체력 값
    private readonly float initHP = 100.0f;
    // 현재 체력 값
    public float currHP;
    // HP Bar에 연결할 변수
    private Image hpBar;
    
    // 델리게이트 선언
    public delegate void PlayerDieHander();
    // 이벤트 선언
    public static event PlayerDieHander OnPlayerDie;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // HP Bar 로드
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        
        // 초기 체력값 설정
        currHP = initHP;
        
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.Play("Idle");

        // 게임 시작시, 마우스 위치로 인하여 플레이어가 보는 방향이 확 이동될 때.
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // 수평 Input 값
        float v = Input.GetAxis("Vertical");    // 수직 Input 값
        float r = Input.GetAxis("Mouse X");     // 마우스 수평 Input 값
        
        // Debug.Log("h=" + h);
        // Debug.Log("v=" + v);

        // 움직임 저장
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        
        // 벡터의 크기를 확인하기 위한 디버그
        // Debug.Log(moveDir.normalized.magnitude);
        
        // 이동과 회전에는 Time.deltaTime이 꼭 필요.
        // 각 PC들 마다 재생되는 프레임 속도가 다르기 때문에 일괄된 결과가 나올려면 필요하다.
        
        // 이동
        tr.Translate(moveDir.normalized * (Time.deltaTime * moveSpeed));
        
        // 회전
        tr.Rotate(Vector3.up * (turnSpeed * Time.deltaTime * r));
        
        // 플레이어 애니메이션 동작
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        if (v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f); // 전진 애니메이션
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f); // 후진 애니메이션
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f); // 오른쪽 애니메이션
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f); // 왼쪽 애니메이션
        }
        else
        {
            anim.CrossFade("Idle", 0.25f); // 아무것도 하지 않았다면 대기
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currHP >= 0.0f && other.CompareTag("PUNCH"))
        {
            currHP -= 10.0f;
            // 체력 보여주기
            DisplayHealth();
            
            Debug.Log($"Player HP = {currHP/initHP}");

            if (currHP <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private void PlayerDie()
    {
        Debug.Log("Player Dead!");

        // GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        //
        // foreach (GameObject monster in monsters)
        // {
        //     monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        // }

        OnPlayerDie();
    }

    private void DisplayHealth()
    {
        hpBar.fillAmount = currHP / initHP;
    }
}
