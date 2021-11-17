using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bat : MonoBehaviour,IBallHitObjet
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

    public void OnHit(Rigidbody rb, Vector3 normal, float ballSpeed)
    {
        rb.velocity = DecideVelocity(normal) * BattingPower(ballSpeed);
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
