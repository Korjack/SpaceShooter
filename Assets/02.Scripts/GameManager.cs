using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // 몬스터가 출현할 위치를 저장할 배열
    public List<Transform> points = new List<Transform>();
    
    // 몬스터를 미리 새성해 저장할 리스트 자료형
    public List<GameObject> monsterPool = new List<GameObject>();
    
    // 오프젝트 풀에 생성할 몬스터의 최대 개수
    public int maxMonsters = 10;
    
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

    // 스코어 텍스트
    public TMP_Text scoreText;
    // 누적 점수를 기록하기 위한 변수
    private int totScore = 0;
    
    // 싱글턴을 위한 인스턴스 생성
    public static GameManager instance = null;

    private void Awake()
    {
        // 인스턴스가 없는 경우
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        
        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();
        
        // 스폰 포인트 게임 오브젝트 찾기
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        // 하위 스폰 포인트들을 불러오기
        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }
        
        // 일정한 간격으로 함수를 호출
        InvokeRepeating("CreateMonster", 2.0f, createTime);
        
        // 스코어 점수 출력
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DisplayScore(totScore);
    }

    void CreateMonster()
    {
        // 몬스터를 불규칙한 위치에 생성하기 위한 랜덤
        int idx = Random.Range(0, points.Count);
        
        // 몬스터 프리팹 생성
        // Instantiate(monster, points[idx].position, points[idx].rotation);

        // 오브젝트 풀에서 몬스터 추출
        GameObject _monster = GetMonsterInPool();
        // 추출한 몬스터 위치 및 회전 설정
        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
        
        // 추출한 몬스터 활성화
        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            // 몬스터 프리팹 생성
            GameObject _monster = Instantiate<GameObject>(monster);
            
            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";
            
            // 몬스터 비활성화
            _monster.SetActive(false);
            
            // 몬스터 리스트에 추가
            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            // 비활성화 여부로 사용 가능한 몬스터를 판단
            if (_monster.activeSelf == false)
            {
                // 몬스터 반환
                return _monster;
            }
        }

        return null;
    }

    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE :</color> <color=#ff0000>{totScore:#,##0}</color>";
        
        // 스코어 저장
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}
