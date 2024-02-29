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

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
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
}
