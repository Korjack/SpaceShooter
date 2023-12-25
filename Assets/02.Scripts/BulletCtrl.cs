using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 총알 데미지
    public float damage = 20.0f;
    // 총알 발사 힘
    public float force = 1500.0f;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // 총알은 앞으로만 이동 (힘을 증가 시킴)
        // rb.AddForce(transform.forward * force);
        
        // 로컬 좌표계 기준으로 힘을 가함
        rb.AddRelativeForce(Vector3.forward * force);
    }
}
