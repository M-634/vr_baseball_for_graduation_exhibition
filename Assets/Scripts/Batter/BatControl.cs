using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �o�b�g������ł��Ԃ����ɔ�������͊w�I�ȉ^���𐧌䂷��N���X.
/// </summary>
public class BatControl : MonoBehaviour,IBallHitObjet
{
    [SerializeField] Transform hand;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

  
    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        rb.velocity = DecideVelocity(hitObjectInfo.normal) * BattingPower(ballSpeed);
    }

    private Vector3 DecideVelocity(Vector3 normal)
    {
        return normal;
    }

    private float BattingPower(float ballSpeed)
    {
        return ballSpeed;
    }

}
