using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterCtrl : MonoBehaviour
{
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent _agent;
    
    // 몬스터 상태 정보
    public enum State 
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    // 몬스터 현재 상태
    public State state = State.IDLE;
    // 추적 사정거리
    public float traceDist = 10.0f;
    // 공격 사정거리
    public float attackDis = 2.0f;
    // 몬스터 사망 여부
    public bool isDie = false;
    
    // 혈흔 효과 프리팹
    private GameObject bloodEffect;
    
    //애니메이터
    private Animator anim;
    private static readonly int IsTrace = Animator.StringToHash("IsTrace");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int PlayerDie = Animator.StringToHash("PlayerDie");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Die = Animator.StringToHash("Die");

    private int hp = 100;

    private void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    // Start is called before the first frame update
    void Start()
    {
        // 각 컴포넌트 로드
        // Awake에서 이미 로드했기 때문에 주석 처리
        // monsterTr = GetComponent<Transform>();
        // playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        // _agent = GetComponent<NavMeshAgent>();
        // anim = GetComponent<Animator>();
        //
        // bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        // 몬스터 추적 바로 시작
        // _agent.destination = playerTr.position;

        // 몬스터 상태 업로드 코루틴 호출
        // StartCoroutine(CheckMonsterState());
        //
        // // 몬스터 상태에 따른 행동
        // StartCoroutine(MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 0.3초 대기
            yield return new WaitForSeconds(0.3f);
            
            // 몬스터가 죽었다면 코루틴 종료
            if(state == State.DIE) yield break;
            
            // 몬스터와 주인공 사이의 거리 측정
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            
            // 공격 사정거리 범위로 들어왔는지 확인
            if (distance <= attackDis)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 범위 안에 들어왔다면?
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDE 상태
                case State.IDLE:
                    // 추격 중단
                    _agent.isStopped = true;
                    // 에니매이터의 IsTrace 변수를 false
                    anim.SetBool(IsTrace, false);
                    break;
                // 추적 상태
                case State.TRACE:
                    // 추격 대상으로 이동
                    _agent.SetDestination(playerTr.position);
                    _agent.isStopped = false;
                    
                    // 에니매이터의 IsTrace 변수를 true
                    anim.SetBool(IsTrace, true);
                    
                    // 에니메이터의 IsAttack 변수를 false
                    anim.SetBool(IsAttack, false);
                    break;
                // 공격 상태
                case State.ATTACK:
                    // 에니메이터의 IsAttack 변수를 true
                    anim.SetBool(IsAttack, true);
                    break;
                // 사망
                case State.DIE:
                    isDie = true;

                    // 추적 정지
                    _agent.isStopped = true;
                    // 사망 애니메이션 수행
                    anim.SetTrigger(Die);
                    // 몬스터의 Collider 컴포넌트 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    
                    // 몬스터가 사망시 50점 추가
                    GameManager.instance.DisplayScore(50);

                    yield return new WaitForSeconds(3.0f);
                    
                    // 사망 후 초기화
                    hp = 100;
                    isDie = false;
                    state = State.IDLE;
                    
                    // 몬스터 Collider 컴포넌트 활성화
                    GetComponent<CapsuleCollider>().enabled = true;
                    // 몬스터 비활성화
                    this.gameObject.SetActive(false);
                    
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 부딫힌게 총알이면
        if (other.collider.CompareTag("BULLET"))
        {
            // 총알 삭제
            Destroy(other.gameObject);
            // // 피격 리액션 에니메이션 실행
            // anim.SetTrigger(Hit);
            //
            // // 총알 충돌 지점
            // Vector3 pos = other.GetContact(0).point;
            // // 총알의 충돌 지점의 법선 벡터
            // Quaternion rot = Quaternion.LookRotation(-other.GetContact(0).normal);
            // // 혈흔 효과를 생성하는 함수
            // ShowBloodEffect(pos, rot);
            //
            // hp -= 10;
            // if (hp < 0)
            // {
            //     state = State.DIE;
            // }
        }
    }
    
    // 레이캐스트를 사용해 데미지를 입히는 로직
    public void OnDamge(Vector3 pos, Vector3 normal)
    {
        // 피격 리액션 에니메이션 실행
        anim.SetTrigger(Hit);
        
        // 총알의 충돌 지점의 법선 벡터
        Quaternion rot = Quaternion.LookRotation(normal);
        // 혈흔 효과를 생성하는 함수
        ShowBloodEffect(pos, rot);

        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // 혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }

    private void OnDrawGizmos()
    {
        // 추적 사정거리 표기
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        
        // 공격 사정거리 표기
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDis);
            
        }
    }

    private void OnPlayerDie()
    {
        // 몬스터 상태를 체크하는 코루틴 모두 정지
        StopAllCoroutines();
        
        // 추적을 정지하고 애니메이션 수행
        _agent.isStopped = true;
        anim.SetFloat(Speed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(PlayerDie);
    }

    // 스크립트가 활성화될 때마다 호출되는 함수
    private void OnEnable()
    {
        //이벤트 발생시 수행할 함수 연결
        PlyaerCtrl.OnPlayerDie += OnPlayerDie;
        
        // 몬스터 상태 업로드 코루틴 호출
        StartCoroutine(CheckMonsterState());

        // 몬스터 상태에 따른 행동
        StartCoroutine(MonsterAction());
    }

    // 스크립트가 비활성화될 떄마다 호출되는 함수
    private void OnDisable()
    {
        // 기존에 연결된 함수 해제
        PlyaerCtrl.OnPlayerDie -= OnPlayerDie;
    }

    // 몬스터의 몸과 양쪽 팔의 물리적 충돌 감지를 위해 추가한 트리거
    // 현재 레이어 기준으로 나눠놔서, 감지되지 않음.
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log(other.gameObject.name);
    // }
}
