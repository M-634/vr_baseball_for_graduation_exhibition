using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bat : MonoBehaviour,IBallHitObjet
{
    [SerializeField] Transform hand;
    [SerializeField] float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand.Rotate(Vector3.up, angle);
    }

    public void OnHit(Rigidbody rb, Vector3 normal, float ballSpeed)
    {
        rb.velocity = DecideVelocity(normal) * BattingPower(ballSpeed);
        //Debug.Log("Hit!!");
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
