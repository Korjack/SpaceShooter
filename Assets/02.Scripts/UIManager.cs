using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 버튼 변수 3개
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction _action;

    private void Start()
    {
        // UnityAction을 사용한 이벤트 연결 방식
        _action = () => OnStartClick();
        startButton.onClick.AddListener(_action);
        
        // 무명 메서드를 활용한 이벤트 연결 방식
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });
        
        // 람다식을 활용한 이벤트 연결 방식
        shopButton.onClick.AddListener(()=> OnButtonClick(shopButton.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Button Clicked!  {msg}");
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Level_01");
        SceneManager.LoadScene("Play", LoadSceneMode.Additive);
    }
}
