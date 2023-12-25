using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // 총알
    public GameObject bullet;

    // 총알 발사 위치
    public Transform firePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
