using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_BaseBall.Test
{
    public class GenerateBall : MonoBehaviour
    {
        [SerializeField] GameObject ballPrefab;
        [SerializeField] float interval = 2f;

        float previousTime;


        // Update is called once per frame
        void Update()
        {
            if (Time.time - previousTime > interval)
            {
                Instantiate(ballPrefab, transform.position, Quaternion.identity);
                previousTime = Time.time;
            }
        }
    }
}
