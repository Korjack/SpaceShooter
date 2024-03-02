using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private Animator _animator;
    private new Transform _transform;
    private Vector3 moveDir;

    private PlayerInput _playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _playerInput = GetComponent<PlayerInput>();

        #region INPUT_SYSTEM

        // ActionMap 추출
        mainActionMap = _playerInput.actions.FindActionMap("PlayerActions");
        
        // Move, Attack 액션 추출
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");
        
        // Move 액션의 performed 이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 direction = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(direction.x, 0, direction.y);
            _animator.SetFloat("Movement", direction.magnitude);
        };
        
        // Move 액션의 canceled 이벤트 연결
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            _animator.SetFloat("Movement", 0.0f);
        };
        
        // Attack 액션의 performed 이벤트 연결
        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# Event");
            _animator.SetTrigger("Attack");
        };

        #endregion
    }

    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            // 진행 방향으로 회전
            _transform.rotation = Quaternion.LookRotation(moveDir);
            // 회전한 후 전진 방향으로 이동
            _transform.Translate(Vector3.forward * (Time.deltaTime * 4.0f));
        }
    }

    #region SEND_MESSAGE

    void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        moveDir = new Vector3(direction.x, 0, direction.y);
        
        _animator.SetFloat("Movement", direction.magnitude);
        Debug.Log($"Move = ({direction.x}, {direction.y})");
    }

    void OnAttack()
    {
        Debug.Log("Attack");
        _animator.SetTrigger("Attack");
    }

    #endregion

    #region UNITY_EVENTS

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        
        // 2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(direction.x, 0, direction.y);
        
        // Warrior_Run 애니매이션 실행
        _animator.SetFloat("Movement", direction.magnitude);
    }
    
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase = {ctx.phase}");

        if (ctx.performed)
        {
            Debug.Log("Attack");
            _animator.SetTrigger("Attack");
        }
    }
  

    #endregion
    
}
