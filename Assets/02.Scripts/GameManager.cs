using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 몬스터가 출현할 위치를 저장할 배열
    public List<Transform> points = new List<Transform>();
    
    // 몬스터 프리팹 변수
    public GameObject monster;
    
    // 몬스터 생성 간격
    public float createTime = 3.0f;
    
    // 게임 종료 여부
    private bool isGameOver;
    
    // 게임 종류 여부 프로퍼티
    public bool IsGameOver
    {
        get {return isGameOver;}
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // 스폰 포인트 게임 오브젝트 찾기
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        // 하위 스폰 포인트들을 불러오기
        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }
        
        // 일정한 간격으로 함수를 호출
        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }

    void CreateMonster()
    {
        // 몬스터를 불규칙한 위치에 생성하기 위한 랜덤
        int idx = Random.Range(0, points.Count);
        
        // 몬스터 프리팹 생성
        Instantiate(monster, points[idx].position, points[idx].rotation);
    }
}
